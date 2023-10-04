using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Player_UI : MonoBehaviour
    {
        [SerializeField] PlayerReference playerRef;
        [SerializeField] Scrollbar scrollbarHealh;
        [SerializeField] Scrollbar scrollbarPower;
        private Health _playerHealth;
        
        void Awake()
        {
            _playerHealth = playerRef.Instance.GetComponent<Health>();
            _playerHealth.OnValueChangedCurrentHealth += UpdateHealthUI;
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
    }
}
