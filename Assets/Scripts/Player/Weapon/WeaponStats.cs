using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weapons {Turret}

[System.Serializable]
public class Weapon
{
    public Weapons weapon;
    public int damage;
    [Tooltip("Times Attacked Per Second")]
    public float attackSpeed;
    public float attackRange;
    public float projectileSpeed;
    public float projectileLifetime;
}

public class WeaponStats : MonoBehaviour
{
    public Weapon[] Weapons;
    public Weapon GetWeapon(Weapons weaponName)
    {
        foreach (Weapon weapon in Weapons) 
        {
            if (weapon.weapon == weaponName) 
                return weapon; 
        }
        return null;
    }
}
