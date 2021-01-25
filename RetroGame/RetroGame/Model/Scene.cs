using RenderEngine;
using RetroGame.Services;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace RetroGame.Model
{
    abstract class Scene
    {
        public abstract IEnumerable<IMenu> Menu { get; }
        public abstract bool RequireClearOnLoad { get; }
        public abstract bool RequireClearOnExit { get; }

        public event EventHandler OnEnter;
        public event EventHandler OnExit;

        public Scene()
        {
            BuildScene();
        }

        public void Enter()
        {
            RenderService.Instance.LoadMenu(Menu, RequireClearOnLoad);
        }

        public abstract void BuildScene();
        public void OnResize() => BuildScene();
        public void Exit()
        {
            if (RequireClearOnExit)
                RenderService.Instance.ClearEntireScene();
        }
    }
}
