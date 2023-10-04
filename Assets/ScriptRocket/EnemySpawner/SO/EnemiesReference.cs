using NaughtyAttributes;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName ="EnemiesReference")]
    public class EnemiesReference : MultipleReference<Entity>
    {
        public void Init()
        {
            ClearList();
        }
    }
}
