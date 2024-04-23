using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyBehaviour {Chase, Ranged, Explode, Dash, Boss}
public enum EnemyType {Slime, FishLegs, FrogCar, CarrotDude, CaviarPlant, MushroomSquid, ElephantFairy, ChickenBunny, BananaBoy, extra1, extra2, extra3, extra4, extra5}
public class AIManager : MonoBehaviour
{
    GameObject playerGameObject;
    GameManager gameManager;
    WaveManager waveManager;
    WeaponStats weaponStats;
    // *** Enemy Pooling ***
    public Dictionary<EnemyType, List<EnemyInfo>> enemyPools = new Dictionary<EnemyType, List<EnemyInfo>>();
    // *** End Pooling ***
    public List<EnemyInfo> activeEnemies = new List<EnemyInfo>();
    public List<GameObject> projectilePool;
    public GameObject tempGameobjectFolder;


    // Start is called before the first frame update
    void Start()
    {
        GetReferences();
        gameManager.pauseEvent += PauseEnemies;
        gameManager.playEvent += ResumeEnemies;
    }
    void GetReferences()
    {
        playerGameObject = FindObjectOfType<PlayerController>().gameObject;
        waveManager = FindObjectOfType<WaveManager>();
        weaponStats = FindObjectOfType<WeaponStats>();
        gameManager = FindObjectOfType<GameManager>();
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
        if(gameManager.isPaused)
        {
            return;
        }
        Vector2 playerPos = playerGameObject.transform.position;
        if(activeEnemies.Count > 0)
        {
            foreach (EnemyInfo enemy in activeEnemies)
            {
                if(enemy.active)
                {
                    enemy.AILoop(playerPos);
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
    void PauseEnemies()
    {
        if (activeEnemies.Count > 0)
        {
            foreach (EnemyInfo enemy in activeEnemies)
            {
                if (enemy.active)
                {
                    enemy.rb.simulated = false;
                }
            }
        }
    }

    void ResumeEnemies()
    {
        if (activeEnemies.Count > 0)
        {
            foreach (EnemyInfo enemy in activeEnemies)
            {
                if (enemy.active)
                {
                    enemy.rb.simulated = true;
                }
            }
        }
    }

}
