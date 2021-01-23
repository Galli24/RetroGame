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
        static void Main()
        {
            float lastFrame = 0;
            var mm = new MenuManager();
            var sc = new SceneGraph(new Vector2(1920, 1080), "C# Window", mm);
            var win = sc.Win;
            var a = new AnimatedSprite(new[] {
                "C:\\Users\\jerem\\Pictures\\PixelArt\\Bowser.png",
                "C:\\Users\\jerem\\Pictures\\PixelArt\\BowserPink.png",
                "C:\\Users\\jerem\\Pictures\\PixelArt\\BowserBlue.png", },
                1, Vector2.Zero, new Vector2(64, 96), new Vector2(3, 3));

            sc.Nodes = new List<IGraphNode>() { a };
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (!win.ShouldClose())
            {
                float currentFrame = (float)stopwatch.Elapsed.TotalSeconds;
                var deltaTime = currentFrame - lastFrame;
                lastFrame = currentFrame;
                sc.Update(deltaTime);
                sc.Render(deltaTime);
            }
            sc.Dispose();
        }
    }
}
