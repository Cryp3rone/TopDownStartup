using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game.PlayerMunition;

namespace Game
{
    public class Weapon : MonoBehaviour
    {
        public event EventHandler<OnShootEvent> OnShoot;
        public class OnShootEvent : EventArgs
        {
            public Vector3 shootDirection;
            public GameObject spawnPoint;
            public WEAPON_TYPE weaponType;
            public int dmg;
            public int magazine;
        }

        [SerializeField] private WEAPON_TYPE weaponType;
        [SerializeField] private Transform aimTransform;
        [SerializeField] private GameObject spawnPoint;
        [SerializeField] private int _dmg;
        [SerializeField] private int _magazine;

        private PlayerMunition munition;

        private void Start()
        {
            munition = PlayerMunition.SharedInstance;

            munition.SetWeapon(this);
        }

        // Update is called once per frame
        void Update()
        {
            HandleAiming();
            HandleShooting();
        }

        private void HandleAiming()
        {
            Vector3 mousePosition = GetMousePosition();

            Vector3 aimDirection = (mousePosition - transform.position).normalized;
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

            aimTransform.eulerAngles = new Vector3(0, 0, angle);
        }

        private static Vector3 GetMousePosition()
        {
            Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            vec.z = 0f;
            return vec;
        }

        private void HandleShooting()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnShoot?.Invoke(this, new OnShootEvent
                {
                    shootDirection = GetMousePosition(),
                    spawnPoint = spawnPoint,
                    weaponType = weaponType,
                    dmg = _dmg,
                    magazine = _magazine,
                });
            }
        }

    }
}
