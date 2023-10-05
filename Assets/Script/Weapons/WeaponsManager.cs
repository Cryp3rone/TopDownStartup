using Codice.Utils;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class WeaponsManager : MonoBehaviour
    {

        [SerializeField] private GameObject pistol;
        [SerializeField] private GameObject shotgun;
        [SerializeField] private GameObject sniper;

        private GameObject currentWeapon;

        // Start is called before the first frame update
        void Start()
        {
            currentWeapon = pistol;
            currentWeapon.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        [Button]
        private void ChangeToPistol()
        {
            if (currentWeapon == pistol)
                return;

            currentWeapon.SetActive(false);

            currentWeapon = pistol;
            currentWeapon.SetActive(true);
        }

        [Button]
        private void ChangeToShotgun()
        {
            if (currentWeapon == shotgun)
                return;

            currentWeapon.SetActive(false);

            currentWeapon = shotgun;
            currentWeapon.SetActive(true);
        }

        [Button]
        private void ChangeToSniper()
        {
            if (currentWeapon == sniper)
                return;

            currentWeapon.SetActive(false);

            currentWeapon = sniper;
            currentWeapon.SetActive(true);
        }
    }
}
