// <copyright file="Program.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
// This console application allows one to easily create some random temporary tests.
// </summary>
// <author>Paul Ennemoser</author>

namespace Atom.Tests.Console
{
    using System;
    using System.Diagnostics;
    using Atom.Math;

    public static class Program
    {
        static void Main( string[] args )
        {
            Curve curve = new Curve();
            curve.Keys.Add( new CurveKey( 0.0f, 0.0f ) );
            curve.Keys.Add( new CurveKey( 0.5f, 1.0f ) );
            curve.Keys.Add( new CurveKey( 1.0f, 10.0f ) );
            curve.Keys.Add( new CurveKey( 1.5f, 1.0f ) );
            curve.Keys.Add( new CurveKey( 2.0f, 0.0f ) );
            curve.PostLoop = CurveLoopType.Cycle;
            curve.PreLoop = CurveLoopType.Cycle;

            for( float i = 0; i < 3.0f; i += 0.05f )
            {
                Console.WriteLine(i + " --> " + curve.Evaluate(i) );
            }

            Console.Read();
        }

        private static double Time( Action action, string name )
        {
            var sw = Stopwatch.StartNew();

            action();

            sw.Stop();
          //  System.Console.WriteLine( name + ": " + sw.Elapsed.TotalMilliseconds );
            return sw.Elapsed.TotalMilliseconds;
        }
    }
}
