using System;
using UnityEngine;

[Serializable]
public class StatOffenseGroup 
{
    public Stat attackSpeed;

    // Physical
    public Stat damage;
    public Stat critPower;
    public Stat critChance;
    public Stat armorReduction;

    // Elemental Damage
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;
}
