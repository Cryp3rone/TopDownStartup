using NaughtyAttributes;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName ="EnemiesReference")]
    public class EnemiesReference : MultipleReference<Entity>
    {
        public void Init()
        {
            _instances.Clear();
        }

        // Testing if the references are available
        /*[Button]
        public void AccessFirstEnemy()
        {
            Debug.Log(_instances[0].name);
        }*/
    }
}
