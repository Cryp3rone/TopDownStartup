using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game
{
    [Serializable]
    public class Wave
    {
        public WaveEnemyPercent waveEnemyPercent = new WaveEnemyPercent();
        public byte enemyCount;
    }

    [Serializable]
    public class WaveEnemyPercent
    {
        [Range(0, 100)] 
        public int melee;
        [Range(0, 100)] 
        public int distance;
        [Range(0, 100)] 
        public int boss;
    }
}
