using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;
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
            SpawnEnemy(ChooseEnemyWithWeight(), pos);
            
            _spawnedEnemies++;
            
            float randomTime = Random.Range(spawnMinInterval, spawnMaxInterval);
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
        
        void EnemyDie(GameObject go, EntityType type)
        {
            switch (type)
            {
                case EntityType.EnemyMelee:
                    _meleePool.Pool.Release(go);
                    break;
                case EntityType.EnemyDistance:
                    _distancePool.Pool.Release(go);
                    break;
                case EntityType.EnemyBoss:
                    _bossPool.Pool.Release(go);
                    break;
            }
        }

        EntityType ChooseEnemyWithWeight()
        {
            var wavePercent = _currentWave.waveEnemyPercent;
            var enemyTable = new int[3];
            enemyTable[0] = wavePercent.melee;
            enemyTable[1] = wavePercent.distance;
            enemyTable[2] = wavePercent.boss;
            var totalWeight = enemyTable[0] + enemyTable[1] + enemyTable[2];
            var enemyRandomNumber = Random.Range(0, totalWeight);
  
            
            // 3 because we have 3 different enemy
            for (int i = 0; i < enemyTable.Length; i++)
            {
                if (enemyRandomNumber <= enemyTable[i])
                {
                    switch (i)
                    {
                        case 0:
                            return EntityType.EnemyMelee;
                        case 1:
                            return EntityType.EnemyDistance;
                        case 2:
                            return EntityType.EnemyBoss;
                    }
                }
                enemyRandomNumber -= enemyTable[i];
            }
            return EntityType.EnemyMelee;
        }

        void SpawnEnemy(EntityType type, Vector2 pos)
        {
            GameObject go = null;
            switch (type)
            {
                case EntityType.EnemyMelee: 
                    go = _meleePool.Pool.Get();
                    break;
                case EntityType.EnemyDistance:
                    go = _distancePool.Pool.Get();
                    break;
                case EntityType.EnemyBoss:
                    go = _bossPool.Pool.Get();
                    break;
            }
            
            go.transform.position = pos;
            Health health = go.GetComponent<Health>();
            health.SetEntityType(type);
            health.OnDie += EnemyDie;
        }
    }
}
