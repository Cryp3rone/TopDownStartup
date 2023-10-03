using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class EnemyInjector : MonoBehaviour
    {
        [SerializeField] Entity _e;
        [SerializeField] EnemiesReference _ref;

        ISet<Entity> RealRefSet => _ref;
        IRemove<Entity> RealRefRemove => _ref;

        void Awake()
        {
            SetRef();
        }

        public void SetRef()
        {
            RealRefSet.Set(_e);
        }

        public void RemoveRef()
        {
            RealRefRemove.Remove(_e);
        }
    }
}
