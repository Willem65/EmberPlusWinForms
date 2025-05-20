using Lawo.EmberPlusSharp.Model;
using Lawo.EmberPlusSharp.S101;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Text.Json;
using System.IO;

namespace EmberPlusWinForms
{
    public partial class Form1 : Form
    {
        public static Form1 Instance;  // Now you can access the form from static code
        //private dynamic fader;

        public Form1()
        {
            InitializeComponent();
            Instance = this;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           // Task startEmberPlusListenerAsync = StartEmberPlusListenerAsync();
            //Task emberTreePlusListenerAsync = EmberTreePlusListenerAsync();



            //// Specify the path of program.cs
            //string inputFilePath = "C:\\temp26-Emberpluscsharp-WindowsForms\\EmberPlusWinForms\\EmberPlusWinForms\\bin\\Debug\\net8.0-windows\\EmberModel.cs";
            //string outputFilePath = "C:\\temp26-Emberpluscsharp-WindowsForms\\EmberPlusWinForms\\EmberPlusWinForms\\bin\\Debug\\net8.0-windows\\test.json";
            //// Step 1: Read content from program.cs
            //string fileContent = File.ReadAllText(inputFilePath);

            //// Step 2: Replace CR and LF with their JSON-escaped counterparts
            //fileContent = fileContent.Replace("\r", "").Replace("\n", "n");

            //// Step 2: Process data (in this example, we take the content as-is)
            //var data = new
            //{
            //    FileName = "program.cs",
            //    Content = fileContent
            //};
            //// Step 3: Convert data to JSON
            //string jsonString = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            //// Step 4: Write JSON to file
            //File.WriteAllText(outputFilePath, jsonString);

        }

        private static async Task<S101Client> ConnectAsync(string host, int port)
        {
            var tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(host, port);
            var stream = tcpClient.GetStream();
            Thread.Sleep(300);
            return new S101Client(tcpClient, stream.ReadAsync, stream.WriteAsync);
        }



        //-----------------------------------------------------------------------------------------------------

        private static void WriteChildren(INode node, string filePath)
        {
            using (var writer = new StreamWriter(filePath, false)) // Overwrite if file exists
            {
                //writer.WriteLine(" ");
                //writer.WriteLine(" ");
                //writer.WriteLine(" ");
                //writer.WriteLine(" ");
                //writer.WriteLine(" ");

                //writer.WriteLine("using Lawo.EmberPlusSharp.Model;");
                //writer.WriteLine(" ");
                ////writer.WriteLine("namespace EmberPlusWinForms");
                //writer.WriteLine("namespace " + Instance.GetType().Namespace);
                //writer.WriteLine("{");
                //var indent = new string(' ', 4);
                //writer.WriteLine(indent + "public static class EmberTree");
                //writer.WriteLine(indent + "{");
                //writer.WriteLine(indent + indent + $"public sealed class {node.Children[0].Identifier}Root : Root<{node.Children[0].Identifier}Root>");
                //writer.WriteLine(indent + indent + "{");
                //writer.WriteLine(indent + indent + "    internal " + node.Children[0].Identifier + " " + node.Children[0].Identifier + " {get; private set;}");
                //writer.WriteLine(indent + indent + "}");
                WriteChildrenAsProperties(node, 1, writer);
                //writer.WriteLine(indent + indent + "}");
                //writer.WriteLine(indent + "}");
                //writer.WriteLine("}");
            }
        }

        


        private static void WriteChildrenAsProperties(INode node, int depth, StreamWriter writer)
        {
            var indent = new string(' ', 8 * depth);
            int teller=0;

            for (int i = 0; i < node.Children.Count; i++)
            {

                var child = node.Children[i];

                var childNode = child as INode;
                var childParameter = child as IParameter;



                if (childNode != null)
                {
                    writer.WriteLine(indent + " }");
                    //writer.WriteLine($"{indent}     internal " + childNode.Identifier + " " + childNode.Identifier + " {get; private set;}");
                    writer.WriteLine($"{indent} public sealed class {childNode.Identifier} : FieldNode<{childNode.Identifier}>");
                    writer.WriteLine(indent + " {");
                    writer.WriteLine(" ");
                    writer.WriteLine(teller++);
                    WriteChildrenAsProperties(childNode, 1, writer); // Recursive
                    
                    
                    //WriteChildrenAsProperties(childNode.Description, 1, writer); // Recursive

                    // writer.WriteLine($"{indent}     Internal " + childNode.Identifier + " " + childNode.Identifier  + " { get; private set; }");
                    //writer.WriteLine($"{indent} public sealed class {childNode.Identifier} : FieldNode<{childNode.Identifier}>");
                    // writer.WriteLine(indent + " }");
                    //  writer.WriteLine(" ");



                }




                

                if (childParameter != null)
                {

                    var type = MapParameterType(childParameter); // Determine C# type
                    var safeName = ToValidCSharpIdentifier(childParameter.Identifier);

                    //writer.WriteLine($"{indent}     [Element(Identifier = \"{safeName}\")]");

                    //writer.WriteLine("{0}Parameter {1}: {2}", indent, child.Identifier, type);

                   // writer.WriteLine($"{indent}     internal " + childParameter.Type + "Parameter  " + childParameter.Identifier + " { get; private set; }  ");
                    
                   // writer.WriteLine(indent + " }");

                }


            }


            //childNode = node.Children[1] as INode;

            //if (childNode != null)
            //{
            //    writer.WriteLine(indent + "3{");
            //    writer.WriteLine($"{indent} internal {childNode.Identifier} {childNode.Identifier} {{ get; private set; }}");
            //    writer.WriteLine(indent + "}3");
            //    WriteChildrenAsProperties(childNode, depth, writer); // Recursive

            //}

        }

        private static string MapParameterType(IParameter param)
        {
            var value = param.Value;

            if (value is int) return "int";
            if (value is double) return "double";
            if (value is bool) return "bool";
            if (value is string) return "string";

            return "object"; // fallback
        }

        private static string ToValidCSharpIdentifier(string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier))
                return "Unknown";

            // Replace spaces and special characters
            var valid = Regex.Replace(identifier, @"[^a-zA-Z0-9_]", "_");
            if (char.IsDigit(valid[0]))
                valid = "_" + valid;

            return valid;
        }

        //-----------------------------------------------------------------------------------------------------



        //public sealed class SapphireRoot : Root<SapphireRoot>
        //{
        //    internal Sapphire Sapphire { get; private set; }

        //}

        //public sealed class Sapphire : FieldNode<Sapphire>
        //{
        //    internal Sources Sources { get; private set; }
        //    //internal identity identity { get; private set; }
        //}

        //public sealed class Sources : FieldNode<Sources>
        //{
        //    [Element(Identifier = "FPGM 1")]
        //    internal Source Fpgm1 { get; private set; }
        //}

        //public sealed class Source : FieldNode<Source>
        //{
        //    internal Fader Fader { get; private set; }
        //}

        //public sealed class Fader : FieldNode<Fader>
        //{
        //    [Element(Identifier = "dB Value")]
        //    internal RealParameter DBValue { get; private set; }

        //    internal IntegerParameter Position { get; private set; }
        //}
























        //public sealed class SapphireRoot : FieldNode<SapphireRoot>
        //{
        //    internal identity identity { get; private set; }
        //}

        //   public sealed class identity : FieldNode<identity>
        //   {
        //    internal IntegerParameter Position { get; private set; }
        //    internal IntegerParameter company { get; private set; }
        //   }




        private sealed class MyRoot : DynamicRoot<MyRoot>
        {
        }

        //private async Task EmberTreePlusListenerAsync()
        //{
        //    using (S101Client client = await ConnectAsync("127.0.0.1", 9000))
        //    using (Consumer<MyRoot> consumer = await Consumer<MyRoot>.CreateAsync(client))
        //    {
        //        string outputPath = "EmberModel.cs";
        //        WriteChildren(consumer.Root, outputPath);
        //        //WriteChildren(consumer.Root, 100);
        //    }
        //}

        public async Task StartEmberPlusListenerAsync()
        {
            //using (S101Client client = await ConnectAsync("192.168.1.2", 9000))
            //using (Consumer<EmberPlusWinForms.SapphireRoot> consumer = await Consumer<Models.SapphireRoot>.CreateAsync(client))

            //using (var client = await ConnectAsync("localhost", 9000))
            //using (var consumer = await Consumer<MyRoot>.CreateAsync(client))

            //using (S101Client client = await ConnectAsync("127.0.0.1", 9000))
            //using (Consumer<SapphireRoot> consumer = await Consumer<SapphireRoot>.CreateAsync(client))

            //using (S101Client client = await ConnectAsync("127.0.0.1", 9000))
            //using (Consumer<SapphireRoot> consumer = await Consumer<SapphireRoot>.CreateAsync(client))


            using (S101Client client = await ConnectAsync("127.0.0.1", 9000))
            using (Consumer<MyRoot> consumer = await Consumer<MyRoot>.CreateAsync(client))
            {

                //string outputPath = "EmberModel.cs";
                //WriteChildren(consumer.Root, outputPath);
                //WriteChildren(consumer.Root, 100);

                INode root = consumer.Root;
                //var faderPosition = consumer.Root.Sapphire.Sources.Fpgm1.Fader.Position;
                var faderPosition = (IParameter)root.GetElement("/Sapphire/Sources/FPGM 1/Fader/Position");
                ///
                //var consumer = await Consumer<MyRoot>.CreateAsync(s101Client);

                //var mixer = (INode)consumer.Root.DynamicChildren.First(c => c.Identifier == "MixerEmberIdentifier");
                //var mute = (IParameter)mixer.Children.First(c => c.Identifier == "Mute");
                //mute.Value = true;

                //var mixer = (INode)consumer.Root.DynamicChildren.First(c => c.Identifier == "MyRoot/Sources/FPGM 1/Fader");
                //var faderPosition = (IParameter)mixer.Children.First(c => c.Identifier == "Sapphire/Sources/FPGM 1/FaderPosition");
                //mute.Value = true;


                //INode root = await Consumer<Root>.CreateAsync(S101Client);





                //INode root = consumer.Root;

                //// Navigate to the parameter we're interested in.
                //var sapphire = (INode)root.Children.First(c => c.Identifier == "Sapphire");
                //var sources = (INode)sapphire.Children.First(c => c.Identifier == "Sources");
                //var fpgm1 = (INode)sources.Children.First(c => c.Identifier == "FPGM 1");
                //var fader = (INode)fpgm1.Children.First(c => c.Identifier == "Fader");
                //var faderPosition = (IParameter)fader.Children.First(c => c.Identifier == "Position");




                //// Cast to IParameter (safe if you know the structure)
                //var faderPosition = (IParameter)positionElement;

                ////Thread.Sleep(100);
                ///
                //faderPosition.PropertyChanged -= (sender, args) =>
                //{
                //};

                faderPosition.PropertyChanged += (sender, args) =>
                {
                    //var updatedValue = ((dynamic)sender).Value;
                    var updatedValue = Convert.ToInt32(((IParameter)sender).Value);
                    this.BeginInvoke((Action)(() =>
                    {


                        var parameter = sender as IParameter;
                    if (parameter != null)
                    {
                        textBox1.AppendText($"[IN] Fader.Position changed: {((IParameter)sender).Value}\r\n");
                            int clampedValue = Math.Min(Math.Max((int)updatedValue, trackBar1.Minimum), trackBar1.Maximum);
                            if (trackBar1.Value != clampedValue)
                                trackBar1.Value = clampedValue;
                        }

                    }));


                };
                //// SetupFaderPositionListener(faderPosition);
                //SetupTrackBarSync(faderPosition, consumer);
                //await consumer.SendAsync();


                trackBar1.Scroll += async (s, e) =>
                {
                    int newValue = trackBar1.Value;

                    faderPosition.Value = (long)newValue;
                    await consumer.SendAsync();

                    textBox1.AppendText($"[OUT] Sent Fader.Position: {newValue}\r\n");
                };

                await Task.Delay(Timeout.Infinite); // Keep app alive
            }

        }


        //private void SetupTrackBarSync(dynamic faderPosition, dynamic consumer)
        //{
        //    //WriteChildren(consumer.Root, 0);
        //    trackBar1.Scroll += async (s, e) =>
        //    {
        //        int newValue = trackBar1.Value;

        //        faderPosition.Value = Convert.ToInt32(newValue);
        //        await consumer.SendAsync();

        //        textBox1.AppendText($"[OUT] Sent Fader.Position: {newValue}\r\n");
        //    };
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            Task startEmberPlusListenerAsync = StartEmberPlusListenerAsync();
            



            //// Specify the path of program.cs
            //string inputFilePath = "C:\\temp26-Emberpluscsharp-WindowsForms\\EmberPlusWinForms\\EmberPlusWinForms\\bin\\Debug\\net8.0-windows\\EmberModel.cs";
            //string outputFilePath = "C:\\temp26-Emberpluscsharp-WindowsForms\\EmberPlusWinForms\\EmberPlusWinForms\\bin\\Debug\\net8.0-windows\\test.json";
            //// Step 1: Read content from program.cs
            //string fileContent = File.ReadAllText(inputFilePath);

            //// Step 2: Replace CR and LF with their JSON-escaped counterparts
            //fileContent = fileContent.Replace("\r", "").Replace("\n", "n");

            //// Step 2: Process data (in this example, we take the content as-is)
            //var data = new
            //{
            //    FileName = "program.cs",
            //    Content = fileContent
            //};
            //// Step 3: Convert data to JSON
            //string jsonString = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            //// Step 4: Write JSON to file
            //File.WriteAllText(outputFilePath, jsonString);

        }

        private void button2_Click(object sender, EventArgs e)
        {
           // Task emberTreePlusListenerAsync = EmberTreePlusListenerAsync();
        }
    }
}




//private async Task StartEmberPlusListenerAsync()
//{
//    using (var client = await ConnectAsync("192.168.1.2", 9000))
//    using (var consumer = await Consumer<SapphireRoot>.CreateAsync(client))
//        {

//        var fader = consumer.Root.Sapphire.Sources.Fpgm1.Fader.Position;

//        // ✅ Receive updates from hardware
//        fader.PropertyChanged += (sender, args) =>
//        {
//            var updatedValue = ((dynamic)sender).Value;

//            this.BeginInvoke((Action)(() =>
//            {
//                // Clamp just in case
//                int clampedValue = Math.Min(Math.Max((int)updatedValue, trackBar1.Minimum), trackBar1.Maximum);
//                if (trackBar1.Value != clampedValue) // Prevent unnecessary redraw
//                    trackBar1.Value = clampedValue;

//                textBox1.AppendText($"[IN] Fader.Position changed: {updatedValue}\r\n");
//            }));
//        };



//        // ✅ Send updates to hardware when user moves the slider
//        trackBar1.Scroll += async (s, e) =>
//        {
//            int newValue = trackBar1.Value;

//            fader.Value = newValue;
//            await consumer.SendAsync();

//            textBox1.AppendText($"[OUT] Sent Fader.Position: {newValue}\r\n");
//        };
//        await Task.Delay(Timeout.Infinite); // Keep app alive

//    }
//}

//    var valueChanged = new TaskCompletionSource<string>();
//    var positionParameter = consumer.Root.Sapphire.Sources.Fpgm1.Fader.Position;
//    positionParameter.PropertyChanged += (s, e) => valueChanged.SetResult(((IElement)s).GetPath());

//    Console.WriteLine("Waiting for the parameter to change...");
//    Console.WriteLine("A value of the element with the path {0} has been changed.", await valueChanged.Task);
//}
//{

//var fader = consumer.Root.Sapphire.Sources.Fpgm1.Fader;
//fader.DBValue.Value = -67.0;
//fader.Position.Value = 128;
//await consumer.SendAsync();

//var fader = consumer.Root.Sapphire.Sources.Fpgm1.Fader;


//}

//{
//////    var positionParameter = consumer.Root.Sapphire.Sources.Fpgm1.Fader.Position;

//////    positionParameter.PropertyChanged += (sender, args) =>
//////    {
//////        var updatedValue = ((dynamic)sender).Value;
//////        textBox1.AppendText($"Fader.Position changed: {updatedValue}\r\n");
//////        int clampedValue = Math.Min(Math.Max((int)updatedValue, trackBar1.Minimum), trackBar1.Maximum);
//////        trackBar1.Value = clampedValue;
//////    };

//////    textBox1.AppendText("Listening for position changes...");
//////    await Task.Delay(Timeout.Infinite); // Keep app alive
///
/// 
///


//trackBar1.Scroll += async (s, e) =>
//{
//    var newValue = trackBar1.Value;
//    //fader.Position.Value = newValue;
//    //await consumer.SendAsync();
//};



