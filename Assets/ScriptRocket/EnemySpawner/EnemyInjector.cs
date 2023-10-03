using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class EnemyInjector : MonoBehaviour, IPoolable
    {
        [SerializeField] Entity _e;
        [SerializeField] EnemiesReference _ref;

        ISet<Entity> RealRefSet => _ref;
        IRemove<Entity> RealRefRemove => _ref;

        void SetRef()
        {
            RealRefSet.Set(_e);
        }

        void RemoveRef()
        {
            RealRefRemove.Remove(_e);
        }
        
        #region Pool Functions
        public void Activate()
        {
            Debug.Log("Activate");
            SetRef();
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            Debug.Log("Deactivate");
            RemoveRef();
            gameObject.SetActive(false);
        }

        public void Destroy()
        {
            Debug.Log("Destroy");
            Destroy(gameObject);
        }
        #endregion
    }
}
