using UnityEngine;
using UnityEngine.Pool;

namespace PropsPools
{
    public class WorldObjectsPool
    {
        private ObjectPool<WorldPoolableObject> _objectsPool;
        private WorldPoolableObject _example;
        internal WorldObjectsPool(int defaultCapacity, WorldPoolableObject example)
        {
            _example = example;
            _objectsPool = new ObjectPool<WorldPoolableObject>
                (CreatePoolableItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, false, defaultCapacity);
        }

        private WorldPoolableObject CreatePoolableItem()
        {
            //var go = _example.GetGameObject;
            var instance = Object.Instantiate(_example);
            return instance;
        }

        private void OnTakeFromPool(WorldPoolableObject element)
        {
            element.OnTakeFromPool();
        }

        private void OnReturnedToPool(WorldPoolableObject element)
        {
            element.OnReturnedToPool();
        }

        private void OnDestroyPoolObject(WorldPoolableObject element)
        {
            element.OnDestroyCall();
        }

        public WorldPoolableObject Get()
        {
            return _objectsPool.Get();
        }
        public PooledObject<WorldPoolableObject> Get(out WorldPoolableObject v)
        {
            return _objectsPool.Get(out v);
        }
        internal void Release(WorldPoolableObject element)
        {
            _objectsPool.Release(element);
        }

        private void DisposePool()
        {
            _objectsPool.Dispose();
        }

        private void ClearPool()
        {
            _objectsPool.Clear();
        }
    }
}