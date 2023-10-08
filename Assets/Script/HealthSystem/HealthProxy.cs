using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthProxy : MonoBehaviour, IHealth
{
    [SerializeField, Required] Health _target;

    public Health Target { get { return _target; } set { _target = value; } }
    public void Damage(GameObject source, int amount) => _target.Damage(source,amount);
    public void Kill() => _target.Kill();
    public void Regen(int amount) => _target.Regen(amount);

}
