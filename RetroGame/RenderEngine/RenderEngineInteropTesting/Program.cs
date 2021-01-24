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
            var button = new Button(win.Size / 2, "this is a button", IMenu.Anchor.Center, font, Vector2.One * 10);
            var field = new TextBox(new Vector2(win.Size.X/2, win.Size.Y), IMenu.Anchor.Center, font, Vector2.One * 10);
            button.OnMousePress += (k, x, y) => Console.WriteLine($"Tamer {k} {x} {y}");
            tb.OnScroll += (x, y) => Console.WriteLine($"Scroll {x} {y}");
            field.OnCharReceived += (c) => Console.WriteLine($"WOLA ON A RECU {c}");
            sc.AddNode(a);
            mm.AddNode(tb);
            mm.AddNode(button);
            mm.AddNode(field);
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
