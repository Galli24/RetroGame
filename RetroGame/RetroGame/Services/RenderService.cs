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

        public float FrameTime { get; private set; }
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


        #region Debug info

        private readonly Stopwatch _frameStopwatch = new Stopwatch();
        private TextBlock _fpsCounter;
        private bool _showFPS;
        private TextBlock _pingCounter;
        private TextBlock _serverTickCounter;
        private TextBlock _clientTickCounter;
        private TextBlock _tickDiffCounter;

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
            _pingCounter = new TextBlock(new Vector2(0, Window.Size.Y - 30), "", IMenu.Anchor.TopLeft, fpsFont, Vector2.One * 10);
            _serverTickCounter = new TextBlock(new Vector2(0, Window.Size.Y - 60), "", IMenu.Anchor.TopLeft, fpsFont, Vector2.One * 10);
            _clientTickCounter = new TextBlock(new Vector2(0, Window.Size.Y - 90), "", IMenu.Anchor.TopLeft, fpsFont, Vector2.One * 10);
            _tickDiffCounter = new TextBlock(new Vector2(0, Window.Size.Y - 120), "", IMenu.Anchor.TopLeft, fpsFont, Vector2.One * 10);
            Window.OnKeyPressed += (key, _) =>
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
            {
                AddRenderItem(_fpsCounter);
                AddRenderItem(_pingCounter);
                AddRenderItem(_serverTickCounter);
                AddRenderItem(_clientTickCounter);
                AddRenderItem(_tickDiffCounter);
            }
        }

        public void LoadNodes(IEnumerable<IGraphNode> nodes, bool clearScene = false)
        {
            _sceneGraph.Nodes = new List<IGraphNode>(nodes);

            if (_showFPS)
            {
                AddRenderItem(_fpsCounter);
                AddRenderItem(_pingCounter);
                AddRenderItem(_serverTickCounter);
                AddRenderItem(_clientTickCounter);
                AddRenderItem(_tickDiffCounter);
            }

        }


        public void ClearEntireScene()
        {
            _sceneGraph.Nodes = new List<IGraphNode>();
            _menuManager.Nodes = new List<IMenu>();

            if (_showFPS)
            {
                AddRenderItem(_fpsCounter);
                AddRenderItem(_pingCounter);
                AddRenderItem(_serverTickCounter);
                AddRenderItem(_clientTickCounter);
                AddRenderItem(_tickDiffCounter);
            }
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
            _fpsCounter.Text = $"{Math.Round(1 / elapsed)}FPS";
            _pingCounter.Text = $"Ping: {NetworkManager.Instance.Ping}ms";
            _serverTickCounter.Text = $"Server tick: {GameManager.Instance.CurrentServerTick}";
            _clientTickCounter.Text = $"Client tick: {GameManager.Instance.CurrentClientTick}";
            _tickDiffCounter.Text = $"Tick difference: {GameManager.Instance.TickDiff}";
            FrameTime = elapsed;
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
            AddRenderItem(_pingCounter);
            AddRenderItem(_serverTickCounter);
            AddRenderItem(_clientTickCounter);
            AddRenderItem(_tickDiffCounter);
            _showFPS = value;
        }

#endregion



    }
}
