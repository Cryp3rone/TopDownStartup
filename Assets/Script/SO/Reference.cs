using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ISet<T>
{
    void Set(T value);
}

interface IRemove<T>
{
    void Remove(T value);
}

public class Reference<T> : ScriptableObject, ISet<T>
{
    T _instance;

    public T Instance { get => _instance;}

    public event Action<T> OnValueChanged;

    void ISet<T>.Set(T value)
    {
        _instance = value;
        OnValueChanged?.Invoke(_instance);
    }
}

public class MultipleReference<T> : ScriptableObject, ISet<T>, IRemove<T>
{
    public List<T> _instances;

    public List<T> Instances { get => _instances;}

    //public event Action<T> OnValueChanged;

    void ISet<T>.Set(T value)
    {
        _instances.Add(value);
    }
    
    void IRemove<T>.Remove(T value)
    {
        _instances.Remove(value);
    }

    void Clear()
    {
        _instances.Clear();
    }
}
