using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weapons {W_Turret, W_FlameThrower, W_RocketLauncher}

[System.Serializable]
public class Weapon
{
    public Weapons weapon;
    public int damage;
    [Tooltip("Times Attacked Per Second")]
    public float attackSpeed = 1;
    public float attackRange = 1;
    public float projectileSpeed = 1;
    public float projectileLifetime =1 ;
    public float projectileSize = 1;
    public float duration = 1;
    //
    [HideInInspector]public float baseAttackSpeed = 1;
    [HideInInspector]public float baseAttackRange = 1;
    [HideInInspector]public float baseProjectileSpeed = 1;
    [HideInInspector]public float baseProjectileLifetime = 1;
    [HideInInspector]public float baseProjectileSize = 1;
    [HideInInspector]public float baseDuration = 1;
}

public class WeaponStats : MonoBehaviour
{
    public Weapon[] weapons;
    private void Start()
    {
        foreach(Weapon w in weapons)
        {
            w.baseAttackSpeed = w.attackSpeed;
            w.baseAttackRange = w.attackRange;
            w.baseProjectileSpeed = w.projectileSpeed;
            w.baseProjectileLifetime = w.projectileLifetime;
            w.baseProjectileSize = w.projectileSize;
            w.baseDuration = w.duration;
        }
    }
    public Weapon GetWeapon(Weapons weaponName)
    {
        foreach (Weapon weapon in weapons) 
        {
            if (weapon.weapon == weaponName) 
                return weapon; 
        }
        return null;
    }

}
