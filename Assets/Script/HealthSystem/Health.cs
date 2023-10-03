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
    public event Action OnDie;

    public void Damage(int amount)
    {
        Assert.IsTrue(amount >= 0);
        if (IsDead) return;

        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
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
        OnRegen?.Invoke(CurrentHealth-old);
    }
    void InternalDie()
    {
        if (!IsDead) return;
        OnDie?.Invoke();
    }
}
