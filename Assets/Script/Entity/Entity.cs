using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField, Required("nop")] Health _health;
}

public enum EntityType
{
    Player,
    EnemyMelee,
    EnemyDistance,
    EnemyBoss,
}
