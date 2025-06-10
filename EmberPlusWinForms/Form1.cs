using Lawo.EmberPlusSharp.Model;
using Lawo.EmberPlusSharp.S101;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Button = System.Windows.Forms.Button;
using TrackBar = System.Windows.Forms.TrackBar;

// kleine test3
namespace EmberPlusWinForms
{
    public partial class Form1 : Form
    {
        public bool checkboxloggingEnabled;
        private FaderManager faderManager;
        private ButtonManager buttonManager;
        public static Form1? instanse;
        string ipaddress;

        public Form1()   // constructor
        {
            InitializeComponent();
            instanse = this;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            //RegisterButtonEvents();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!(File.Exists("ip.txt")))
            {
                File.WriteAllText("ip.txt", "127.0.0.1");
            }
            bool isEmpty = new FileInfo("ip.txt").Length == 0;
            if (isEmpty)
            {
                Console.WriteLine("The file is empty.");
                ipaddress = "127.0.0.1";
            }
            string content = File.ReadAllText("ip.txt");
            textBox2.Text = content;
            ipaddress = textBox2.Text;
            timer1.Stop();
            Task startEmberPlusListenerAsync = StartEmberPlusListenerAsync();
            faderManager = new FaderManager(faderParams, trackBars);
            buttonManager = new ButtonManager(faderParams, buttons);

        }


        private static async Task<S101Client> ConnectAsync(string host, int port)
        {
            var tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(host, port); // Ensure this is awaited
            NetworkStream stream = tcpClient.GetStream();
            await Task.Delay(300); // Optional delay if needed
            var clients101 = new S101Client(tcpClient, stream.ReadAsync, stream.WriteAsync);
            return clients101;
            //return new S101Client(tcpClient, stream.ReadAsync, stream.WriteAsync);
        }

        //private sealed class MyRoot : DynamicRoot<MyRoot>
        //{
        //}

        //-------------- Start -----------------------------------------------------------------------------------------------------------------------

        public Consumer<GetSet.AuronRoot> consumer;
        private List<IParameter> faderParams;
        private INode root;

        private CancellationTokenSource? cancellationTokenSource;

        private async Task StartEmberPlusListenerAsync()
        {
            cancellationTokenSource = new CancellationTokenSource();
            try
            {
                using var client = await ConnectAsync(ipaddress, 9000);
                consumer = await Consumer<GetSet.AuronRoot>.CreateAsync(client);
                root = consumer.Root;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to connect: {ex.Message}", "Greeting", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return; // Exit early if the connection fails
            }
            root = consumer.Root;

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

            int aantalMoules = modules.Length;

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
            for (int i = 1; i <= faderParams.Count; i++)
            {
                var trackBar = (TrackBar)Controls["trackBar" + i];
                trackBars.Add(trackBar);
            }

            var faderManager = new FaderManager(faderParams, trackBars);
            faderManager.InitializeFaders();

            List<IParameter> buttonParams = new List<IParameter>();

            for (int i = 0; i < aantalMoules; i++)
            {
                buttonParams.Add(modules[i].control.sw_1.state);
                buttonParams.Add(modules[i].control.sw_2.state);
                buttonParams.Add(modules[i].control.sw_3.state);
                buttonParams.Add(modules[i].control.sw_4.state);
            }

            for (int i = 0; i < aantalMoules; i++)
            {
                buttonParams.Add(modules[i].control.sw_1.mode);
                buttonParams.Add(modules[i].control.sw_2.mode);
                buttonParams.Add(modules[i].control.sw_3.mode);
                buttonParams.Add(modules[i].control.sw_4.mode);
            }

            for (int i = 0; i < aantalMoules; i++)
            {
                buttonParams.Add(modules[i].control.sw_1.color_on);
                buttonParams.Add(modules[i].control.sw_2.color_on);
                buttonParams.Add(modules[i].control.sw_3.color_on);
                buttonParams.Add(modules[i].control.sw_4.color_on);
            }

            for (int i = 0; i < aantalMoules; i++)
            {
                buttonParams.Add(modules[i].control.sw_1.color_off);
                buttonParams.Add(modules[i].control.sw_2.color_off);
                buttonParams.Add(modules[i].control.sw_3.color_off);
                buttonParams.Add(modules[i].control.sw_4.color_off);
            }

            var buttons = new List<Button>();
            for (int i = 1; i <= (aantalMoules * 4); i++)
            {
                var button = (Button)Controls["button" + i];
                buttons.Add(button);
            }

            var buttonManager = new ButtonManager(buttonParams, buttons);
            buttonManager.InitializeButtonsAsync();

            await Task.Delay(Timeout.Infinite, cancellationTokenSource.Token);
        }


        //-------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///  Alle events van buttons, faders, timers en checkboxes zijn hier in de code onder gezet.
        /// </summary>
        //-------------------------------------------------------------------------------------------------------------------------------------


        static int tellertje;
        private List<TrackBar> trackBars;
        private List<Button> buttons;



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
                    FaderManager.SyncTrackBarToEmberAsync(trackBars[i], faderParams[i]);
                }
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {

        }

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

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void ipbutton_Click(object sender, EventArgs e)
        {
            startIPverbinding();
        }

        private async void startIPverbinding()
        {
            ipaddress = textBox2.Text;
            File.WriteAllText("ip.txt", ipaddress);
            await StartEmberPlusListenerAsync(); // run without blocking UI
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;      // Prevents further processing
                e.SuppressKeyPress = true; // Prevents the 'ding' sound or newline

                startIPverbinding();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ipaddress = textBox2.Text;
            File.WriteAllText("ip.txt", ipaddress);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }


        // -----------Het zonder meer laten knipperen van de buttons--------------------------------------------

        private void timer2_Tick(object sender, EventArgs e)
        {
            int aantalbuttons = 40;

            Button[] buttons = new Button[40];

            for (int i = 0; i < aantalbuttons; i++)
            {
                buttons[i] = (Button)Controls["button" + (i + 1)]; // Finds button by name
            }


            for (int i = 0; i < aantalbuttons; i++)
            {
                ButtonSimulator.SimulateButtonPress(buttons[i]);
            }
        }


        public class ButtonSimulator
        {
            [DllImport("user32.dll")]
            private static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

            private const int WM_LBUTTONDOWN = 0x201;
            private const int WM_LBUTTONUP = 0x202;

            public static void SimulateButtonPress(Button button)
            {
                if (button != null)
                {
                    IntPtr handle = button.Handle;
                    SendMessage(handle, WM_LBUTTONDOWN, IntPtr.Zero, IntPtr.Zero);
                    SendMessage(handle, WM_LBUTTONUP, IntPtr.Zero, IntPtr.Zero);
                }
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            timer2.Enabled = checkBox3.Checked;
        }

//---------------------------------------------------------------------------------------

    }
}