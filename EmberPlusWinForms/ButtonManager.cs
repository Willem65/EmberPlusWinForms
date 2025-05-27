using Lawo;
using Lawo.EmberPlusSharp.Model;

namespace EmberPlusWinForms
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using static System.Windows.Forms.AxHost;

    class ButtonManager
    {       
        private readonly List<IParameter> _parameters;
        private readonly List<Button> _buttons;
        private Timer timer2;
        private List<Button> blinkingButtons = new List<Button>();
        private Dictionary<Button, Color> originalColors = new Dictionary<Button, Color>();

        //private void timer2_Tick(object sender, EventArgs e)  // laat de knoppen knipperen
        //{        
        //    for (int i = 0; i < blinkingButtons.Count; i++)
        //    {
        //        Button btn = blinkingButtons[i];
        //        Color originalColor = originalColors[btn];
        //        //btn.BackColor = btn.BackColor == originalColor ? Color.Transparent : originalColor;
        //        if (btn.BackColor == originalColor)
        //            btn.BackColor = Color.Transparent;
        //        else
        //            btn.BackColor = originalColor;

        //    }
        //}

        public ButtonManager( List<IParameter> parameters, List<Button> buttons)  // Lawo parameters , Windows buttons
        {
            _parameters = parameters;
            _buttons = buttons;
        }

        //------------------------------------------------------------------------------------------------------------
        private bool isOn = true;
        private bool state;

        public void InitializeButtons()
        {
            //timer2 = new Timer
            //{
            //    Interval = 500 // Adjust the blink speed in milliseconds
            //};

            //timer2.Tick += timer2_Tick;

            for (int i = 0; i < 40; i++)   // 40
            {
                int index = i;
                var button = _buttons[index];          // Op dit moment staan er meteen al 40 buttons in een array
                var parameter = _parameters[index];    // Op dit moment staan er meteen al 160 parameters in een array,
                                                       // namelijk 0-40 de state van de button,
                                                       // 40-80 de blink-mode en
                                                       // 80-120 de on_color
                                                       // 120-160 de off_color van de button

                //button.BackColor = DetermineBackColor((bool)parameter.Value, index);
                //int mode = Convert.ToInt32(_parameters[index + 40].Value);

                //// Store its original background color
                //if (!originalColors.ContainsKey(button))
                //{
                //    originalColors[button] = button.BackColor;
                //}

                //if (mode > 0)
                //{
                //    blinkingButtons.Add(button);
                //    timer2.Start();
                //}

                parameter.PropertyChanged += async  (sender, args) =>   // Receiving data (Ember+ update UI)
                {
                    //await Task.Delay(20); // Simulating async work
                    await Task.CompletedTask;
                    var param = sender as IParameter;
                    // button.Text = param.Path[2].ToString() + "_" + param.Path[4].ToString();  // Tijdelijk even de knop nummers weergeven

//--------------------------------------------------------------------------------------------------------------------------------------------------

                    state = (bool)(button.Tag ?? true); // default to true if Tag is null

                    Form1.instanse.listBox1.Items.Add($"          Receive-Statee: {state}");
                    Form1.instanse.listBox1.SelectedIndex = Form1.instanse.listBox1.Items.Count - 1;

                    //if ((bool)button.Tag)
                    //{
                    //    state = true;
                    //}
                    //else
                    //{
                    //    state = false;
                    //}

                    if (state)
                    {
                        button.BackColor = Color.LightGreen; // Set to green when ON , Het is niet de bedoeling dat de back color op deze wijze wordt aangepast, maar dat gebeurt nu wel voor testing
                        button.Text = "ON";
                        button.Text = param.Path[2].ToString() + "_" + param.Path[4].ToString();  // Tijdelijk even de knop nummers weergeven
                    }
                    else
                    {
                        button.BackColor = Color.LightSalmon; // Set to red when OFF
                        button.Text = "OFF";
                    }

                    //--------------------------------------------------------------------------------------------------------------------------------------------------



                    //    button.BackColor = DetermineBackColor((bool)param.Value, index);
                    //    int mode = Convert.ToInt32(_parameters[index + 40].Value);

                    //    if (mode > 0)
                    //    {
                    //        blinkingButtons.Add(button);
                    //        timer2.Start();
                    //    }
                    //    else
                    //    {
                    //        button.BackColor = DetermineBackColor((bool)parameter.Value, index);
                    //    }

                    //if (Form1.instanse.checkboxloggingEnabled)
                    //{
                    //    Form1.instanse.listBox1.Items.Add($"IN <---- module: {mode}");
                    //    Form1.instanse.listBox1.SelectedIndex = Form1.instanse.listBox1.Items.Count - 1;
                    //}
                };

                button.Click += async (s, e) =>   // Sending outgoing data (Button click sends value to Ember+)
                {
                    isOn ^= true;       // EXOR de state, toggle between true and false
                    button.Tag = isOn;

                    Form1.instanse.listBox1.Items.Add($"Send-State: {isOn}");
                    Form1.instanse.listBox1.SelectedIndex = Form1.instanse.listBox1.Items.Count - 1;
                    button.Refresh();


                    await SyncButtonToEmberAsync(button, parameter);
                };
            }
        }

        //private Color DetermineBackColor(bool state, int index)
        //{
        //    int offset = 0; // = state ? 80 : 120;

        //    if (state)         // Stand van de switch true ?
        //        offset = 80;   // De on_color vanaf 80
        //    else
        //        offset = 120;  // De off_color vanaf 120

        //    return Convert.ToInt32(_parameters[index + offset].Value) switch
        //    {
        //        0 => Color.Transparent,
        //        1 => Color.LightSalmon,
        //        2 => Color.LightGreen,
        //        3 => Color.Yellow,
        //        //  _ => Color.Transparent
        //    };
        //}

        private async Task SyncButtonToEmberAsync(Button button, IParameter parameter)  // Send the button state to Ember+ asynchronously
        {                                                                               // Dit gedaan deze functie vanaf een andere thread kan worden aangeroepen
            if (button.Tag is bool state)            {
               
                parameter.Value = state;
                //if (Form1.instanse.checkboxloggingEnabled)
                //{
                //    Form1.instanse.listBox1.Items.Add($"OUT ---->  Tag: {state}");
                //    Form1.instanse.listBox1.SelectedIndex = Form1.instanse.listBox1.Items.Count - 1;
                //}
            }
            // Simulate async work (optional)
            await Task.CompletedTask;
        }
    }
}
