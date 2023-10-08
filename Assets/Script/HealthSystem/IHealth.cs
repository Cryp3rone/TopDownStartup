using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth 
{
    void Damage(GameObject source, int amount);
    void Regen(int amount);
    void Kill();

}
