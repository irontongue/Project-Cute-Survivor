using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyBehaviour {Chase, Ranged}
public enum EnemyType {Slime, FrogCar}
public class AIManager : MonoBehaviour
{
    GameObject player;
    WaveManager waveManager;
    WeaponStats weaponStats;
    // *** Enemy Pooling ***
    public Dictionary<EnemyType, List<EnemyInfo>> enemyPools = new Dictionary<EnemyType, List<EnemyInfo>>();
    // *** End Pooling ***
    public List<EnemyInfo> activeEnemies = new List<EnemyInfo>();


    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        waveManager = FindObjectOfType<WaveManager>();
        weaponStats = FindObjectOfType<WeaponStats>();
    }

    // Update is called once per frame
    public void AddToEnemyPools(EnemyInfo enemyInfo)
    {
        enemyPools[enemyInfo.enemyType].Add(enemyInfo);
    }

    public void AddToActiveEnemies(EnemyType enemyType, EnemyInfo info = null)
    {
        if(info == null)
        {
            info = enemyPools[enemyType][0];
            enemyPools[enemyType].RemoveAt(0);
        }
        activeEnemies.Add(info);
        waveManager.activeEnemies[enemyType]++;
    }
    public void RemoveActiveEnemy(EnemyInfo enemyInfo)
    {
        activeEnemies.Remove(enemyInfo);
        enemyPools[enemyInfo.enemyType].Add(enemyInfo);

        waveManager.activeEnemies[enemyInfo.enemyType]--;

    }
    float dpsTimer = 0;
    private void Update()
    {
        if(dpsTimer < 1)
        {
            dpsTimer += Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        Vector2 playerPos = player.transform.position;
        if(activeEnemies.Count > 0)
        {
            foreach (EnemyInfo enemy in activeEnemies)
            {
                if(enemy.active)
                {
                    enemy.AILoop(enemy.enemyBehaviour, playerPos);
                    enemy.Animate();
                    if (dpsTimer < 1)
                        continue;
                    if (enemy.onFire)
                        enemy.DamagePerSecond(weaponStats.fireDamagePerTick);
                }
            }
            if(dpsTimer >= 1)
            {
                dpsTimer = 0;
            }
        }
    }

}
