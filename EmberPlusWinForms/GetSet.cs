using Lawo.EmberPlusSharp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmberPlusWinForms
{
    public partial class GetSet
    {

        //-----------------------------------------------------------------------------------------------------------------------------------------
        // Auron  setters and getters    // "/auron/modules/module_{i}/path/fader"
        //-----------------------------------------------------------------------------------------------------------------------------------------

        public sealed class AuronRoot : Root<AuronRoot>
        {
            internal auro auron { get; private set; }
        }

        public sealed class auro : FieldNode<auro>
        {
            internal Modules modules { get; private set; }
        }

        public sealed class Modules : FieldNode<Modules>
        {
            [Element(Identifier = "module_1")]
            internal Module module_1 { get; private set; }

            [Element(Identifier = "module_2")]
            internal Module module_2 { get; private set; }

            [Element(Identifier = "module_3")]
            internal Module module_3 { get; private set; }

            [Element(Identifier = "module_4")]
            internal Module module_4 { get; private set; }

            [Element(Identifier = "module_5")]
            internal Module module_5 { get; private set; }

            [Element(Identifier = "module_6")]
            internal Module module_6 { get; private set; }
            [Element(Identifier = "module_7")]
            internal Module module_7 { get; private set; }

            [Element(Identifier = "module_8")]
            internal Module module_8 { get; private set; }

            [Element(Identifier = "module_9")]
            internal Module module_9 { get; private set; }

            [Element(Identifier = "module_10")]
            internal Module module_10 { get; private set; }
        }





        public sealed class Module : FieldNode<Module>
        {
            [Element(Identifier = "control")]
            internal Control control { get; private set; }

            [Element(Identifier = "path")]
            internal Path path { get; private set; }
        }

        public sealed class Control : FieldNode<Control>
        {
            [Element(Identifier = "sw_1")]
            internal SW sw_1 { get; private set; }

            [Element(Identifier = "sw_2")]
            internal SW sw_2 { get; private set; }

            [Element(Identifier = "sw_3")]
            internal SW sw_3 { get; private set; }

            [Element(Identifier = "sw_4")]
            internal SW sw_4 { get; private set; }
        }

        public sealed class SW : FieldNode<SW>
        {
            [Element(Identifier = "state")]
            internal BooleanParameter state { get; private set; }
            [Element(Identifier = "mode")]
            internal IntegerParameter mode { get; private set; }
            [Element(Identifier = "color_on")]
            internal IntegerParameter color_on { get; private set; }
            [Element(Identifier = "color_off")]
            internal IntegerParameter color_off { get; private set; }
        }



        public sealed class Path : FieldNode<Path>
        {
            [Element(Identifier = "eq")]
            internal BooleanParameter eq { get; private set; }

            [Element(Identifier = "fader")]
            internal IntegerParameter fader { get; private set; }
            
        }

        /*
        //-----------------------------------------------------------------------------------------------------------------------------------------
        // Sapphire  setters and getters     "Sapphire/Sources/FPGM 1/Fader/Position"
        //-----------------------------------------------------------------------------------------------------------------------------------------

        private sealed class SapphireRoot : Root<SapphireRoot>
        {
            internal Sapphire Sapphire { get; private set; }
            
        }

        private sealed class Sapphire : FieldNode<Sapphire>
        {
            internal IntegerParameter test { get; private set; }
            //  internal Sources Sources { get; private set; }
            // internal Test Test { get; private set; }                // Deze staat hier als test, maar wordt niet gebruikt.
        }


        private sealed class Sources : FieldNode<Sources>
        {
            [Element(Identifier = "FPGM 1")]
            internal Source Fpgm1 { get; private set; }
        }

        private sealed class Source : FieldNode<Source>
        {
            internal Fader Fader { get; private set; }
        }

        private sealed class Fader : FieldNode<Fader>
        {
            [Element(Identifier = "dB Value")]
            internal RealParameter DBValue { get; private set; }

            internal IntegerParameter Position { get; private set; }
        }
        */


    }
}
