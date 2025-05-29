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

        public ButtonManager( List<IParameter> parameters, List<Button> buttons)  // Lawo parameters , Windows buttons
        {
            _parameters = parameters;
            _buttons = buttons;
        }

        private bool isOn = true;
        private bool state;

        public void InitializeButtons()
        {
            for (int i = 0; i < _buttons.Count; i++)   // 40
            {
                int index = i;
                var button = _buttons[index];          // Op dit moment staan er meteen al 40 buttons in een array
                var parameter = _parameters[index];    // Op dit moment staan er meteen al 160 parameters in een array,
                                                       // namelijk 0-40 de state van de button,
                                                       // 40-80 de blink-mode en
                                                       // 80-120 de on_color
                                                       // 120-160 de off_color van de button

                parameter.PropertyChanged += async (sender, args) =>   // Receiving data (Ember+ update UI)
                {
                    await Task.CompletedTask;
                    var param = sender as IParameter;
                    button.Text = param.Path[2].ToString() + "_" + param.Path[4].ToString();  // Tijdelijk even de knop nummers weergeven
                    button.BackColor = DetermineBackColor((bool)param.Value, index);
                };

                button.MouseDown += async (s, e) =>   // Sending  (Button click sends value to Ember+)
                {
                    parameter.Value = true;
                };
                button.MouseUp += async (s, e) =>   // Sending  (Button click sends value to Ember+)
                {
                    parameter.Value = false;
                };
            }
        }

        private Color DetermineBackColor(bool state, int index)
        {
            int offset = 0; // = state ? 80 : 120;

            if (state)         // Stand van de switch true ?
                offset = _buttons.Count * 2;   // De on_color vanaf 80
            else 
                offset = _buttons.Count * 3;  // De off_color vanaf 120

            return Convert.ToInt32(_parameters[index + offset].Value) switch
            {
                0 => Color.Transparent,
                1 => Color.LightSalmon,
                2 => Color.LightGreen,
                3 => Color.Yellow,
                 _ => Color.Transparent   // Default , voor de veiligheid, als er een onbekende waarde is
            };
        }
    }
}
