using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
            var font = new Font("D:/Code/Epitech/PL/RetroGame/RetroGame/RenderEngine/RenderEngineTesting/Fonts/Roboto.ttf", 36);
            var tb = new TextBlock(new Vector2(win.Size.X, 0), "", IMenu.Anchor.BottomRight, font, Vector2.One * 10);
            sc.AddNode(a);
            mm.AddNode(tb);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var r = new Random();
            while (!win.ShouldClose())
            {

                float currentFrame = (float)stopwatch.Elapsed.TotalSeconds;
                var deltaTime = currentFrame - lastFrame;
                tb.Text = $"{Math.Round(1 / deltaTime)}fps / {Math.Round(deltaTime * 1000)}ms";
                lastFrame = currentFrame;
                sc.Update(deltaTime);
                sc.Render(deltaTime);
            }
            sc.Dispose();
        }
    }
}
