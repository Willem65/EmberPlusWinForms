using Lawo.EmberPlusSharp.Model;
using TrackBar = System.Windows.Forms.TrackBar;

namespace EmberPlusWinForms
{
    public partial class FaderManager
    {
        private readonly List<IParameter> _faderParams;
        private readonly List<TrackBar> _trackBars;
        private readonly Form _form;

        public FaderManager( List<IParameter> faderParams, List<TrackBar> trackBars)
        {
            _faderParams = faderParams;
            _trackBars = trackBars;            
        }

 //------------------------------------------------------------------------------------------------------------

        public void InitializeFaders()
        {           
            for (int i = 0; i < _faderParams.Count; i++)
            {
                int index = i;
                var faderParam = _faderParams[index];
                var trackBar = _trackBars[index];
                trackBar.Value = (int)(long)_faderParams[index].Value;

                faderParam.PropertyChanged += async (sender, args) =>        // Handle receiving data (Ember+ update UI)
                {
                    await Task.Delay(20); // Simulating async work
                    long updatedValue = (long)((IParameter)sender).Value;
                    int clampedValue = Math.Min(Math.Max((int)updatedValue, trackBar.Minimum), trackBar.Maximum);
                    if (trackBar.Value != clampedValue)
                        trackBar.Value = clampedValue;
                    if (Form1.instanse.checkboxloggingEnabled)
                    {
                        Form1.instanse.listBox1.Items.Add("IN <---- " + updatedValue.ToString());
                        Form1.instanse.listBox1.SelectedIndex = Form1.instanse.listBox1.Items.Count - 1;
                    };
                };
                
                trackBar.Scroll += async (s, e) =>                // Handle sending outgoing data (Button click sends value to Ember+)
                {
                    await SyncTrackBarToEmberAsync(trackBar, faderParam, index);
                };
            }
        }
       
        public async Task SyncTrackBarToEmberAsync(TrackBar tb, IParameter param, int index)     // Om een waarde voor de trackbar scroll te verzenden
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
