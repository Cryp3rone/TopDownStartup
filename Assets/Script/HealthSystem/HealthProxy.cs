using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthProxy : MonoBehaviour, IHealth
{
    [SerializeField, Required] Health _target;

    public Health Target { get { return _target; } set { _target = value; } }
    public void Damage(int amount) => _target.Damage(amount);
    public void Kill() => _target.Kill();
    public void Regen(int amount) => _target.Regen(amount);

}
