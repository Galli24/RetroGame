using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Threading;
using RenderEngine;
namespace RenderEngineInteropTesting
{
    class Program
    {
        private static void T()
        {
            float lastFrame = 0;
            float deltaTime = 0;

            var sc = new SceneGraph(new Vector2(2560, 1440), "C# Window");
            var a = new AnimatedSprite(new[] {
                "C:\\Users\\jerem\\Pictures\\PixelArt\\Bowser.png",
                "C:\\Users\\jerem\\Pictures\\PixelArt\\BowserPink.png",
                "C:\\Users\\jerem\\Pictures\\PixelArt\\BowserBlue.png", },
                1, Vector2.Zero, new Vector2(64, 96),
                new Vector2(3, 3));
            sc.Nodes = new List<IGraphNode>() { a };
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (true)
            {
                float currentFrame = (float)stopwatch.Elapsed.TotalSeconds;
                deltaTime = currentFrame - lastFrame;
                lastFrame = currentFrame;
                sc.Update(deltaTime);
                sc.Render(deltaTime);
            }
            sc.Dispose();
        }

        static void Main(string[] args)
        {
            T();
        }
    }
}
