using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SpawnerManager : MonoBehaviour
    {
        [SerializeField] private EnemiesReference enemiesReference;
        [SerializeField] private SpawnerData spawnerData;
        [SerializeField] private float timeBtwWaves;
        
        public int EnemyAlive
        {
            get => enemiesReference._instances.Count;
            set
            {
                if (value == 0)
                {
                    _isWaveFinished = true;
                }
            }
        }

        private bool _isWaveFinished;
        private byte waveIndex = 0;

        public void StartSystem()
        {
            if (spawnerData.waves.Count == 0)
            {
                throw new SpawnerManagerException("Wave Count == 0");
            }
            
            SpawnWave();
        }

        private void SpawnWave()
        {
            
        }

        private void SetupBeforeNextWave()
        {
            waveIndex++;
            
            if (spawnerData.waves.Count <= waveIndex)
            {
                Win();
            }
            else
            {
                TransitionToNextWave();
            }
        }

        private void TransitionToNextWave()
        {
            StartCoroutine(Transition());
            
            IEnumerator Transition()
            {
                //Transition Effect
                yield return new WaitForSeconds(timeBtwWaves);
                SpawnWave();
            }
        }

        private void Win()
        {
            
        }
    }


}
