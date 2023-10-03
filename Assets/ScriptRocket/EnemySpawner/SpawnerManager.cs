using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Random = UnityEngine.Random;

namespace Game
{
    public class SpawnerManager : MonoBehaviour
    {
        [SerializeField] private EnemiesReference enemiesReference;
        [SerializeField] private PlayerReference playerReference;

        //Object Pool System
        [SerializeField, Required] private GameObject meleePrefab;
        [SerializeField, Required] private GameObject distancePrefab;
        [SerializeField, Required] private GameObject bossPrefab;
        private EnemyPool _meleePool;
        private EnemyPool _distancePool;
        private EnemyPool _bossPool;
        
        //Spawning Var
        [SerializeField] private SpawnerData spawnerData;
        [SerializeField] private float timeBtwWaves;
        [SerializeField] private float spawnMinInterval;
        [SerializeField] private float spawnMaxInterval;
        [SerializeField] private float minDistanceFromPlayer;
        [SerializeField] private GameObject spawnParent;
        
        // Spawning range of X
        [Header ("X Spawn Range")]
        [SerializeField] private float xMin;
        [SerializeField] private float xMax;
 
        // Spawning range of Y
        [Header ("Y Spawn Range")]
        [SerializeField] private float yMin;
        [SerializeField] private float yMax;
        
        private bool _waitingForAllEnemiesToBeDead;
        private byte _waveIndex = 0;
        private Wave _currentWave;
        private int _spawnedEnemies;
        

        private void Awake()
        {
            enemiesReference.Init();
            enemiesReference.OnValueChanged += CheckIfWaveIsFinished;
            _meleePool = new EnemyPool(meleePrefab);
            _distancePool = new EnemyPool(distancePrefab);
            _bossPool = new EnemyPool(bossPrefab);
        }

        /*[Button]
        public void DebugMeleePool()
        {
            Debug.Log(_meleePool.Pool.CountAll);
        }

        [Button]
        public void CreateMelee()
        {
            GameObject go = _meleePool.Pool.Get();
        }*/

        [Button]
        public void KillRandomEnemy()
        {
            var randomEnemy = Random.Range(0, enemiesReference.Instances.Count - 1);
            //Need to know in which pool is the go
            _meleePool.Pool.Release(enemiesReference.Instances[randomEnemy].gameObject);
        }

        public void StartSystem()
        {
            if (spawnerData.waves.Count == 0)
            {
                throw new SpawnerManagerException("Wave Count == 0");
            }
            
            _currentWave = spawnerData.waves[_waveIndex];
            _spawnedEnemies = 0;
            SpawnWave();
        }
        
        private void SpawnWave()
        {
            if(_spawnedEnemies == _currentWave.enemyCount) 
            { 
                _waitingForAllEnemiesToBeDead = true;
                Debug.Log("_waitingForAllEnemiesToBeDead");
                return;
            }
            
            Vector2 pos;
            pos = new Vector2 (Random.Range (xMin, xMax), Random.Range (yMin, yMax));

            if (isPosOnPlayer(pos))
            {
                SpawnWave();
                return;
            }
            
            //Weighted spawn
            GameObject go = Instantiate(_meleePool.Pool.Get(), pos, Quaternion.identity);
            go.transform.parent = spawnParent.transform;
            
            _spawnedEnemies++;

            //Should be when enemies are killed
            float randomTime = Random.Range(spawnMinInterval, spawnMaxInterval);
            Debug.Log(randomTime);
            StartCoroutine(WaitRandomTime(randomTime));

            IEnumerator WaitRandomTime(float rt)
            {
                yield return new WaitForSeconds(rt);
                SpawnWave();
            }
        }

        void CheckIfWaveIsFinished(int enemyAlive)
        {
            Debug.Log("Enemy Alive :" + enemyAlive);
            if (enemyAlive == 0 && _waitingForAllEnemiesToBeDead)
            {
                SetupNextWave();
            }
        }

        void SetupNextWave()
        {
            Debug.Log("SetupNextWave");
            _waitingForAllEnemiesToBeDead = false;
            _waveIndex++;
            
            if (spawnerData.waves.Count <= _waveIndex)
            {
                Win();
            }
            else
            {
                _currentWave = spawnerData.waves[_waveIndex];
                _spawnedEnemies = 0;
                TransitionToNextWave();
            }
        }

        void TransitionToNextWave()
        {
            StartCoroutine(Transition());
            
            IEnumerator Transition()
            {
                //Transition Effect
                yield return new WaitForSeconds(timeBtwWaves);
                SpawnWave();
            }
        }

        void Win()
        {
            Debug.Log("Win");
        }

        bool isPosOnPlayer(Vector2 pos)
        {
            Vector2 playerPos = playerReference.Instance.transform.position;
            return Vector2.Distance(pos, playerPos) < minDistanceFromPlayer;
        }
    }


}
