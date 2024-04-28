using System.Collections;
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
            if (gameManager.isPaused)
                yield return null;
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
        if(audioSource)
        {
            StartCoroutine(StopFireAudio());
            PlayAudio();
        }
        while(timer < weaponStats.duration)
        {
            if (gameManager.isPaused)
                yield return null;
            Fire();
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1 / weaponStats.attackSpeed);
        while (gameManager.isPaused)
            yield return null;
        timer = 0;
        StartCoroutine(MainLoop());
    }
    IEnumerator StopFireAudio()
    {
        yield return new WaitForSeconds(weaponStats.duration);
        audioSource.Stop();
    }
    void Fire()
    {
        areaSize = weaponStats.areaSize;
        center = muzzlePosition.position + transform.right * (areaSize.x * 0.5f);
        float angle = transform.root.rotation.eulerAngles.z + muzzlePosition.transform.rotation.z;
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
