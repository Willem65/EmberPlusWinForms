using Lawo.EmberPlusSharp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static EmberPlusWinForms.GetSet;

namespace EmberPlusWinForms
{

    public partial class GetSet
    {
        //-----------------------------------------------------------------------------------------------------------------------------------------
        // Auron setters and getters // "/auron/modules/module_{i}/path/fader"
        //-----------------------------------------------------------------------------------------------------------------------------------------

        //public sealed class AuronRoot : Root<AuronRoot>
        //{
        //    //    internal auron auron { get; private set; } 
        //    //}
        //    internal auron auron { get; private set; }
        //}

        //public sealed class auron : FieldNode<auron>
        //{
        //    internal modules modules { get; private set; }
        //}

        //public sealed class modules : FieldNode<modules>
        //{
        //    internal A module_1 { get; private set; }
        //    internal A module_2 { get; private set; }
        //    //internal A module_3 { get; private set; }
        //    //internal module_4 module_4 { get; private set; }
        //    //internal module_5 module_5 { get; private set; }
        //    //internal module_6 module_6 { get; private set; }
        //    //internal module_7 module_7 { get; private set; }
        //    //internal module_8 module_8 { get; private set; }
        //    //internal module_9 module_9 { get; private set; }
        //    //internal module_10 module_10 { get; private set; }
        //}

        //public sealed class A : FieldNode<A>
        //{
        //    [Element(Identifier = "control")]
        //    internal B control { get; private set; }
        //    [Element(Identifier = "path")]
        //    internal C path { get; private set; }
        //}

        //public sealed class B : FieldNode<B>
        //{
        //    [Element(Identifier = "sw_1")]
        //    internal F sw_1 { get; private set; }

        //    [Element(Identifier = "sw_2")]
        //    internal F sw_2 { get; private set; }

        //    [Element(Identifier = "sw_3")]
        //    internal F sw_3 { get; private set; }

        //    [Element(Identifier = "sw_4")]
        //    internal F sw_4 { get; private set; }
        //}

        //public sealed class F : FieldNode<F>
        //{
        //    [Element(Identifier = "state")]
        //    internal BooleanParameter state { get; private set; }
        //    [Element(Identifier = "mode")]
        //    internal IntegerParameter mode { get; private set; }
        //    [Element(Identifier = "color_on")]
        //    internal IntegerParameter color_on { get; private set; }
        //    [Element(Identifier = "color_off")]
        //    internal IntegerParameter color_off { get; private set; }
        //}

        //public sealed class C : FieldNode<C>
        //{
        //    [Element(Identifier = "fader")]
        //    internal IntegerParameter fader { get; private set; }
        //}




        //public sealed class AuronRoot : Root<AuronRoot>
        //{
        //    internal auron auron { get; private set; }
        //}
        //public sealed class auron : FieldNode<auron>
        //{
        //    internal modules modules { get; private set; }
        //}

        //public sealed class modules : FieldNode<modules>
        //{
        //    internal module_1 module_1 { get; private set; }
        //    internal module_1 module_2 { get; private set; }
        //}

        //public sealed class module_1 : FieldNode<module_1>
        //{
        //    internal path path { get; private set; }
        //    internal control control { get; private set; }
        //}

        //public sealed class module_2 : FieldNode<module_2>
        //{
        //    internal path path { get; private set; }
        //    internal control control { get; private set; }
        //}

        //public sealed class path : FieldNode<path>
        //{
        //    [Element(Identifier = "eq")]
        //    internal BooleanParameter eq { get; private set; }
        //    [Element(Identifier = "fader")]
        //    internal IntegerParameter fader { get; private set; }
        //}

        //public sealed class control : FieldNode<control>
        //{
        //    internal sw_1 sw_1 { get; private set; }
        //    internal sw_2 sw_2 { get; private set; }
        //    internal sw_3 sw_3 { get; private set; }
        //    internal sw_4 sw_4 { get; private set; }
        //}

        //public sealed class sw_1 : FieldNode<sw_1>
        //{
        //    [Element(Identifier = "state")]
        //    internal BooleanParameter state { get; private set; }
        //    [Element(Identifier = "mode")]
        //    internal IntegerParameter mode { get; private set; }
        //    [Element(Identifier = "color_on")]
        //    internal IntegerParameter color_on { get; private set; }
        //    [Element(Identifier = "color_off")]
        //    internal IntegerParameter color_off { get; private set; }
        //}

        //public sealed class sw_2 : FieldNode<sw_2>
        //{
        //    [Element(Identifier = "state")]
        //    internal BooleanParameter state { get; private set; }
        //    [Element(Identifier = "mode")]
        //    internal IntegerParameter mode { get; private set; }
        //    [Element(Identifier = "color_on")]
        //    internal IntegerParameter color_on { get; private set; }
        //    [Element(Identifier = "color_off")]
        //    internal IntegerParameter color_off { get; private set; }
        //}
        //public sealed class sw_3 : FieldNode<sw_3>
        //{
        //    [Element(Identifier = "state")]
        //    internal BooleanParameter state { get; private set; }
        //    [Element(Identifier = "mode")]
        //    internal IntegerParameter mode { get; private set; }
        //    [Element(Identifier = "color_on")]
        //    internal IntegerParameter color_on { get; private set; }
        //    [Element(Identifier = "color_off")]
        //    internal IntegerParameter color_off { get; private set; }
        //}

        //public sealed class sw_4 : FieldNode<sw_4>
        //{
        //    [Element(Identifier = "state")]
        //    internal BooleanParameter state { get; private set; }
        //    [Element(Identifier = "mode")]
        //    internal IntegerParameter mode { get; private set; }
        //    [Element(Identifier = "color_on")]
        //    internal IntegerParameter color_on { get; private set; }
        //    [Element(Identifier = "color_off")]
        //    internal IntegerParameter color_off { get; private set; }
        //}











        //public sealed class AuronRoot : Root<AuronRoot>
        //{
        //    internal auron auron { get; private set; }
        //}
        //public sealed class auron : FieldNode<auron>
        //{
        //    internal modules modules { get; private set; }
        //}

        //public sealed class modules : FieldNode<modules>
        //{
        //    internal module_1 module_1 { get; private set; }
        //    internal module_2 module_2 { get; private set; }
        //}

        //public sealed class module_1 : FieldNode<module_1>
        //{
        //    internal path path { get; private set; }
        //    internal control control { get; private set; }
        //}

        //public sealed class path : FieldNode<path>
        //{
        //    [Element(Identifier = "fader")]
        //    internal IntegerParameter fader { get; private set; }
        //}

        //public sealed class module_2 : FieldNode<module_2>
        //{
        //    internal path path { get; private set; }
        //    internal control control { get; private set; }
        //}

        //public sealed class control : FieldNode<control>
        //{
        //    internal sw_1 sw_1 { get; private set; }
        //    internal sw_1 sw_2 { get; private set; }
        //    internal sw_1 sw_3 { get; private set; }
        //    internal sw_1 sw_4 { get; private set; }
        //}








        //public partial class GetSet
        //{

            //-----------------------------------------------------------------------------------------------------------------------------------------
            // Auron  setters and getters    // "/auron/modules/module_{i}/path/fader"
            //-----------------------------------------------------------------------------------------------------------------------------------------

            public sealed class AuronRoot : Root<AuronRoot>
            {
                internal E auron { get; private set; }
            }

            public sealed class E : FieldNode<E>
            {
                internal D modules { get; private set; }
            }

            public sealed class D : FieldNode<D>
            {
                [Element(Identifier = "module_1")]
                internal A module_1 { get; private set; }

                [Element(Identifier = "module_2")]
                internal A module_2 { get; private set; }

                [Element(Identifier = "module_3")]
                internal A module_3 { get; private set; }

                [Element(Identifier = "module_4")]
                internal A module_4 { get; private set; }

                [Element(Identifier = "module_5")]
                internal A module_5 { get; private set; }

                [Element(Identifier = "module_6")]
                internal A module_6 { get; private set; }
                [Element(Identifier = "module_7")]
                internal A module_7 { get; private set; }

                [Element(Identifier = "module_8")]
                internal A module_8 { get; private set; }

                [Element(Identifier = "module_9")]
                internal A module_9 { get; private set; }

                [Element(Identifier = "module_10")]
                internal A module_10 { get; private set; }
            }





            public sealed class A : FieldNode<A>
            {
                [Element(Identifier = "control")]
                internal B control { get; private set; }

                [Element(Identifier = "path")]
                internal C path { get; private set; }
            }

            public sealed class B : FieldNode<B>
            {
                [Element(Identifier = "sw_1")]
                internal F sw_1 { get; private set; }

                [Element(Identifier = "sw_2")]
                internal F sw_2 { get; private set; }

                [Element(Identifier = "sw_3")]
                internal F sw_3 { get; private set; }

                [Element(Identifier = "sw_4")]
                internal F sw_4 { get; private set; }
            }

            public sealed class F : FieldNode<F>
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



            public sealed class C : FieldNode<C>
            {
                //[Element(Identifier = "eq")]
                //internal BooleanParameter eq { get; private set; }

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
