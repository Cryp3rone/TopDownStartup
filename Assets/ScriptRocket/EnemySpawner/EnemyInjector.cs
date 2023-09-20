using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class EnemyInjector : MonoBehaviour
    {
        [SerializeField] Entity _e;
        [SerializeField] EnemiesReference _ref;

        ISet<Entity> RealRef => _ref;

        public IReadOnlyList<int> T { get => t; }

        List<int> t;

        void Awake()
        {
            //_ref.Set(_e);
            RealRef.Set(_e);
        }

        private void OnDestroy()
        {
            //Iset remove reference when enemy is killed

        }
    }
}
