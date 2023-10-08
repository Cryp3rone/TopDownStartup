using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityAttack : MonoBehaviour
{
    [SerializeField] AttackZone _attackZone;

    private float _cooldown = 1f;
    private float _currentCooldown;
    private bool _canAttack = true;
    public event UnityAction OnAttack;

    public void LaunchAttack()
    {
        OnAttack?.Invoke();
        foreach (var el in _attackZone.InZone)
        {
            if (_canAttack)
            {
                el.Damage(this.gameObject, 10);
                _canAttack = false;
                _currentCooldown = 0;
            }
        }
    }

    private void Update()
    {
        if (!_canAttack)
        {
            _currentCooldown += Time.deltaTime;

            if (_currentCooldown >= _cooldown)
            {
                _canAttack = true;
            }
        }
    }
}
