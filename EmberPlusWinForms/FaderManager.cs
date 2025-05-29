using Lawo.EmberPlusSharp.Model;
using TrackBar = System.Windows.Forms.TrackBar;

namespace EmberPlusWinForms
{
    public partial class FaderManager
    {
        private readonly List<IParameter> _faderParams;
        private readonly List<TrackBar> _trackBars;

        public FaderManager( List<IParameter> faderParams, List<TrackBar> trackBars)
        {
            _faderParams = faderParams;
            _trackBars = trackBars;            
        }

        public void InitializeFaders()
        {           
            for (int i = 0; i < _faderParams.Count; i++)
            {
                int index = i;                        // index copy is nodig, omdat het vanuit async event handlers wordt aangeroepen
                var trackBar = _trackBars[index];        // Op dit moment staan er meteen  10 faders in een array
                var faderParam = _faderParams[index];    // en ook 10 keer de faderparameter in een array 

                trackBar.Value = (int)(long)_faderParams[index].Value;

                faderParam.PropertyChanged +=  (sender, args) =>        // Handle receiving data (Ember+ update UI)
                {
                   // await Task.Delay(20); // Simulating async work
                    long updatedValue = (long)((IParameter)sender).Value;
                    int clampedValue = Math.Min(Math.Max((int)updatedValue, trackBar.Minimum), trackBar.Maximum);
                    if (trackBar.Value != clampedValue)
                        trackBar.Value = clampedValue;
                    if (Form1.instanse.checkboxloggingEnabled)
                    {
                        Form1.instanse.listBox1.Items.Add("IN <---- " + updatedValue.ToString());
                        Form1.instanse.listBox1.SelectedIndex = Form1.instanse.listBox1.Items.Count - 1;
                    }
                };
                
                trackBar.Scroll +=  (s, e) =>                // Handle sending outgoing data (Button click sends value to Ember+)
                {
                    SyncTrackBarToEmberAsync(trackBar, faderParam);
                };
            }
        }
       
        public static void SyncTrackBarToEmberAsync(TrackBar tb, IParameter param)     // Om een waarde voor de trackbar scroll te verzenden
        {
            param.Value = (long)tb.Value;
            if (Form1.instanse.checkboxloggingEnabled)
            {
                Form1.instanse.listBox1.Items.Add("OUT ----> " + tb.Value.ToString());
                Form1.instanse.listBox1.SelectedIndex = Form1.instanse.listBox1.Items.Count - 1;
            }
        }
    }
}
