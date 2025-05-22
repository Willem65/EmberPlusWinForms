using Lawo.ComponentModel;
using Lawo.EmberPlusSharp.Ember;
using Lawo.EmberPlusSharp.Model;
using Lawo.EmberPlusSharp.S101;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using static EmberPlusWinForms.FaderManager;
using TrackBar = System.Windows.Forms.TrackBar;
using Button = System.Windows.Forms.Button;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;
using System.Diagnostics;

namespace EmberPlusWinForms
{
    public partial class Form1 : Form
    {
        bool checkboxEnabled = false;
        // Change the accessibility of the 'checkboxloggingEnabled' field to 'public' to fix the CS0122 error.
        public bool checkboxloggingEnabled;

        int buttonNR = 1;
        private FaderManager faderManager;
        private ButtonManager buttonManager;
        public static Form1 instanse;



        public Form1()
        {
            InitializeComponent();
            instanse = this;
            checkBox1.CheckedChanged += CheckBox1_CheckedChanged;
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Stop();
            Task startEmberPlusListenerAsync = StartEmberPlusListenerAsync();
            ToggleAllControls(this, false);
            Thread.Sleep(2000);
            ToggleAllControls(this, true);
            // faderManager = new FaderManager(consumer);
            faderManager = new FaderManager(this, faderParams, trackBars);
            buttonManager = new ButtonManager(this, faderParams, buttons);
        }


        private void ToggleAllControls(Control parent, bool isEnabled)
        {
            foreach (Control control in parent.Controls)
            {
                control.Enabled = isEnabled;
                if (control.HasChildren) // Check if the control contains child controls
                    ToggleAllControls(control, isEnabled); // Recursively disable/enable child controls
            }

        }



        private static async Task<S101Client> ConnectAsync(string host, int port)
        {
            var tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(host, port);
            var stream = tcpClient.GetStream();
            Thread.Sleep(300);

            return new S101Client(tcpClient, stream.ReadAsync, stream.WriteAsync);
        }





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



        //-------------------------------------------------------------------------------------------------------------------------------------


        private sealed class MyRoot : DynamicRoot<MyRoot>
        {
        }


        public Consumer<GetSet.AuronRoot> consumer;
        private List<IParameter> faderParams;
        private INode root;

        private async Task StartEmberPlusListenerAsync()
        {
            using var client = await ConnectAsync("localhost", 9000);
            consumer = await Consumer<GetSet.AuronRoot>.CreateAsync(client);
            root = consumer.Root;

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


            var trackBars = new[]
            {
                trackBar1, trackBar2, trackBar3, trackBar4, trackBar5,
                trackBar6, trackBar7, trackBar8, trackBar9, trackBar10
            }.ToList();


            var faderManager = new FaderManager(this, faderParams, trackBars);
            faderManager.InitializeFaders();

            //-------------------------------------------------------------------------------------------------------------------------------------

            // First, gather your modules into an array for easier iteration.
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

            // Group 1: Add states for sw_1 through sw_4
            for (int i = 0; i < modules.Length; i++)
            {
                buttonParams.Add(modules[i].control.sw_1.state);
                buttonParams.Add(modules[i].control.sw_2.state);
                buttonParams.Add(modules[i].control.sw_3.state);
                buttonParams.Add(modules[i].control.sw_4.state);
            }

            // Group 2: Add modes for sw_1 through sw_4
            for (int i = 0; i < modules.Length; i++)
            {
                buttonParams.Add(modules[i].control.sw_1.mode);
                buttonParams.Add(modules[i].control.sw_2.mode);
                buttonParams.Add(modules[i].control.sw_3.mode);
                buttonParams.Add(modules[i].control.sw_4.mode);
            }

            // Group 3: Add color_on for sw_1 through sw_4
            for (int i = 0; i < modules.Length; i++)
            {
                buttonParams.Add(modules[i].control.sw_1.color_on);
                buttonParams.Add(modules[i].control.sw_2.color_on);
                buttonParams.Add(modules[i].control.sw_3.color_on);
                buttonParams.Add(modules[i].control.sw_4.color_on);
            }

            // Group 4: Add color_off for sw_1 through sw_4
            for (int i = 0; i < modules.Length; i++)
            {
                buttonParams.Add(modules[i].control.sw_1.color_off);
                buttonParams.Add(modules[i].control.sw_2.color_off);
                buttonParams.Add(modules[i].control.sw_3.color_off);
                buttonParams.Add(modules[i].control.sw_4.color_off);
            }




            //-------------------------------------------------------------------------------------------------------------------------------------

            var buttons = new List<Button>();

            for (int i = 1; i <= 40; i++)
            {
                var button = (Button)Controls["button" + i];
                    buttons.Add(button);
            }


            var buttonManager = new ButtonManager(this, buttonParams, buttons);
            buttonManager.InitializeButtons();

            await Task.Delay(Timeout.Infinite);

        }

//-------------------------------------------------------------------------------------------------------------------------------------













private bool isOn = true;

        private void button1_Click(object sender, EventArgs e)
        {
            isOn = !isOn; // Toggle the boolean valuef
            button1.Tag = isOn; // Store the valuef in the Tag property
        }

        private void button2_Click(object sender, EventArgs e)
        {
            isOn = !isOn; // Toggle the boolean valuef
            button2.Tag = isOn; // Store the valuef in the Tag property
        }

        static int tellertje;
        private List<TrackBar> trackBars;
        private List<Button> buttons;
        private object lstdata;





        // Moving all the 10 ffaders

        private void timer1_Tick(object sender, EventArgs e)
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
        private void CheckBox1_CheckedChanged(object? sender, EventArgs e)
        {

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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_DataContextChanged(object sender, EventArgs e)
        {
            //int listdata = listBox1.Items.Add("willem");
            //FaderManager fdr = new FaderManager(listdata);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button20.Tag = isOn;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button11.Tag = isOn;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button12.Tag = isOn;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button13.Tag = isOn;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button14.Tag = isOn;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button15.Tag = isOn;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button16.Tag = isOn;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button17.Tag = isOn;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button18.Tag = isOn;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button19.Tag = isOn;
        }

        private void button31_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button31.Tag = isOn;
        }

        private void button32_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button32.Tag = isOn;
        }

        private void button33_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button33.Tag = isOn;
        }

        private void button34_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button34.Tag = isOn;
        }

        private void button35_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button35.Tag = isOn;
        }

        private void button36_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button36.Tag = isOn;
        }

        private void button37_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button37.Tag = isOn;
        }

        private void button38_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button38.Tag = isOn;
        }

        private void button39_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button39.Tag = isOn;
        }

        private void button40_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button40.Tag = isOn;
        }

        private void button21_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button21.Tag = isOn;
        }

        private void button22_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button22.Tag = isOn;
        }

        private void button23_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button23.Tag = isOn;
        }

        private void button24_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button24.Tag = isOn;
        }

        private void button25_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button25.Tag = isOn;
        }

        private void button26_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button26.Tag = isOn;
        }

        private void button27_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button27.Tag = isOn;
        }

        private void button28_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button28.Tag = isOn;
        }

        private void button29_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button29.Tag = isOn;
        }

        private void button30_Click(object sender, EventArgs e)
        {
            isOn = !isOn;
            button30.Tag = isOn;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Enabled = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            checkboxloggingEnabled = checkBox2.Checked;
        }

        //private void timer1_Tick_1(object sender, EventArgs e)
        //{

        //}
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

