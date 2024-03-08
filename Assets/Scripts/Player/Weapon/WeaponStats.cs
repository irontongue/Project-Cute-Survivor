using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon
{
    public string name;
    public int damage;
    public float attackSpeed;
    public float attackRange;
    public float projectileSpeed;
    public float projectileLifetime;
}

public class WeaponStats : MonoBehaviour
{
    public Weapon[] Weapons;
    public Weapon GetWeapon(string weaponName)
    {
        foreach (Weapon weapon in Weapons) 
        {
            if (weapon.name == weaponName) 
                return weapon; 
        }
        return null;
    }
}
