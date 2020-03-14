using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using EksedraEngine;
using SFML.Graphics;

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

            EksedraSprite loadSprite = new EksedraSprite(new Texture("images/blueokirislogo-2018.png"), new IntRect[] { new IntRect(0, 0, 800, 600) });
            Engine engine = new Engine(
                                1280, 720, "New Super Chunks", "title", 
                                new List<Type>() {
                                    typeof(ControlObject),
                                    typeof(Player),
                                    typeof(AirBlock),
                                    typeof(CloudThrough),
                                    typeof(GrassThrough),
                                    typeof(BlockType),
                                    typeof(BoundaryBlock),
                                    typeof(LadderBlock),
                                    typeof(WaterPlatform),
                                    typeof(Water),
                                }, "NewSuperChunks", loadSprite);

            Console.WriteLine("Running engine!");
            engine.Run();
        }
    }
}
