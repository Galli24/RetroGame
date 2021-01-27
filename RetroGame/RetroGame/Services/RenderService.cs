using RenderEngine;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RetroGame.Services
{
    class RenderService
    {
        #region Declaration

        public enum ActionTime
        {
            BeforeFrame,
            AfterFrame
        }

        #endregion

        #region Properties

        public Window Window => _sceneGraph?.Win;

        private ConcurrentDictionary<Action, ActionTime> _actions = new ConcurrentDictionary<Action, ActionTime>();


        #endregion

        #region Singleton

        private static RenderService _instance;
        public static RenderService Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new RenderService();
                return _instance;
            }
        }

        #endregion


        #region Members

        private MenuManager _menuManager;
        private SceneGraph _sceneGraph;


        #region FPS

        private readonly Stopwatch _frameStopwatch = new Stopwatch();
        private TextBlock _fpsCounter;
        private bool _showFPS;
        #endregion

        #endregion

        #region Public Interface

        public void Init()
        {
            _menuManager = new MenuManager();
#if DEBUG
            _sceneGraph = new SceneGraph(new Vector2(1280, 720), "RetroGame Pog", _menuManager);
#else
            _sceneGraph = new SceneGraph(new Vector2(1920, 1080), "RetroGame Pog", _menuManager);
#endif
            _frameStopwatch.Start();
            var fpsFont = FontManager.Instance["Roboto"];
            _fpsCounter = new TextBlock(new Vector2(0, Window.Size.Y), "", IMenu.Anchor.TopLeft, fpsFont, Vector2.One * 10);
            Window.OnKeyAction += (key, _) =>
            {
                if (key == 294)
                    SceneManager.Instance.ReloadCurrentScene();
            };
        }

        public void LoadMenu(IEnumerable<IMenu> items, bool clearScene = false)
        {
            _menuManager.Nodes = new List<IMenu>(items);
            if (clearScene)
                _sceneGraph.Nodes = new List<IGraphNode>();

            if (_showFPS)
                AddRenderItem(_fpsCounter);
        }

        public void ClearEntireScene()
        {
            _sceneGraph.Nodes = new List<IGraphNode>();
            _menuManager.Nodes = new List<IMenu>();

            if (_showFPS)
                AddRenderItem(_fpsCounter);
        }

        public void AddRenderItem(IGraphNode item)
        {
            if (item is IMenu m)
                _menuManager.AddNode(m);
            else
                _sceneGraph.AddNode(item);
        }

        public void RemoveRenderItem(IGraphNode item)
        {
            if (item is IMenu m)
                _menuManager.RemoveNode(m);
            else
                _sceneGraph.RemoveNode(item);
        }

        public void DoInRenderThread(Action action, ActionTime actionTime = ActionTime.BeforeFrame)
        {
            _actions.TryAdd(action, actionTime);
        }

        public bool RenderFrame()
        {
            var shouldRender = !Window.ShouldClose();
            if (!shouldRender)
                return false;

            var preAction = _actions.Where(elt => elt.Value == ActionTime.BeforeFrame);
            var postAction = _actions.Where(elt => elt.Value == ActionTime.AfterFrame);

            foreach (var action in preAction)
            {
                action.Key.Invoke();
                _actions.TryRemove(action);
            }

            var elapsed = (float)_frameStopwatch.Elapsed.TotalSeconds;
            _frameStopwatch.Restart();
            _fpsCounter.Text = $"{Math.Round(1 / elapsed)}fps - {Math.Round(elapsed * 1000)}ms";

            _sceneGraph.Update(elapsed);
            _sceneGraph.Render(elapsed);

            foreach (var action in postAction)
            {
                action.Key.Invoke();
                _actions.TryRemove(action);
            }

            return true;
        }

        public void SetFPSVisibility(bool value)
        {
            AddRenderItem(_fpsCounter);
            _showFPS = value;
        }

#endregion



    }
}
