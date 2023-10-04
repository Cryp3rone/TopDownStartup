using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Game
{
    public class EnemyPool
    {
        private ObjectPool<GameObject> _pool;
        private readonly GameObject _prefab;

        public ObjectPool<GameObject> Pool
        {
            get
            {
                if (_pool == null)
                {
                    _pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, true, 10, 30);
                }
                return _pool;
            }
        }
        
        public EnemyPool(GameObject prefab)
        {
            _prefab = prefab;
        }
        
        GameObject CreatePooledItem()
        {
            //Creating the enemy
            GameObject go = Object.Instantiate(_prefab);
            return go;
        }
    
        // Called when an item is returned to the pool using Release
        void OnReturnedToPool(GameObject go)
        {
            //Deactivate object
            go.gameObject.GetComponent<EnemyInjector>().Deactivate();
        }
    
        // Called when an item is taken from the pool using Get
        void OnTakeFromPool(GameObject go)
        {
            //Activate object
            go.gameObject.GetComponent<EnemyInjector>().Activate();
        }
    
        // If the pool capacity is reached then any items returned will be destroyed.
        // We can control what the destroy behavior does, here we destroy the GameObject.
        void OnDestroyPoolObject(GameObject go)
        {
            //Destroy Object
            go.gameObject.GetComponent<EnemyInjector>().Destroy();
        }
        
    }
}


