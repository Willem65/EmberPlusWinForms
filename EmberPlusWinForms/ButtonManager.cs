using Lawo.EmberPlusSharp.Model;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace EmberPlusWinForms
{
    class ButtonManager
    {       
        private readonly List<IParameter> _parameters;
        private readonly List<Button> _buttons;
        private readonly Dictionary<Button, System.Windows.Forms.Timer> _blinkTimers = new();
        private readonly Dictionary<Button, Stopwatch> _stopwatches = new();


        public ButtonManager( List<IParameter> parameters, List<Button> buttons)  // Lawo parameters , Windows buttons
        {
            _parameters = parameters;
            _buttons = buttons;
        }

        public async Task InitializeButtonsAsync()
        {
            var tasks = _parameters.Select(param => WaitForParameterChange(param)).ToArray();
            // ⚡ Start waiting for all parameters in the background (DON'T block execution)
            _ = Task.WhenAll(tasks);

            for (int i = 0; i < _buttons.Count; i++)   // 40
            {
                int index = i;                         // index copy is nodig, omdat het vanuit async event handlers wordt aangeroepen
                var button = _buttons[index];          // Op dit moment staan er meteen al 40 buttons in een array
                var parameter = _parameters[index];    // Op dit moment staan er meteen al 160 parameters in een array,
                                                       // namelijk 0-40 de state van de button,
                                                       // 40-80 de blink-mode en
                                                       // 80-120 de on_color
                                                       // 120-160 de off_color van de button




                parameter.PropertyChanged += async (sender, args) =>   // Receiving data (Ember+ update UI)
                {
                    var param = sender as IParameter;
                    button.Text = param.Path[2].ToString() + "_" + param.Path[4].ToString();  // Tijdelijk even de knop nummers weergeven
                    button.BackColor = DetermineBackColor((bool)param.Value, index);

                    _blinkTimers[button].Start(); // Start blinking
                };



                //--------------------------------------------------------------------------------------------------------------------------------------------------------
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

            if (!(state))         // Stand van de switch true ?
                offset = _buttons.Count * 2;   // De on_color vanaf 80
            else 
                offset = _buttons.Count * 3;  // De off_color vanaf 120

            var btncolor = Convert.ToInt32(_parameters[index + offset].Value) switch
            {
                0 => Color.Transparent,
                1 => Color.LightSalmon,
                2 => Color.LightGreen,
                3 => Color.Yellow,
                 _ => Color.Transparent   // Default , voor de veiligheid, als er een onbekende waarde is
            };

            return btncolor;
        }



        private void ToggleButtonBlink(Button button)
        {
            button.BackColor = button.BackColor == Color.Red ? Color.Transparent : Color.Red;
        }



        //---- Geen idee of dit nodig is, het is de bedoeling dat er gewacht word totdat alle parameters binnen zijn gekomen ----
        private Task WaitForParameterChange(IParameter parameter)
        {
            var tcs = new TaskCompletionSource<bool>();

            PropertyChangedEventHandler handler = null;
            handler = (sender, args) =>
            {
                parameter.PropertyChanged -= handler; // Remove handler to prevent leaks
                tcs.TrySetResult(true); // Mark task as completed when parameter updates
            };
            parameter.PropertyChanged += handler; // Attach event
            return tcs.Task;
        }
        //-----------------------------------------------------------------------------------------------


    }
}
