using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName ="SpawnerData")]
    public class SpawnerData : ScriptableObject
    {
        public List<Wave> waves = new List<Wave>();

    }
}
