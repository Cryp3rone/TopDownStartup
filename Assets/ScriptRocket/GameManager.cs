using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private EnemiesReference enemiesReference;
        [SerializeField] private SpawnerManager spawnerManager;
        
        private void Awake()
        {
            enemiesReference.Init();
            spawnerManager.StartSystem();
        }
    }
}
