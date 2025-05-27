using Lawo;
using Lawo.EmberPlusSharp.Model;

namespace EmberPlusWinForms
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    class ButtonManager
    {       
        private readonly List<IParameter> _parameters;
        private readonly List<Button> _buttons;
        private Timer timer2;
        private List<Button> blinkingButtons = new List<Button>();
        private Dictionary<Button, Color> originalColors = new Dictionary<Button, Color>();

        private void timer2_Tick(object sender, EventArgs e)  // laat de knoppen knipperen
        {        
            for (int i = 0; i < blinkingButtons.Count; i++)
            {
                Button btn = blinkingButtons[i];
                Color originalColor = originalColors[btn];
                btn.BackColor = btn.BackColor == originalColor ? Color.Transparent : originalColor;
            }
        }

        public ButtonManager( List<IParameter> parameters, List<Button> buttons)
        {
            _parameters = parameters;
            _buttons = buttons;
        }

//------------------------------------------------------------------------------------------------------------

        public void InitializeButtons()
        {
            timer2 = new Timer
            {
                Interval = 500 // Adjust the blink speed in milliseconds
            };

            timer2.Tick += timer2_Tick;

            for (int i = 0; i < 40; i++)   // 40
            {
                int index = i;
                var button = _buttons[index];
                var parameter = _parameters[index];

                button.BackColor = DetermineBackColor((bool)parameter.Value, index);
                int mode = Convert.ToInt32(_parameters[index + 40].Value);

                // Store its original background color
                if (!originalColors.ContainsKey(button))
                {
                    originalColors[button] = button.BackColor;
                }

                if (mode > 0)
                {
                    blinkingButtons.Add(button);
                    timer2.Start();
                }

                parameter.PropertyChanged += async  (sender, args) =>   // Handle receiving data (Ember+ update UI)
                {
                    await Task.Delay(20); // Simulating async work
                    var param = sender as IParameter;
                    button.Text = param.Path[2].ToString() + "_" + param.Path[4].ToString();  // Tijdelijk even de knop nummers weergeven

                    button.BackColor = DetermineBackColor((bool)param.Value, index);
                    int mode = Convert.ToInt32(_parameters[index + 40].Value);

                //    if (mode > 0)
                //    {
                //        blinkingButtons.Add(button);
                //        timer2.Start();
                //    }
                //    else
                //    {
                //        button.BackColor = DetermineBackColor((bool)parameter.Value, index);
                //    }

                    if (Form1.instanse.checkboxloggingEnabled)
                    {
                        Form1.instanse.listBox1.Items.Add($"IN <---- module: {mode}");
                        Form1.instanse.listBox1.SelectedIndex = Form1.instanse.listBox1.Items.Count - 1;
                    }
                };

                button.Click += async (s, e) =>            // Handle sending outgoing data (Button click sends value to Ember+)
                {
                    try
                    {
                        button.Enabled = false; // Prevent multiple clicks
                        await Task.Delay(10); // Simulating async work
                        //MessageBox.Show("Button clicked!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}");
                    }
                    finally
                    {
                        button.Enabled = true; // Re-enable button
                    }
                    await SyncButtonToEmberAsync(button, parameter);
                };
            }
        }

        private Color DetermineBackColor(bool state, int index)
        {
            int offset = 0; // = state ? 80 : 120;

            if (state)         // Stand van de switch true ?
                offset = 80;   // De on_color vanaf 80
            else 
                offset = 120;  // De off_color vanaf 120

            return Convert.ToInt32(_parameters[index + offset].Value) switch
            {
                0 => Color.Transparent,
                1 => Color.Red,
                2 => Color.Green,
                3 => Color.Yellow,
                _ => Color.Transparent
            };
        }

        private async Task SyncButtonToEmberAsync(Button button, IParameter parameter)
        {
            if (button.Tag is bool state)
            {
                parameter.Value = state;
                if (Form1.instanse.checkboxloggingEnabled)
                {
                    Form1.instanse.listBox1.Items.Add($"OUT ---->  Tag: {state}");
                    Form1.instanse.listBox1.SelectedIndex = Form1.instanse.listBox1.Items.Count - 1;
                }
            }
            // Simulate async work (optional)
            await Task.CompletedTask;
        }
    }
}
