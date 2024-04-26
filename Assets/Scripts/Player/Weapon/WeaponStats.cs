using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weapons {W_Turret, W_FlameThrower, W_RocketLauncher, W_RodFromGod, W_Lazer, W_Buzzsaw}

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
    public float radius = 1;
    public int projectileBounces = 0;
    public Vector2 areaSize = Vector2.zero;
    //
    [HideInInspector]public float baseAttackSpeed = 1;
    [HideInInspector]public float baseAttackRange = 1;
    [HideInInspector]public float baseProjectileSpeed = 1;
    [HideInInspector]public float baseProjectileLifetime = 1;
    [HideInInspector]public float baseProjectileSize = 1;
    [HideInInspector]public float baseDuration = 1;
    [HideInInspector]public float baseRadius = 1;
    [HideInInspector]public int baseProjectileBounces = 0;
    [HideInInspector]public Vector2 baseAreaSize = Vector2.zero;
}

public class WeaponStats : MonoBehaviour
{
    public Weapon[] weapons;
    [Header("Other Stats")]
    public int fireDamagePerTick = 1;
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
            w.baseRadius = w.radius;       
            w.baseAreaSize = w.areaSize;
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
