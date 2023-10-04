using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

public class Health : MonoBehaviour, IHealth
{
    [SerializeField] int _maxHealth;
    private EntityType _entityType;

    /// <summary>
    /// coucou
    /// </summary>
    public int CurrentHealth 
    {
        get;
        private set;
    }
    public bool IsDead => CurrentHealth > 0;
    public int MaxHealth { get => _maxHealth; }

    public event Action<int> OnDamage;
    public event Action<int> OnRegen;
    public event Action<GameObject, EntityType> OnDie;

    public event Action<int> OnValueChangedCurrentHealth;

    public void Damage(int amount)
    {
        Assert.IsTrue(amount >= 0);
        if (IsDead) return;

        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        OnValueChangedCurrentHealth?.Invoke(CurrentHealth);
        OnDamage?.Invoke(amount);
    }
    public void Regen(int amount)
    {
        Assert.IsTrue(amount >= 0);
        if (IsDead) return;
        InternalRegen(amount);
    }
    public void Kill()
    {
        if (IsDead) return;
        InternalDie();
    }

    public void Revive(int amount)
    {
        Assert.IsTrue(amount >= 0);
        if (!IsDead) return;
        InternalRegen(amount);
    }

    void InternalRegen(int amount)
    {
        Assert.IsTrue(amount >= 0);

        var old = CurrentHealth;
        CurrentHealth = Mathf.Min(_maxHealth, CurrentHealth + amount);
        OnValueChangedCurrentHealth?.Invoke(CurrentHealth);
        OnRegen?.Invoke(CurrentHealth-old);
    }
    void InternalDie()
    {
        if (!IsDead) return;
        OnDie?.Invoke(this.gameObject, _entityType);
    }

    [Button]
    public void Die()
    {
        OnDie?.Invoke(this.gameObject, _entityType);
    }

    public void SetEntityType(EntityType type)
    {
        _entityType = type;
    }
}
