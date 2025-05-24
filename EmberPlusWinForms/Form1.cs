using Lawo.EmberPlusSharp.Model;
using Lawo.EmberPlusSharp.S101;
using System.Net.Sockets;
using TrackBar = System.Windows.Forms.TrackBar;
using Button = System.Windows.Forms.Button;

namespace EmberPlusWinForms
{
    public partial class Form1 : Form
    {
        public bool checkboxloggingEnabled;
        private FaderManager faderManager;
        private ButtonManager buttonManager;
        public static Form1? instanse;

        public Form1()   // constructor
        {
            InitializeComponent();
            instanse = this;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            RegisterButtonEvents();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Stop();
            Task startEmberPlusListenerAsync = StartEmberPlusListenerAsync();
            faderManager = new FaderManager(faderParams, trackBars);
            buttonManager = new ButtonManager(faderParams, buttons);
        }


        private void RegisterButtonEvents()
        {
            for (int i = 0; i < this.Controls.Count; i++)
            {
                if (this.Controls[i] is Button btn && btn.Name.StartsWith("button"))
                {
                    btn.Tag = true; // initial state ON
                    btn.Text = "ON";
                    btn.MouseDown += ToggleButton;
                }
            }
        }

        private void ToggleButton(object sender, MouseEventArgs e)
        {
            if (sender is Button btn)
            {
                bool state = (bool)(btn.Tag ?? true); // default to true if Tag is null
                state = !state;
                btn.Tag = state;
                btn.Text = state ? "ON" : "OFF";
            }
        }



        private static async Task<S101Client> ConnectAsync(string host, int port)
        {
            var tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(host, port); // Ensure this is awaited
            var stream = tcpClient.GetStream();
            await Task.Delay(300); // Optional delay if needed
            return new S101Client(tcpClient, stream.ReadAsync, stream.WriteAsync);
        }

        //-------------- Start -----------------------------------------------------------------------------------------------------------------------


        private sealed class MyRoot : DynamicRoot<MyRoot>
        {
        }

        public Consumer<GetSet.AuronRoot> consumer;
        private List<IParameter> faderParams;
        private INode root;

        private async Task StartEmberPlusListenerAsync()
        {
            using var client = await ConnectAsync("192.168.1.2", 9000);
            consumer = await Consumer<GetSet.AuronRoot>.CreateAsync(client);
            {
                var connectionLost = new TaskCompletionSource<Exception>();
                consumer.ConnectionLost += (s, e) => connectionLost.SetResult(e.Exception);
                await Task.Delay(500); // Optional delay if needed
                Form1.instanse.listBox1.Items.Add($" Connection Lost! ");
                Form1.instanse.listBox1.SelectedIndex = Form1.instanse.listBox1.Items.Count - 1;
            }

            root = consumer.Root;

            //------------- Faders ------------------------------------------------------------------------------------------------------------------------
            // First, gather your modules into an array for easier iteration.
            //
            faderParams = new List<IParameter>
            {
                consumer.Root.auron.modules.module_1.path.fader,
                consumer.Root.auron.modules.module_2.path.fader,
                consumer.Root.auron.modules.module_3.path.fader,
                consumer.Root.auron.modules.module_4.path.fader,
                consumer.Root.auron.modules.module_5.path.fader,
                consumer.Root.auron.modules.module_6.path.fader,
                consumer.Root.auron.modules.module_7.path.fader,
                consumer.Root.auron.modules.module_8.path.fader,
                consumer.Root.auron.modules.module_9.path.fader,
                consumer.Root.auron.modules.module_10.path.fader
            };

            var trackBars = new List<TrackBar>();
            for (int i = 1; i <= 10; i++)
            {
                var trackBar = (TrackBar)Controls["trackBar" + i];
                trackBars.Add(trackBar);
            }

            var faderManager = new FaderManager(faderParams, trackBars);
            faderManager.InitializeFaders();

            //-------------- Buttons -----------------------------------------------------------------------------------------------------------------------
            // First, gather your modules into an array for easier iteration.
            // 
            var modules = new[]
            {
                consumer.Root.auron.modules.module_1,
                consumer.Root.auron.modules.module_2,
                consumer.Root.auron.modules.module_3,
                consumer.Root.auron.modules.module_4,
                consumer.Root.auron.modules.module_5,
                consumer.Root.auron.modules.module_6,
                consumer.Root.auron.modules.module_7,
                consumer.Root.auron.modules.module_8,
                consumer.Root.auron.modules.module_9,
                consumer.Root.auron.modules.module_10
            };




            List<IParameter> buttonParams = new List<IParameter>();

            int aantalMoules = modules.Length;
            //int aantalMoules = 1;

            // Group 1: Add states for sw_1 through sw_4
            for (int i = 0; i < aantalMoules; i++)
            {
                buttonParams.Add(modules[i].control.sw_1.state);
                buttonParams.Add(modules[i].control.sw_2.state);
                buttonParams.Add(modules[i].control.sw_3.state);
                buttonParams.Add(modules[i].control.sw_4.state);
            }

            // Group 2: Add modes for sw_1 through sw_4
            for (int i = 0; i < aantalMoules; i++)
            {
                buttonParams.Add(modules[i].control.sw_1.mode);
                buttonParams.Add(modules[i].control.sw_2.mode);
                buttonParams.Add(modules[i].control.sw_3.mode);
                buttonParams.Add(modules[i].control.sw_4.mode);
            }

            // Group 3: Add color_on for sw_1 through sw_4
            for (int i = 0; i < aantalMoules; i++)
            {
                buttonParams.Add(modules[i].control.sw_1.color_on);
                buttonParams.Add(modules[i].control.sw_2.color_on);
                buttonParams.Add(modules[i].control.sw_3.color_on);
                buttonParams.Add(modules[i].control.sw_4.color_on);
            }

            // Group 4: Add color_off for sw_1 through sw_4
            for (int i = 0; i < aantalMoules; i++)
            {
                buttonParams.Add(modules[i].control.sw_1.color_off);
                buttonParams.Add(modules[i].control.sw_2.color_off);
                buttonParams.Add(modules[i].control.sw_3.color_off);
                buttonParams.Add(modules[i].control.sw_4.color_off);
            }

            var buttons = new List<Button>();
            for (int i = 1; i <= 40; i++)
            {
                var button = (Button)Controls["button" + i];
                buttons.Add(button);
            }

            var buttonManager = new ButtonManager(buttonParams, buttons);
            buttonManager.InitializeButtons();

            await Task.Delay(Timeout.Infinite);
        }




        //-------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Alle events van buttons, faders, timers en checkboxes zijn hier in de code onder gezet.
        /// </summary>
        //-------------------------------------------------------------------------------------------------------------------------------------


        static int tellertje;
        private List<TrackBar> trackBars;
        private List<Button> buttons;
        private object lstdata;


        private void timer1_Tick(object sender, EventArgs e)  // om de faders even te laten bewegen
        {
            // Increment the counter and wrap it around.
            tellertje = (tellertje + 1) % 1001;

            // Create an array of TrackBars for easy iteration.
            TrackBar[] trackBars = new TrackBar[]
            {
                trackBar1, trackBar2, trackBar3, trackBar4, trackBar5,
                trackBar6, trackBar7, trackBar8, trackBar9, trackBar10
            };

            // Update each trackBar's value only if it's different.
            for (int i = 0; i < trackBars.Length; i++)
            {
                if (trackBars[i].Value != tellertje)
                {
                    trackBars[i].Value = tellertje;
                }

                if (faderParams != null && i < faderParams.Count)
                {
                    _ = faderManager.SyncTrackBarToEmberAsync(trackBars[i], faderParams[i], i);
                }
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {

        }

        //private bool isOn = true;
        //private bool isOn2 = true;
        //private bool isOn3 = true;
        //private bool isOn4 = true;
        //private bool isOn5 = true;
        //private bool isOn6 = true;
        //private bool isOn7 = true;
        //private bool isOn8 = true;
        //private bool isOn9 = true;
        //private bool isOn10 = true;
        //private bool isOn11 = true;
        //private bool isOn12 = true;
        //private bool isOn13 = true;
        //private bool isOn14 = true;
        //private bool isOn15 = true;
        //private bool isOn16 = true;
        //private bool isOn17 = true;
        //private bool isOn18 = true;
        //private bool isOn19 = true;
        //private bool isOn20 = true;
        //private bool isOn21 = true;
        //private bool isOn22 = true;
        //private bool isOn23 = true;
        //private bool isOn24 = true;
        //private bool isOn25 = true;
        //private bool isOn26 = true;
        //private bool isOn27 = true;
        //private bool isOn28 = true;
        //private bool isOn29 = true;
        //private bool isOn30 = true;
        //private bool isOn31 = true;
        //private bool isOn32 = true;
        //private bool isOn33 = true;
        //private bool isOn34 = true;
        //private bool isOn35 = true;
        //private bool isOn36 = true;
        //private bool isOn37 = true;
        //private bool isOn38 = true;
        //private bool isOn39 = true;
        //private bool isOn40 = true;

        //private void button1_MouseDown(object sender, MouseEventArgs e)
        //{
        //    isOn = !isOn;
        //    button1.Tag = isOn;
        //    button1.Text = isOn ? "ON" : "OFF";
        //}

        //private void button2_MouseDown(object sender, MouseEventArgs e)
        //{
        //    isOn2 = !isOn2;
        //    button2.Tag = isOn2;
        //    button2.Text = isOn2 ? "ON" : "OFF";
        //}

        //private void button3_MouseDown(object sender, MouseEventArgs e)
        //{
        //    isOn3 = !isOn3;
        //    button3.Tag = isOn3;
        //    button3.Text = isOn3 ? "ON" : "OFF";
        //}

        //private void button4_MouseDown(object sender, MouseEventArgs e)
        //{
        //    isOn4 = !isOn4;
        //    button4.Tag = isOn4;
        //    button4.Text = isOn4 ? "ON" : "OFF";
        //}

        //private void button5_MouseDown(object sender, MouseEventArgs e)
        //{
        //    isOn5 = !isOn5;
        //    button5.Tag = isOn5;
        //    button5.Text = isOn5 ? "ON" : "OFF";
        //}

        //private void button6_MouseDown(object sender, MouseEventArgs e)
        //{
        //    isOn6 = !isOn6;
        //    button6.Tag = isOn6;
        //    button6.Text = isOn6 ? "ON" : "OFF";
        //}

        //private void button7_MouseDown(object sender, MouseEventArgs e)
        //{
        //    isOn7 = !isOn7;
        //    button7.Tag = isOn7;
        //    button7.Text = isOn7 ? "ON" : "OFF";
        //}

        //private void button8_MouseDown(object sender, MouseEventArgs e)
        //{
        //    isOn8 = !isOn8;
        //    button8.Tag = isOn8;
        //    button8.Text = isOn8 ? "ON" : "OFF";
        //}

        //private void button9_MouseDown(object sender, MouseEventArgs e)
        //{
        //    isOn9 = !isOn9;
        //    button9.Tag = isOn9;
        //    button9.Text = isOn9 ? "ON" : "OFF";
        //}

        //private void button10_MouseDown(object sender, MouseEventArgs e)
        //{
        //    isOn10 = !isOn10;
        //    button10.Tag = isOn10;
        //    button10.Text = isOn10 ? "ON" : "OFF";
        //}

        //private void button11_MouseDown(object sender, MouseEventArgs e)
        //{
        //    isOn11 = !isOn11;
        //    button11.Tag = isOn11;
        //    button11.Text = isOn11 ? "ON" : "OFF";
        //}

        //private void button12_MouseDown(object sender, MouseEventArgs e)
        //{
        //    isOn12 = !isOn12;
        //    button12.Tag = isOn12;
        //    button12.Text = isOn12 ? "ON" : "OFF";
        //}













        //private void button20_Click(object sender, EventArgs e)
        //{
        //    isOn = !isOn;
        //    button20.Tag = isOn;
        //}



        //private void button13_Click(object sender, EventArgs e)
        //{
        //    isOn = !isOn;
        //    button13.Tag = isOn;
        //}

        //private void button14_Click(object sender, EventArgs e)
        //{
        //    isOn = !isOn;
        //    button14.Tag = isOn;
        //}

        //private void button15_Click(object sender, EventArgs e)
        //{
        //    isOn = !isOn;
        //    button15.Tag = isOn;
        //}

        //private void button16_Click(object sender, EventArgs e)
        //{
        //    isOn = !isOn;
        //    button16.Tag = isOn;
        //}

        //private void button17_Click(object sender, EventArgs e)
        //{
        //    isOn = !isOn;
        //    button17.Tag = isOn;
        //}

        //private void button18_Click(object sender, EventArgs e)
        //{
        //    isOn = !isOn;
        //    button18.Tag = isOn;
        //}

        //private void button19_Click(object sender, EventArgs e)
        //{
        //    isOn = !isOn;
        //    button19.Tag = isOn;
        //}

        //private void button31_Click(object sender, EventArgs e)
        //{
        //    isOn = !isOn;
        //    button31.Tag = isOn;
        //}

        //private void button32_Click(object sender, EventArgs e)
        //{
        //    isOn = !isOn;
        //    button32.Tag = isOn;
        //}

        //private void button33_Click(object sender, EventArgs e)
        //{
        //    isOn = !isOn;
        //    button33.Tag = isOn;
        //}

        //private void button34_Click(object sender, EventArgs e)
        //{
        //    isOn = !isOn;
        //    button34.Tag = isOn;
        //}

        //private void button35_Click(object sender, EventArgs e)
        //{
        //    isOn = !isOn;
        //    button35.Tag = isOn;
        //}

        //private void button36_Click(object sender, EventArgs e)
        //{
        //    isOn = !isOn;
        //    button36.Tag = isOn;
        //}

        //private void button37_Click(object sender, EventArgs e)
        //{
        //    isOn = !isOn;
        //    button37.Tag = isOn;
        //}

        //private void button38_Click(object sender, EventArgs e)
        //{
        //    isOn = !isOn;
        //    button38.Tag = isOn;
        //}

        //private void button39_Click(object sender, EventArgs e)
        //{
        //    isOn = !isOn;
        //    button39.Tag = isOn;
        //}

        //private void button40_Click(object sender, EventArgs e)
        //{
        //    isOn = !isOn;
        //    button40.Tag = isOn;
        //}

        //private void button21_Click(object sender, EventArgs e)
        //{
        //    isOn = !isOn;
        //    button21.Tag = isOn;
        //}

        //private void button22_Click(object sender, EventArgs e)
        //{
        //    isOn = !isOn;
        //    button22.Tag = isOn;
        //}

        //private void button23_Click(object sender, EventArgs e)
        //{
        //    isOn = !isOn;
        //    button23.Tag = isOn;
        //}

        //private void button24_Click(object sender, EventArgs e)
        //{
        //    isOn = !isOn;
        //    button24.Tag = isOn;
        //}

        //private void button25_Click(object sender, EventArgs e)
        //{
        //    isOn = !isOn;
        //    button25.Tag = isOn;
        //}

        //private void button26_Click(object sender, EventArgs e)
        //{
        //    isOn = !isOn;
        //    button26.Tag = isOn;
        //}

        //private void button27_Click(object sender, EventArgs e)
        //{
        //    isOn = !isOn;
        //    button27.Tag = isOn;
        //}

        //private void button28_Click(object sender, EventArgs e)
        //{
        //    isOn = !isOn;
        //    button28.Tag = isOn;
        //}

        //private void button29_Click(object sender, EventArgs e)
        //{
        //    isOn = !isOn;
        //    button29.Tag = isOn;
        //}

        //private void button30_Click(object sender, EventArgs e)
        //{
        //    isOn = !isOn;
        //    button30.Tag = isOn;
        //}

        //private void trackBar1_Scroll(object sender, EventArgs e)
        //{

        //}

        //private void trackBar2_Scroll(object sender, EventArgs e)
        //{

        //}

        //private void trackBar4_Scroll(object sender, EventArgs e)
        //{

        //}

        //private void trackBar3_Scroll(object sender, EventArgs e)
        //{

        //}

        //private void trackBar8_Scroll(object sender, EventArgs e)
        //{

        //}

        //private void trackBar7_Scroll(object sender, EventArgs e)
        //{

        //}

        //private void trackBar6_Scroll(object sender, EventArgs e)
        //{

        //}

        //private void trackBar5_Scroll(object sender, EventArgs e)
        //{

        //}

        //private void trackBar10_Scroll(object sender, EventArgs e)
        //{

        //}

        //private void trackBar9_Scroll(object sender, EventArgs e)
        //{

        //}

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_DataContextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Enabled = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            checkboxloggingEnabled = checkBox2.Checked;
        }
        //checkBox1.CheckedChanged += CheckBox1_CheckedChanged;
        //checkBox1.CheckedChanged += checkBox1_CheckedChanged;

    }
}
















///////////////////////////-----------------------------------------------------------------------------------------------------------------------------------------


//private async Task StartEmberPlusListenerAsync()
//{
//    using (var client = await ConnectAsync("localhost", 900))
//    using (var consumer = await Consumer<MyRoot>.CreateAsync(client))
//    {
//        INode root = consumer.Root;

//        // Navigate to the parameters we're interested in.
//        var sapphire = (INode)root.Children.First(c => c.Identifier == "Sapphire");
//        var sources = (INode)sapphire.Children.First(c => c.Identifier == "Sources");
//        var fpgm1 = (INode)sources.Children.First(c => c.Identifier == "FPGM 1");
//        var fader = (INode)fpgm1.Children.First(c => c.Identifier == "Fader");
//        // var level = (IParameter)fader.Children.First(c => c.Identifier == "dB Value");
//        var position = (IParameter)fader.Children.First(c => c.Identifier == "Position");

//        // Set parameters to the desired values.
//        position.Value = 999L;
//        await consumer.SendAsync();
//    }

//}


//public static INode GetNodeByPath(INode root, string path)
//{
//    return path.Split('/')
//               .Aggregate(root, (current, identifier) =>
//                   (INode)current.Children.First(c => c.Identifier == identifier));
//}

////int i = 2;
////var fader = (TrackBar)Controls[$"trackbar{i}"];
////// var ValFaderParam = (IParameter)root.GetElement($"/auron/modules/module_{i}/path/fader");

////// var path = (INode)root.Children.First(c => c.Identifier == ("auron.modules.module_1.path."));


////var path = (INode)GetNodeByPath(root, "auron/modules/module_1/path");


//////var identifiers = new[] { "auron", "modules", $"module_{i}", "path" };
//////var path = identifiers.Aggregate((INode)root, (node, id) => (INode)node.Children.First(c => c.Identifier == id));


//////var auron = (INode)root.Children.First(c => c.Identifier == "auron");
//////var modules = (INode)auron.Children.First(c => c.Identifier == "modules");
//////var module_1 = (INode)modules.Children.First(c => c.Identifier == "module_1");
//////var path = (INode)module_1.Children.First(c => c.Identifier == "path");

////var ValFaderParam = (IParameter)path.Children.First(c => c.Identifier == "fader");


////BindFader(ValFaderParam, fader);

////var button = (Button)Controls[$"button{i}"];
////var stateParam = (IParameter)root.GetElement($"/auron/modules/module_{i}/control/sw_1/state");
////var colorParam = (IParameter)root.GetElement($"/auron/modules/module_{i}/control/sw_1/color_on");
///


//private void BindFader(IParameter faderp, System.Windows.Forms.TrackBar trackBar)
//{
//    faderp.PropertyChanged += (sender, args) =>
//    {
//        var updatedValue = (long)((IParameter)sender).Value;    // Receive valuef
//        this.BeginInvoke((Action)(() =>
//        { 
//            var parameter = sender as IParameter;
//            if (parameter != null)
//            {
//                textBox1.AppendText($"[IN] {trackBar.Name} changed: {updatedValue}\r\n");
//                int clampedValue = Math.Min(Math.Max((int)updatedValue, trackBar.Minimum), trackBar.Maximum);
//                if (trackBar.Value != clampedValue)
//                    trackBar.Value = clampedValue;
//            }
//        }));
//    };
//    trackBar.ValueChanged += (s, e) =>
//    {
//        int newValue = trackBar.Value;         // Send valuef
//        faderp.Value = (long)newValue;
//        textBox1.AppendText($"[OUT] Sent {trackBar.Name}: {newValue}\r\n");
//    };
//}

//-----------------------------------------------------------------------------------------------------------------------------------------
//private async Task StartEmberPlusListenerAsync()
//{
//    AsyncPump.Run(
//        async () =>
//    {
//        using (var client = await ConnectAsync("localhost", 9000))
//        using (var consumer = await Consumer<AuronRoot>.CreateAsync(client))
//        {
//            var fader = consumer.Root.Auron.Modules.Module_1.Path.Fader;
//            fader.faderpos.Value = 0L;
//            await consumer.SendAsync();
//        }
//    });
//}



//private async Task StartEmberPlusListenerAsync()
//{
//    using (var client = await ConnectAsync("192.168.1.2", 900))
//    using (var consumer = await Consumer<SapphireRoot>.CreateAsync(client))
//    {
//        var faderPosition = consumer.Root.Sapphire.Sources.Fpgm1.Fader.Position;

//        long newValue = 1000L;

//        faderPosition.Value = newValue;
//        await consumer.SendAsync();
//    }
//}





//-------------------------------------------------------------------------------------------------------------------------------------

//////private sealed class MyRoot : DynamicRoot<MyRoot>
//////{
//////}


//////public Consumer<GetSet.AuronRoot> consumer;
//////private List<IParameter> faderParams;
//////private INode root;

//////private async Task StartEmberPlusListenerAsync()
//////{
//////    using var client = await ConnectAsync("localhost", 9000);
//////    consumer = await Consumer<GetSet.AuronRoot>.CreateAsync(client);
//////    root = consumer.Root;

//////    faderParams = new List<IParameter>
//////    {
//////        consumer.Root.auron.modules.module_1.path.fader,
//////        consumer.Root.auron.modules.module_2.path.fader,
//////        consumer.Root.auron.modules.module_3.path.fader,
//////        consumer.Root.auron.modules.module_4.path.fader,
//////        consumer.Root.auron.modules.module_5.path.fader,
//////        consumer.Root.auron.modules.module_6.path.fader,
//////        consumer.Root.auron.modules.module_7.path.fader,
//////        consumer.Root.auron.modules.module_8.path.fader,
//////        consumer.Root.auron.modules.module_9.path.fader,
//////        consumer.Root.auron.modules.module_10.path.fader
//////    };


//////    var trackBars = new List<TrackBar>
//////    {
//////        trackBar1, trackBar2, trackBar3, trackBar4, trackBar5,
//////        trackBar6, trackBar7, trackBar8, trackBar9, trackBar10
//////    };

//////    var faderManager = new FaderManager(this, faderParams, trackBars);
//////    faderManager.InitializeFaders();





//////    List<IParameter> buttonParams = new List<IParameter>
//////    {
//////        consumer.Root.auron.modules.module_1.control.sw_1.state,
//////        consumer.Root.auron.modules.module_2.control.sw_1.state,
//////        consumer.Root.auron.modules.module_3.control.sw_1.state,
//////        consumer.Root.auron.modules.module_4.control.sw_1.state,
//////        consumer.Root.auron.modules.module_5.control.sw_1.state,
//////        consumer.Root.auron.modules.module_6.control.sw_1.state,
//////        consumer.Root.auron.modules.module_7.control.sw_1.state,
//////        consumer.Root.auron.modules.module_8.control.sw_1.state,
//////        consumer.Root.auron.modules.module_9.control.sw_1.state,
//////        consumer.Root.auron.modules.module_10.control.sw_1.state,
//////        consumer.Root.auron.modules.module_1.control.sw_2.state,
//////        consumer.Root.auron.modules.module_2.control.sw_2.state,
//////        consumer.Root.auron.modules.module_3.control.sw_2.state,
//////        consumer.Root.auron.modules.module_4.control.sw_2.state,
//////        consumer.Root.auron.modules.module_5.control.sw_2.state,
//////        consumer.Root.auron.modules.module_6.control.sw_2.state,
//////        consumer.Root.auron.modules.module_7.control.sw_2.state,
//////        consumer.Root.auron.modules.module_8.control.sw_2.state,
//////        consumer.Root.auron.modules.module_9.control.sw_2.state,
//////        consumer.Root.auron.modules.module_10.control.sw_2.state,
//////        consumer.Root.auron.modules.module_1.control.sw_3.state,
//////        consumer.Root.auron.modules.module_2.control.sw_3.state,
//////        consumer.Root.auron.modules.module_3.control.sw_3.state,
//////        consumer.Root.auron.modules.module_4.control.sw_3.state,
//////        consumer.Root.auron.modules.module_5.control.sw_3.state,
//////        consumer.Root.auron.modules.module_6.control.sw_3.state,
//////        consumer.Root.auron.modules.module_7.control.sw_3.state,
//////        consumer.Root.auron.modules.module_8.control.sw_3.state,
//////        consumer.Root.auron.modules.module_9.control.sw_3.state,
//////        consumer.Root.auron.modules.module_10.control.sw_3.state,
//////        consumer.Root.auron.modules.module_1.control.sw_4.state,
//////        consumer.Root.auron.modules.module_2.control.sw_4.state,
//////        consumer.Root.auron.modules.module_3.control.sw_4.state,
//////        consumer.Root.auron.modules.module_4.control.sw_4.state,
//////        consumer.Root.auron.modules.module_5.control.sw_4.state,
//////        consumer.Root.auron.modules.module_6.control.sw_4.state,
//////        consumer.Root.auron.modules.module_7.control.sw_4.state,
//////        consumer.Root.auron.modules.module_8.control.sw_4.state,
//////        consumer.Root.auron.modules.module_9.control.sw_4.state,
//////        consumer.Root.auron.modules.module_10.control.sw_4.state,   // 40

//////        consumer.Root.auron.modules.module_1.control.sw_1.mode,
//////        consumer.Root.auron.modules.module_2.control.sw_1.mode,
//////        consumer.Root.auron.modules.module_3.control.sw_1.mode,
//////        consumer.Root.auron.modules.module_4.control.sw_1.mode,
//////        consumer.Root.auron.modules.module_5.control.sw_1.mode,
//////        consumer.Root.auron.modules.module_6.control.sw_1.mode,
//////        consumer.Root.auron.modules.module_7.control.sw_1.mode,
//////        consumer.Root.auron.modules.module_8.control.sw_1.mode,
//////        consumer.Root.auron.modules.module_9.control.sw_1.mode,
//////        consumer.Root.auron.modules.module_10.control.sw_1.mode,
//////        consumer.Root.auron.modules.module_1.control.sw_2.mode,
//////        consumer.Root.auron.modules.module_2.control.sw_2.mode,
//////        consumer.Root.auron.modules.module_3.control.sw_2.mode,
//////        consumer.Root.auron.modules.module_4.control.sw_2.mode,
//////        consumer.Root.auron.modules.module_5.control.sw_2.mode,
//////        consumer.Root.auron.modules.module_6.control.sw_2.mode,
//////        consumer.Root.auron.modules.module_7.control.sw_2.mode,
//////        consumer.Root.auron.modules.module_8.control.sw_2.mode,
//////        consumer.Root.auron.modules.module_9.control.sw_2.mode,
//////        consumer.Root.auron.modules.module_10.control.sw_2.mode,
//////        consumer.Root.auron.modules.module_1.control.sw_3.mode,
//////        consumer.Root.auron.modules.module_2.control.sw_3.mode,
//////        consumer.Root.auron.modules.module_3.control.sw_3.mode,
//////        consumer.Root.auron.modules.module_4.control.sw_3.mode,
//////        consumer.Root.auron.modules.module_5.control.sw_3.mode,
//////        consumer.Root.auron.modules.module_6.control.sw_3.mode,
//////        consumer.Root.auron.modules.module_7.control.sw_3.mode,
//////        consumer.Root.auron.modules.module_8.control.sw_3.mode,
//////        consumer.Root.auron.modules.module_9.control.sw_3.mode,
//////        consumer.Root.auron.modules.module_10.control.sw_3.mode,
//////        consumer.Root.auron.modules.module_1.control.sw_4.mode,
//////        consumer.Root.auron.modules.module_2.control.sw_4.mode,
//////        consumer.Root.auron.modules.module_3.control.sw_4.mode,
//////        consumer.Root.auron.modules.module_4.control.sw_4.mode,
//////        consumer.Root.auron.modules.module_5.control.sw_4.mode,
//////        consumer.Root.auron.modules.module_6.control.sw_4.mode,
//////        consumer.Root.auron.modules.module_7.control.sw_4.mode,
//////        consumer.Root.auron.modules.module_8.control.sw_4.mode,
//////        consumer.Root.auron.modules.module_9.control.sw_4.mode,
//////        consumer.Root.auron.modules.module_10.control.sw_4.mode,    // 80

//////        consumer.Root.auron.modules.module_1.control.sw_1.color_on,
//////        consumer.Root.auron.modules.module_2.control.sw_1.color_on,
//////        consumer.Root.auron.modules.module_3.control.sw_1.color_on,
//////        consumer.Root.auron.modules.module_4.control.sw_1.color_on,
//////        consumer.Root.auron.modules.module_5.control.sw_1.color_on,
//////        consumer.Root.auron.modules.module_6.control.sw_1.color_on,
//////        consumer.Root.auron.modules.module_7.control.sw_1.color_on,
//////        consumer.Root.auron.modules.module_8.control.sw_1.color_on,
//////        consumer.Root.auron.modules.module_9.control.sw_1.color_on,
//////       consumer.Root.auron.modules.module_10.control.sw_1.color_on,
//////        consumer.Root.auron.modules.module_1.control.sw_2.color_on,
//////        consumer.Root.auron.modules.module_2.control.sw_2.color_on,
//////        consumer.Root.auron.modules.module_3.control.sw_2.color_on,
//////        consumer.Root.auron.modules.module_4.control.sw_2.color_on,
//////        consumer.Root.auron.modules.module_5.control.sw_2.color_on,
//////        consumer.Root.auron.modules.module_6.control.sw_2.color_on,
//////        consumer.Root.auron.modules.module_7.control.sw_2.color_on,
//////        consumer.Root.auron.modules.module_8.control.sw_2.color_on,
//////        consumer.Root.auron.modules.module_9.control.sw_2.color_on,
//////       consumer.Root.auron.modules.module_10.control.sw_2.color_on,
//////        consumer.Root.auron.modules.module_1.control.sw_3.color_on,
//////        consumer.Root.auron.modules.module_2.control.sw_3.color_on,
//////        consumer.Root.auron.modules.module_3.control.sw_3.color_on,
//////        consumer.Root.auron.modules.module_4.control.sw_3.color_on,
//////        consumer.Root.auron.modules.module_5.control.sw_3.color_on,
//////        consumer.Root.auron.modules.module_6.control.sw_3.color_on,
//////        consumer.Root.auron.modules.module_7.control.sw_3.color_on,
//////        consumer.Root.auron.modules.module_8.control.sw_3.color_on,
//////        consumer.Root.auron.modules.module_9.control.sw_3.color_on,
//////       consumer.Root.auron.modules.module_10.control.sw_3.color_on,
//////        consumer.Root.auron.modules.module_1.control.sw_4.color_on,
//////        consumer.Root.auron.modules.module_2.control.sw_4.color_on,
//////        consumer.Root.auron.modules.module_3.control.sw_4.color_on,
//////        consumer.Root.auron.modules.module_4.control.sw_4.color_on,
//////        consumer.Root.auron.modules.module_5.control.sw_4.color_on,
//////        consumer.Root.auron.modules.module_6.control.sw_4.color_on,
//////        consumer.Root.auron.modules.module_7.control.sw_4.color_on,
//////        consumer.Root.auron.modules.module_8.control.sw_4.color_on,
//////        consumer.Root.auron.modules.module_9.control.sw_4.color_on,
//////       consumer.Root.auron.modules.module_10.control.sw_4.color_on,  //  120

//////        consumer.Root.auron.modules.module_1.control.sw_1.color_off,
//////        consumer.Root.auron.modules.module_2.control.sw_1.color_off,
//////        consumer.Root.auron.modules.module_3.control.sw_1.color_off,
//////        consumer.Root.auron.modules.module_4.control.sw_1.color_off,
//////        consumer.Root.auron.modules.module_5.control.sw_1.color_off,
//////        consumer.Root.auron.modules.module_6.control.sw_1.color_off,
//////        consumer.Root.auron.modules.module_7.control.sw_1.color_off,
//////        consumer.Root.auron.modules.module_8.control.sw_1.color_off,
//////        consumer.Root.auron.modules.module_9.control.sw_1.color_off,
//////       consumer.Root.auron.modules.module_10.control.sw_1.color_off,
//////        consumer.Root.auron.modules.module_1.control.sw_2.color_off,
//////        consumer.Root.auron.modules.module_2.control.sw_2.color_off,
//////        consumer.Root.auron.modules.module_3.control.sw_2.color_off,
//////        consumer.Root.auron.modules.module_4.control.sw_2.color_off,
//////        consumer.Root.auron.modules.module_5.control.sw_2.color_off,
//////        consumer.Root.auron.modules.module_6.control.sw_2.color_off,
//////        consumer.Root.auron.modules.module_7.control.sw_2.color_off,
//////        consumer.Root.auron.modules.module_8.control.sw_2.color_off,
//////        consumer.Root.auron.modules.module_9.control.sw_2.color_off,
//////       consumer.Root.auron.modules.module_10.control.sw_2.color_off,
//////        consumer.Root.auron.modules.module_1.control.sw_3.color_off,
//////        consumer.Root.auron.modules.module_2.control.sw_3.color_off,
//////        consumer.Root.auron.modules.module_3.control.sw_3.color_off,
//////        consumer.Root.auron.modules.module_4.control.sw_3.color_off,
//////        consumer.Root.auron.modules.module_5.control.sw_3.color_off,
//////        consumer.Root.auron.modules.module_6.control.sw_3.color_off,
//////        consumer.Root.auron.modules.module_7.control.sw_3.color_off,
//////        consumer.Root.auron.modules.module_8.control.sw_3.color_off,
//////        consumer.Root.auron.modules.module_9.control.sw_3.color_off,
//////       consumer.Root.auron.modules.module_10.control.sw_3.color_off,
//////        consumer.Root.auron.modules.module_1.control.sw_4.color_off,
//////        consumer.Root.auron.modules.module_2.control.sw_4.color_off,
//////        consumer.Root.auron.modules.module_3.control.sw_4.color_off,
//////        consumer.Root.auron.modules.module_4.control.sw_4.color_off,
//////        consumer.Root.auron.modules.module_5.control.sw_4.color_off,
//////        consumer.Root.auron.modules.module_6.control.sw_4.color_off,
//////        consumer.Root.auron.modules.module_7.control.sw_4.color_off,
//////        consumer.Root.auron.modules.module_8.control.sw_4.color_off,
//////        consumer.Root.auron.modules.module_9.control.sw_4.color_off,
//////       consumer.Root.auron.modules.module_10.control.sw_4.color_off   // 160
//////    };


//////    var buttons = new List<Button>
//////    {
//////        button1, button2, button3, button4, button5,
//////        button6, button7, button8, button9, button10,
//////        button11, button12, button13, button14, button15,
//////        button16, button17, button18, button19, button20,
//////        button21, button22, button23, button24, button25,
//////        button26, button27, button28, button29, button30,
//////        button31, button32, button33, button34, button35,
//////        button36, button37, button38, button39, button40
//////    };

//////    var buttonManager = new ButtonManager(this, buttonParams, buttons);
//////    buttonManager.InitializeButtons();

//////    await Task.Delay(Timeout.Infinite);
//////    //await consumer.SendAsync();
//////}



//private void RegisterFaderEvents()
//{
//    for (int i = 0; i < this.Controls.Count; i++)
//    {
//        //if (this.Controls[i] is TrackBar trackb && trackb.Name.StartsWith("trackbar"))
//           // trackb.Scroll += MoveFaders;
//    }
//}



//// Update the MoveFaders method to match the EventHandler delegate signature
//private void MoveFaders(object sender, EventArgs e)
//{
//    if (sender is TrackBar trackBar)
//    {
//        // Add your logic for handling the TrackBar scroll event here
//        // Example: Update the fader value or perform other actions
//    }
//}
