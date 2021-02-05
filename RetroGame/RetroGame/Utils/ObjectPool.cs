using RenderEngine;
using RetroGame.Services;
using System;
using System.Collections.Concurrent;
using System.Numerics;

namespace RetroGame.Utils
{
    internal class ObjectPool<T> where T : IGraphNode
    {
        private readonly ConcurrentStack<T> _stack = new ConcurrentStack<T>();

        private readonly Func<T> _instanciationFunction;

        public ObjectPool(Func<T> instanciationFunction, int size = 50)
        {
            _instanciationFunction = instanciationFunction;
            for (var i = 0; i < size; i++)
                _stack.Push(_instanciationFunction());
        }

        public T Spawn(Vector2 position)
        {
            if (!_stack.TryPop(out var obj))
                obj = _instanciationFunction();

            obj.Position = position;
            RenderService.Instance.AddRenderItem(obj);
            return obj;
        }

        public void Despawn(T obj)
        {
            _stack.Push(obj);
            RenderService.Instance.RemoveRenderItem(obj);
        }
    }
}
