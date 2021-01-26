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
        }

        public void Enter()
        {
            BuildScene();
            RenderService.Instance.LoadMenu(Menu, RequireClearOnLoad);
        }

        protected void Reload()
        {
            RenderService.Instance.LoadMenu(Menu, true);
        }

        public abstract void BuildScene();
        public void OnResize() => Enter();
        public void Exit()
        {
            if (RequireClearOnExit)
                RenderService.Instance.ClearEntireScene();
        }
    }
}
