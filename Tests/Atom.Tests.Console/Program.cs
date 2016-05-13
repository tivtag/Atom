// <copyright file="Program.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
// This console application allows one to easily create some random temporary tests.
// </summary>
// <author>Paul Ennemoser (Tick)</author>


namespace Atom.Tests.Console
{
    using System;
    using Atom.Scripting;
    using System.Diagnostics;
    using System.Collections.Generic;
    using Atom.Math;

    public sealed class ScriptPlayer
    {
        public string Name { get; set; }
        public int HP { get; set; }
    }

    class Program
    {
        [STAThread]
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


//            var dictionary = new Dictionary<string, int>( 10000 );
//            for( int i = 0; i < 10000; ++i )
//            {
//                dictionary[i.ToString()] = i;
//            }

//            var values = dictionary.Values;

//            var list = new List<int>( 10000 );
//            for( int i = 0; i < 10000; ++i )
//            {
//                list.Add( i );
//            }

//            var arr = new int[10000];
//            for( int i = 0; i < 10000; ++i )
//            {
//                arr[i] = i;
//            }
            
//            double timeA = 0.0;
//            double timeB = 0.0;
//            double timeC = 0.0;
//            double timeD = 0.0;
//            const int Count = 10000;
            
//            for( int x = 0; x < Count; ++x )
//            {                

//                timeB += Time( () => {

//                    int result = 0;

//                    foreach( var item in list )
//                    {
//                        result += item;
//                    }

//                    Console.WriteLine( result );

//                }, "foreach  list" );

//                timeA += Time( () => {

//                    int result = 0;

//                    foreach( var item in values )
//                    {
//                        result += item;
//                    }

//                    Console.WriteLine( result );

//                }, "foreach dictionary" );
                
//                timeC += Time( () => {

//                    int result = 0;

//                    for( int i = 0; i < list.Count; ++i )
//                    {
//                        result += list[i];
//                    }

//                    Console.WriteLine( result );

//                }, "for  list" );
                
//                timeD += Time( () => {

//                    int result = 0;

//                    for( int i = 0; i < arr.Length; ++i )
//                    {
//                        result += arr[i];
//                    }

//                    Console.WriteLine( result );

//                }, "for  arr" );
//            }


//            Console.WriteLine();
//            Console.WriteLine( timeA / (float)Count );
//            Console.WriteLine( timeB / (float)Count );
//            Console.WriteLine( timeC / (float)Count );
//            Console.WriteLine( timeD / (float)Count );


//            Console.ReadLine();
//        }

//        private static void TestScripts()
//        {
//            var player = new ScriptPlayer() {
//                Name = "Link",
//                HP = 10
//            };

//            var scripts = new ScriptingEnvironment();
//            scripts.SetGlobal( "player", player );
//            string code = @"
//                    puts '---s1---'
//                    puts @val
//                    @val = 10
//                    puts @val
//                ";
//            const int Count = 10;

//            for( int x = 0; x < 2; x++ )
//            {
//                var s = scripts.CreateScript( code, true );

//                double t2 = Time( () => {

//                    for( int i = 0; i < Count; ++i )
//                    {
//                        s.Execute();
//                    }
//                }, "Compiled, scope" );

//                var s1 = scripts.CreateScript( code, false );

//                double t1 = Time( () => {
//                    for( int i = 0; i < Count; ++i )
//                    {
//                        s1.Execute();
//                    }
//                }, "Compiled, no scope" );


//                double t3 = Time( () => {

//                    for( int i = 0; i < Count; ++i )
//                    {
//                        scripts.Execute( code );
//                    }
//                }, "uncompiled, no scope" );


//                Console.WriteLine( "c_: " + t1 );
//                Console.WriteLine( "cs: " + t2 );
//                Console.WriteLine( "uc: " + t3 );


//                Console.ReadLine();
//            }
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
