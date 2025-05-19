using Lawo.EmberPlusSharp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using TrackBar = System.Windows.Forms.TrackBar;

namespace EmberPlusWinForms
{
    public partial class FaderManager
    {
       // private readonly Consumer<GetSet.AuronRoot> _consumer;
        private readonly List<IParameter> _faderParams;
        private readonly List<TrackBar> _trackBars;
        private readonly Form _form;
        //private object lsdata;


        public FaderManager(Form form, List<IParameter> faderParams, List<TrackBar> trackBars)
        {
            _form = form;
            _faderParams = faderParams;
            _trackBars = trackBars;            
        }

       // public Consumer<GetSet.AuronRoot>? consumer { get; private set; }

        public void InitializeFaders()
        {           
            for (int i = 0; i < _faderParams.Count; i++)
            {
                int index = i;
                var faderParam = _faderParams[index];
                var trackBar = _trackBars[index];
                trackBar.Value = (int)(long)_faderParams[index].Value;

                // Handle incoming data (Ember+ to UI)
                faderParam.PropertyChanged += (sender, args) =>
                {
                    long updatedValue = (long)((IParameter)sender).Value;
                    _ = _form.BeginInvoke((Action)(() =>
                    {
                        int clampedValue = Math.Min(Math.Max((int)updatedValue, trackBar.Minimum), trackBar.Maximum);
                        if (trackBar.Value != clampedValue)
                            trackBar.Value = clampedValue;

                        Form1.instanse.listBox1.Items.Add("IN <---- " + updatedValue.ToString());
                        Form1.instanse.listBox1.SelectedIndex = Form1.instanse.listBox1.Items.Count - 1;

                    }));
                };
                
                // Handle outgoing data (UI to Ember+)

                trackBar.Scroll += async (s, e) =>
                {
                    await SyncTrackBarToEmberAsync(trackBar, faderParam, index);
                };
            }
        }

        // Om een waarde voor de trackbar scroll te verzenden
        public async Task SyncTrackBarToEmberAsync(TrackBar tb, IParameter param, int index)
        {
            param.Value = (long)tb.Value;
            //await Task.Delay(10); // Simulating async operation
            Form1.instanse.listBox1.Items.Add("OUT ----> " + tb.Value.ToString());
            Form1.instanse.listBox1.SelectedIndex = Form1.instanse.listBox1.Items.Count - 1;
        }
    }
}
