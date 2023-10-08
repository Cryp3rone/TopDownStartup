using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayerMunition : MonoBehaviour
    {
        public static PlayerMunition SharedInstance;

        [SerializeField] private List<bullet> pooledInactiveObjects;
        [SerializeField] private List<bullet> pooledActiveObjects;

        [SerializeField] private bullet objectToPool;
        [SerializeField] private int amountToPool;

        private Weapon playerWeapon = null;
        private int amountShooted = 0;
        private bool onReload = false;

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
            pooledInactiveObjects = new List<bullet>();
            bullet tmp;
            for (int i = 0; i < amountToPool; i++)
            {
                tmp = Instantiate(objectToPool);
                tmp.gameObject.SetActive(false);
                pooledInactiveObjects.Add(tmp);
            }
        }

        public void SetWeapon(Weapon weapon, WEAPON_TYPE type)
        {
            playerWeapon = weapon;

            switch (type)
            {
                case WEAPON_TYPE.PISTOL:
                    playerWeapon.OnShoot += PistolShoot;
                    break;
                case WEAPON_TYPE.SHOTGUN:
                    playerWeapon.OnShoot += ShotgunShoot;
                    break;
                case WEAPON_TYPE.SNIPER:
                    playerWeapon.OnShoot += SniperShoot;
                    break;
            }
        }

        public void AddBulletBackToInactive(bullet bullet)
        {
            pooledInactiveObjects.Add(bullet);
            pooledActiveObjects.Remove(bullet);
        }

        private bullet GetPooledObject(int magazine)
        {
            bullet bulletToReturn;

            for(int i = 0; i < magazine; i++)
            {
                if (pooledInactiveObjects[i] != null)
                {
                    bulletToReturn = pooledInactiveObjects[i];
                    amountShooted += 1;
                    pooledActiveObjects.Add(pooledInactiveObjects[i]);
                    pooledInactiveObjects.Remove(pooledInactiveObjects[i]);
                    return bulletToReturn;
                }
            }
            return null;
        }

        private List<bullet> GetMultiplePooledObject(int magazine, int nbToAdd)
        {
            List<bullet> bullets = new List<bullet>();
            
                for (int i = 0; i < nbToAdd; i++)
                {
                    if (pooledInactiveObjects[i] != null)
                    {
                        bullets.Add(pooledInactiveObjects[i]);
                        pooledActiveObjects.Add(pooledInactiveObjects[i]);
                        pooledInactiveObjects.Remove(pooledInactiveObjects[i]);
                    }
                }
                return bullets;
        }

        private void PistolShoot(object sender, Weapon.OnShootEvent e)
        {
            if (onReload)
                return;

            //Debug.Log(amountShooted + "  " + e.magazine);
            bullet bullet = GetPooledObject(e.magazine);

            if (bullet != null)
            {
                bullet.SetDirectionAndDmg(e.shootDirection.normalized, e.dmg);
                // Set  position
                bullet.transform.position = e.spawnPoint.transform.position;
                // Set rotation
                bullet.gameObject.SetActive(true);


                if(amountShooted == e.magazine)
                    StartCoroutine(Reload(1));
            }
        }

        private void ShotgunShoot(object sender, Weapon.OnShootEvent e)
        {
            if (onReload)
                return;

            List<bullet> bullets = GetMultiplePooledObject(e.magazine, 3);

            if (bullets != null)
            {
                Vector3 dir = e.shootDirection - new Vector3(2f, 2f, 0).normalized;
                foreach (bullet bulletToShoot in bullets)
                {
                    bulletToShoot.SetDirectionAndDmg(dir, e.dmg);
                    dir += new Vector3(2f, 2f, 0).normalized;
                    // Set  position
                    bulletToShoot.transform.position = e.spawnPoint.transform.position;
                    // Set rotation
                    bulletToShoot.gameObject.SetActive(true);
                }
                StartCoroutine(Reload(1));
            }
            
        }

        private void SniperShoot(object sender, Weapon.OnShootEvent e)
        {
            if (onReload)
                return;

            bullet bullet = GetPooledObject(e.magazine);

            if (bullet != null)
            {
                bullet.SetDirectionAndDmg(e.shootDirection.normalized, e.dmg);
                // Set  position
                bullet.transform.position = e.spawnPoint.transform.position;
                // Set rotation
                bullet.gameObject.SetActive(true);
                StartCoroutine(Reload(2));
            }  
        }

        private IEnumerator Reload(int time)
        {
            onReload = true;

            yield return new WaitForSeconds(time);
            amountShooted = 0;
            onReload = false;
        }

    }
}
