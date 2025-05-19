using Lawo.ComponentModel;
using Lawo.EmberPlusSharp.Ember;
using Lawo.EmberPlusSharp.Model;
using Lawo.EmberPlusSharp.S101;
using System;
using System.Drawing;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;


namespace EmberPlusWinForms
{
    public partial class Form1 : Form
    {
        public static Form1 Instance;  // Now you can access the form from static code
        bool checkboxEnabled = false;
        int buttonNR = 1;

        public Form1()
        {
            InitializeComponent();
            Instance = this;
            // Subscribe to the CheckedChanged event
            checkBox1.CheckedChanged += CheckBox1_CheckedChanged;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Stop();
            Task startEmberPlusListenerAsync = StartEmberPlusListenerAsync();
        }

        private static async Task<S101Client> ConnectAsync(string host, int port)
        {
            var tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(host, port);
            var stream = tcpClient.GetStream();
            Thread.Sleep(300);

            return new S101Client(tcpClient, stream.ReadAsync, stream.WriteAsync);
        }

        private sealed class MyRoot : DynamicRoot<MyRoot>
        {
        }
        //-------------------------------------------------------------------------------------------------------------------------------------
        private void BindFader(IParameter fader, TrackBar trackBar)
        {
            fader.PropertyChanged += (sender, args) =>
            {
                var updatedValue = (long)((IParameter)sender).Value;    // Receive value
                this.BeginInvoke((Action)(() =>
                {
                    var parameter = sender as IParameter;
                    if (parameter != null)
                    {
                        textBox1.AppendText($"[IN] {trackBar.Name} changed: {updatedValue}\r\n");
                        int clampedValue = Math.Min(Math.Max((int)updatedValue, trackBar.Minimum), trackBar.Maximum);
                        if (trackBar.Value != clampedValue)
                            trackBar.Value = clampedValue;
                    }
                }));
            };
            trackBar.ValueChanged += (s, e) =>
            {
                int newValue = trackBar.Value;         // Send value
                fader.Value = (long)newValue;
                textBox1.AppendText($"[OUT] Sent {trackBar.Name}: {newValue}\r\n");
            };
        }

//---------------------------------------------------------------------------------------------------------------------------

        private void BindButton(Button button, IParameter state, IParameter color_on)
        {
            state.PropertyChanged += (sender, args) =>           // Receive value
            {
                this.BeginInvoke((Action)(() =>
                {
                    button.Tag = sender as IParameter;
                    textBox1.AppendText($"[IN] {button.Name} changed: {button.Tag}\r\n");
                }));
            };

            color_on.PropertyChanged += (sender, args) =>
            {
                var updatedValue = (long)((IParameter)sender).Value;    // Receive value

                this.BeginInvoke((Action)(() =>
                {
                    //textBox1.AppendText($"[IN] {buttonc.Name} changed: {sender as IParameter}\r\n");
                    switch (updatedValue)
                    {
                        case 1: button.BackColor = Color.Red; break;
                        case 0: button.BackColor = Color.Transparent; break;
                        case 2: button.BackColor = Color.Green; break;
                        case 3: button.BackColor = Color.Black; break;
                        default: button.ForeColor = Color.Blue; break;
                    }
                }));
            };

            button.Click += (s, e) =>
            {
                if ((bool)button.Tag)
                {
                    state.Value = true;                     // Send value
                    color_on.Value = Convert.ToInt64(1);
                }
                else
                {
                    state.Value = false;
                    color_on.Value = Convert.ToInt64(0);
                }
                textBox1.AppendText($"[OUT] {button.Name} changed: {(bool)button.Tag}\r\n");
            };
        }




        public static INode GetNodeByPath(INode root, string path)
        {
            return path.Split('/')
                       .Aggregate(root, (current, identifier) =>
                           (INode)current.Children.First(c => c.Identifier == identifier));
        }



        //-----------------------------------------------------------------------------------------------------------------------------------------
        public async Task StartEmberPlusListenerAsync()
        {
            using (S101Client client = await ConnectAsync("192.168.1.2", 9000))
            using (Consumer<MyRoot> consumer = await Consumer<MyRoot>.CreateAsync(client))
            {
                INode root = consumer.Root;

                //for (int i = 1; i <= 10; i++) // 10 Modules
                {
                    int i = 2;
                    var fader = (TrackBar)Controls[$"trackbar{i}"];
                    // var ValFaderParam = (IParameter)root.GetElement($"/auron/modules/module_{i}/path/fader");

                    // var path = (INode)root.Children.First(c => c.Identifier == ("auron.modules.module_1.path."));


                    var path = (INode)GetNodeByPath(root, "auron/modules/module_1/path");



                    //var identifiers = new[] { "auron", "modules", $"module_{i}", "path" };
                    //var path = identifiers.Aggregate((INode)root, (node, id) => (INode)node.Children.First(c => c.Identifier == id));


                    //var auron = (INode)root.Children.First(c => c.Identifier == "auron");
                    //var modules = (INode)auron.Children.First(c => c.Identifier == "modules");
                    //var module_1 = (INode)modules.Children.First(c => c.Identifier == "module_1");
                    //var path = (INode)module_1.Children.First(c => c.Identifier == "path");

                    var ValFaderParam = (IParameter)path.Children.First(c => c.Identifier == "fader");


                    BindFader(ValFaderParam, fader);

                    var button = (Button)Controls[$"button{i}"];
                    var stateParam = (IParameter)root.GetElement($"/auron/modules/module_{i}/control/sw_1/state");
                    var colorParam = (IParameter)root.GetElement($"/auron/modules/module_{i}/control/sw_1/color_on");

                    BindButton(button, stateParam, colorParam);
                }

                await Task.Delay(Timeout.Infinite); // Keep app alive
            }
        }




        //-----------------------------------------------------------------------------------------------------------------------------------------

        private bool isOn = true;

        private  void button1_Click(object sender, EventArgs e)
        {
            isOn = !isOn; // Toggle the boolean value
            button1.Tag = isOn; // Store the value in the Tag property
            buttonNR = 1;
        }

        private  void button2_Click(object sender, EventArgs e)
        {
            isOn = !isOn; // Toggle the boolean value
            button2.Tag = isOn; // Store the value in the Tag property
            buttonNR = 2;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {

        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {

        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {

        }

        private void trackBar8_Scroll(object sender, EventArgs e)
        {

        }

        private void trackBar7_Scroll(object sender, EventArgs e)
        {

        }

        private void trackBar6_Scroll(object sender, EventArgs e)
        {

        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {

        }

        private void trackBar10_Scroll(object sender, EventArgs e)
        {

        }

        private void trackBar9_Scroll(object sender, EventArgs e)
        {

        }

        private void Form1_Activated(object sender, EventArgs e)
        {

        }

        static int tellertje;

        private void timer1_Tick(object sender, EventArgs e)
        {
            tellertje = tellertje + 20;
            trackBar1.Value = tellertje;
            trackBar2.Value = tellertje;
            trackBar3.Value = tellertje;
            trackBar4.Value = tellertje;
            trackBar5.Value = tellertje;
            trackBar6.Value = tellertje;
            trackBar7.Value = tellertje;
            trackBar8.Value = tellertje;
            trackBar9.Value = tellertje;
            trackBar10.Value = tellertje;
            if (tellertje > 1000)
                tellertje = 0;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {

        }
        private void CheckBox1_CheckedChanged(object? sender, EventArgs e)
        {

        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Enabled = checkBox1.Checked;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            isOn = !isOn; 
            button3.Tag = isOn;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button4.Tag = isOn;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button5.Tag = isOn;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button6.Tag = isOn;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button7.Tag = isOn;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button8.Tag = isOn;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button9.Tag = isOn;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button10.Tag = isOn;
        }
    }
}




