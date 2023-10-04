using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Game
{
    public class Player_UI : MonoBehaviour
    {
        [SerializeField] PlayerReference playerRef;
        [SerializeField] SpawnerReference spawnerRef;
        [SerializeField] Scrollbar scrollbarHealh;
        [SerializeField] Scrollbar scrollbarPower;

        [SerializeField] TMP_Text textUI;
        private Health _playerHealth;
        private SpawnerManager _spawnerManager;
        
        void Awake()
        {
            _playerHealth = playerRef.Instance.GetComponent<Health>();
            _playerHealth.OnValueChangedCurrentHealth += UpdateHealthUI;
            _spawnerManager = spawnerRef.Instance;
        }

        void UpdateHealthUI(int value)
        {
            float clampValue = Mathf.Clamp(value, 0, _playerHealth.MaxHealth);
            float sliderValue = clampValue /_playerHealth.MaxHealth;
            scrollbarHealh.size = sliderValue;
        }

        void UpdatePowerUI(int value)
        {
            
        }

        void UpdateWaveText()
        {
            
        }

        void UpdateWinText()
        {
            
        }
    }
}
