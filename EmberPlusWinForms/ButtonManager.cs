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

            for (int i = 0; i < 4; i++)   // 40
            {
                int index = i;
                var button = _buttons[i];
                var parameter = _parameters[i];

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

                parameter.PropertyChanged += (sender, args) =>   // Handle receiving data (Ember+ update UI)
                {
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

        






















        //////public void InitializeButtons()
        //////{
        //////    //int i = 1;
        //////    //for (int i = 0; i < _buttons.Count && i < _parameters.Count; i++)
        //////    for (int i = 0; i < 40; i++)
        //////    {
        //////        int index = i; // Capture current index
        //////        var button = _buttons[i];
        //////        var parameter = _parameters[i];

        //////        //button.BackColor = (bool)_parameters[i].Value ? Color.Red : Color.Green;

        //////        if ((bool)_parameters[i].Value == true)
        //////        {
        //////            if (Convert.ToInt32(_parameters[i + 80].Value) == 0)
        //////                button.BackColor = Color.Transparent;
        //////            else if (Convert.ToInt32(_parameters[i + 80].Value) == 1)
        //////                button.BackColor = Color.Red;
        //////            else if (Convert.ToInt32(_parameters[i + 80].Value) == 2)
        //////                button.BackColor = Color.Green;
        //////            else if (Convert.ToInt32(_parameters[i + 80].Value) == 3)
        //////                button.BackColor = Color.Yellow;
        //////        }
        //////        else if ((bool)_parameters[i].Value == false)
        //////        {
        //////            if (Convert.ToInt32(_parameters[i + 120].Value) == 0)
        //////                button.BackColor = Color.Transparent;
        //////            else if (Convert.ToInt32(_parameters[i + 120].Value) == 1)
        //////                button.BackColor = Color.Red;
        //////            else if (Convert.ToInt32(_parameters[i + 120].Value) == 2)
        //////                button.BackColor = Color.Green;
        //////            else if (Convert.ToInt32(_parameters[i + 120].Value) == 3)
        //////                button.BackColor = Color.Yellow;
        //////        }


        //////        // Handle incoming data (Ember+ updates UI)
        //////        parameter.PropertyChanged += (sender, args) =>
        //////        {
        //////            bool state = (bool)((IParameter)sender).Value;
        //////            //button.BackColor = state ? Color.Red : Color.Green;

        //////            var param = sender as IParameter;

        //////            int t = param.Path[2];

        //////            if (state == true)  // state of the switch
        //////            {
        //////                if (Convert.ToInt32(_parameters[index + 80].Value) == 0)
        //////                    button.BackColor = Color.Transparent;
        //////                else if (Convert.ToInt32(_parameters[index + 80].Value) == 1)
        //////                    button.BackColor = Color.Red;
        //////                else if (Convert.ToInt32(_parameters[index + 80].Value) == 2)
        //////                    button.BackColor = Color.Green;
        //////                else if (Convert.ToInt32(_parameters[index + 80].Value) == 3)
        //////                    button.BackColor = Color.Yellow;
        //////            }
        //////            else if (state == false)
        //////            {
        //////                if (Convert.ToInt32(_parameters[index + 120].Value) == 0)
        //////                    button.BackColor = Color.Transparent;
        //////                else if (Convert.ToInt32(_parameters[index + 120].Value) == 1)
        //////                    button.BackColor = Color.Red;
        //////                else if (Convert.ToInt32(_parameters[index + 120].Value) == 2)
        //////                    button.BackColor = Color.Green;
        //////                else if (Convert.ToInt32(_parameters[index + 120].Value) == 3)
        //////                    button.BackColor = Color.Yellow;
        //////            }


        //////            Form1.instanse.listBox1.Items.Add($"IN <---- module: {t}");
        //////            Form1.instanse.listBox1.SelectedIndex = Form1.instanse.listBox1.Items.Count - 1;


        //////        };

        //////       // _parameters[i + 40].PropertyChanged += (sender, args) =>
        //////        //{

        //////        //    Form1.instanse.listBox1.Items.Add($"IN <---- mode: {_parameters[40].Value}");
        //////        //    Form1.instanse.listBox1.SelectedIndex = Form1.instanse.listBox1.Items.Count - 1;

        //////        //    //bool state = (bool)((IParameter)sender).Value;

        //////        //};



        //////        // Handle outgoing data (Button click sends value to Ember+)
        //////        button.Click += async (s, e) =>
        //////        {
        //////            await SyncButtonToEmberAsync(button, parameter);
        //////        };
        //////    }
        //////}

        /// <summary>
        /// Sends the button state to the parameter when clicked.
        /// </summary>
        //////private async Task SyncButtonToEmberAsync(Button button, IParameter parameter)
        //////{
        //////    // Ensure Tag is a boolean
        //////    if (button.Tag is bool state)
        //////    {
        //////        parameter.Value = state;
        //////        // parameter.Value = state ? (Boolean)1 : (bool)0; // Fix: convert bool to long
        //////        //Form1.instanse.listBox1.Items.Add($"OUT ---->  Tag: {state}");
        //////        //Form1.instanse.listBox1.SelectedIndex = Form1.instanse.listBox1.Items.Count - 1;
        //////    }
        //////    // Simulate async work (optional)
        //////    await Task.CompletedTask;
        //////}
    }

}
