using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using EksedraEngine;

namespace NewSuperChunks
{
    static class Program
    {
        #if _WINDOWS
        #else
            // Must happen on linux or else threads will crash X11
            [DllImport("X11")]
            extern public static int XInitThreads();
        #endif

        [STAThread]
        static void Main() {
            #if _WINDOWS
            #else
                // Required for threads on linux to work
                // Must be called before anything else not just before window unfotunately.
                // Thus can't be part of lib
                XInitThreads();
            #endif

            Console.WriteLine("Program started!");

            Engine engine = new Engine(
                                1280, 720, "Eksedra Engine", "title", 
                                new List<Type>() {
                                    typeof(ControlObject),
                                    typeof(Player),
                                    typeof(Rock),
                                    typeof(AirBlock),
                                    typeof(CloudThrough),
                                    typeof(BlockType)
                                }, "NewSuperChunks");

            Console.WriteLine("Running engine!");
            engine.Run();
        }
    }
}
