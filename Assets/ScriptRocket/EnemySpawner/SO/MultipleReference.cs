using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleReference<T> : ScriptableObject, ISet<T>, IRemove<T>
{
    private List<T> _instances;

    public List<T> Instances { get => _instances;}

    public event Action<int> OnValueChanged;

    void ISet<T>.Set(T value)
    {
        _instances.Add(value);
        OnValueChanged?.Invoke(Instances.Count);
    }

    void IRemove<T>.Remove(T value)
    {
        _instances.Remove(value);
        OnValueChanged?.Invoke(Instances.Count);
    }

    protected void ClearList()
    {
        _instances.Clear();
    }
}

