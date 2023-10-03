using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayerMunition : MonoBehaviour
    {
        public static PlayerMunition SharedInstance;

        [SerializeField] private List<bullet> pooledObjects;
        [SerializeField] private bullet objectToPool;
        [SerializeField] private int amountToPool;

        private Weapon playerWeapon;

        //_pool = new ObjectPool<GameObject>(createFunc: () => new GameObject("PooledObject"), actionOnGet: (obj) => obj.SetActive(true), actionOnRelease: (obj) => obj.SetActive(false), actionOnDestroy: (obj) => Destroy(obj), collectionChecks: false, defaultCapacity: 10, maxPoolSize: 10);

        public enum WEAPON_TYPE
        {
            PISTOL,
            SHOTGUN,
            SNIPER
        }

        void Awake()
        => SharedInstance = this;

        void Start()
        {
            pooledObjects = new List<bullet>();
            bullet tmp;
            for (int i = 0; i < amountToPool; i++)
            {
                tmp = Instantiate(objectToPool);
                tmp.gameObject.SetActive(false);
                pooledObjects.Add(tmp);
            }

        }

        private void OnDestroy()
        {
            playerWeapon.OnShoot -= ShowPooledObject;
        }

        public void SetWeapon(Weapon weapon)
        {
            playerWeapon = weapon;
            playerWeapon.OnShoot += ShowPooledObject;
        }

        public bullet GetPooledObject(int magazine)
        {
            for(int i = 0; i < magazine; i++)
            {
                if (!pooledObjects[i].gameObject.activeInHierarchy)
                    return pooledObjects[i];
            }
            return null;
            
        }

        public List<bullet> GetMultiplePooledObject(int magazine, int nbToAdd)
        {
            List<bullet> bullets = new List<bullet>();
            for(int j = 0; j < magazine; j++)
            {
                for (int i = 0; i < nbToAdd; i++)
                {
                    if (!pooledObjects[i].gameObject.activeInHierarchy)
                        bullets.Add(pooledObjects[i]);
                }
                return bullets;
            }

            return null;
        }

        public void ShowPooledObject(object sender, Weapon.OnShootEvent e)
        {
            if(e.weaponType == WEAPON_TYPE.SHOTGUN)
            {
                List<bullet> bullets = GetMultiplePooledObject(e.magazine, 3);

                if (bullets != null)
                {
                    Vector3 dir = e.shootDirection - new Vector3(2f,2f,0);
                    foreach (bullet bulletToShoot in bullets)
                    {
                        bulletToShoot.SetDirectionAndDmg(dir, e.dmg);
                        dir += new Vector3(2f, 2f, 0);
                        // Set  position
                        bulletToShoot.transform.position = e.spawnPoint.transform.position;
                        // Set rotation
                        bulletToShoot.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                bullet bullet = GetPooledObject(e.magazine);

                if (bullet != null)
                {
                    bullet.SetDirectionAndDmg(e.shootDirection, e.dmg);
                    // Set  position
                    bullet.transform.position = e.spawnPoint.transform.position;
                    // Set rotation
                    bullet.gameObject.SetActive(true);
                }
            }     
        }

        /*public void Reload()
        {
            amountShooted = 0;
            GameObject tmp;

            for (int i = 0; i < amountToPool; i++)
            {
                tmp = Instantiate(objectToPool);
                tmp.SetActive(false);
                pooledObjects.Add(tmp);
            }
        }*/

    }
}
