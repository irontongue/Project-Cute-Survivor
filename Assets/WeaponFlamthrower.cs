using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponFlamthrower : WeaponBase
{
    ParticleSystem _particleSystem;
    override protected void Start()
    {
       base.Start();
       _particleSystem = GetComponentInChildren<ParticleSystem>();
        StartCoroutine(MainLoop());
    }

    Vector2 areaSize;
    Vector2 center;
    float timer = 0;
    IEnumerator WeaponTimer()
    {
        while(timer < weaponStats.duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator MainLoop()
    {
        _particleSystem.Stop();
        var mainMod = _particleSystem.main;
        mainMod.duration = weaponStats.duration;
        _particleSystem.Play();
        StartCoroutine(WeaponTimer());
        while(timer < weaponStats.duration)
        {
            Fire();
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(weaponStats.attackSpeed);
        timer = 0;
        StartCoroutine(MainLoop());
    }
    void Fire()
    {
        areaSize = weaponStats.areaSize;
        center = muzzlePosition.position + transform.up * (areaSize.x * 0.5f);
        Debug.DrawLine(muzzlePosition.position, center);
        float angle = transform.root.rotation.eulerAngles.z - 90f;
        EnemyInfo[] infos = GetEnemiesInArea(center, areaSize, angle);
        foreach (EnemyInfo info in infos)
        {
            if(info == null) continue;
            if(!info.onFire)
                info.SetOnFire();
            info.DealDamage(weaponStats.damage);
        } 
    }
    //private void OnDrawGizmos()
    //{
    //    Vector3 angle = transform.rotation.eulerAngles;
    //    angle.z -= 90;
    //    Quaternion rot = Quaternion.Euler(angle);
    //    Gizmos.matrix = Matrix4x4.TRS(center, rot, areaSize);
    //    // Then use it one a default cube which is not translated nor scaled
    //    Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    //}
}
