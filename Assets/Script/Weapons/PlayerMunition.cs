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

        private Weapon playerWeapon = null;
        private int amountShooted;
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
            pooledObjects = new List<bullet>();
            bullet tmp;
            for (int i = 0; i < amountToPool; i++)
            {
                tmp = Instantiate(objectToPool);
                tmp.gameObject.SetActive(false);
                pooledObjects.Add(tmp);
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

        private bullet GetPooledObject(int magazine)
        {
            for(int i = 0; i < magazine; i++)
            {
                if (!pooledObjects[i].gameObject.activeInHierarchy)
                {
                    amountShooted += 1;
                    return pooledObjects[i];
                }
            }
            return null;
        }

        private List<bullet> GetMultiplePooledObject(int magazine, int nbToAdd)
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

        private void PistolShoot(object sender, Weapon.OnShootEvent e)
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
