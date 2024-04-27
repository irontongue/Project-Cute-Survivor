using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WavePacket
{
    [Tooltip("In Seconds")]public float duration;
    public WaveEnemyPacket[] enemyTypes;
    public bool increaseMusicIntensity = false;
    public bool decreaseMusicIntensity = false;
}
[System.Serializable]
public class WaveEnemyPacket
{
    public EnemyType enemyType;
    public int amount = 1;
    public float angleStart = 0;
    public float angleEnd = 90;
    public float rotateAmount = 0;
    public float delayBetweenSpawns = 0;
    public float minSpacing = 1;
    [HideInInspector] public bool spawned;
}

public class WaveManager : MonoBehaviour
{
    public bool dev_DontSpawnWave;
    [SerializeField] GameObject[] enemyPrefabs;
    public Dictionary<EnemyType, int> activeEnemies = new Dictionary<EnemyType, int>();
    public Dictionary<EnemyType, int> totalEnemiesToSpawn = new Dictionary<EnemyType, int>();
    public Dictionary<EnemyType, GameObject> enemyPrefabDictionary = new Dictionary<EnemyType, GameObject>();


    [SerializeField] WavePacket[] wavesInfo;
    
    public float spawnRadius = 15f;
    [SerializeField] int currentWaveIndex = 0;
    [SerializeField] Transform tempGameObjectFolder;

    GameManager gameManager;
    AIManager aiManager;
    WeaponStats weaponStats;
    float unitLegnth; //The length of one unit(meter) in degrees;
    bool waveTimerFinished = false;
    Transform player;


    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        gameManager = FindObjectOfType<GameManager>();
        aiManager = FindObjectOfType<AIManager>();
        weaponStats = FindObjectOfType<WeaponStats>();
        AssignDictionaries();

        unitLegnth = 1 / (spawnRadius * Mathf.Deg2Rad);
        if(!dev_DontSpawnWave)
        StartCoroutine(WaveLoop());
    }

    IEnumerator WaveLoop(bool lastWave = false)
    {
        AssignTotalEnemiesToSpawn();
        int timesRepeated = 0;
        while(gameManager.isPaused)
            yield return null;

        waveTimerFinished = false;
        StartCoroutine(WaveTimer());
        if (wavesInfo[currentWaveIndex].increaseMusicIntensity)
        {
            gameManager.IncreaseMusicIntensity(1);
        }
        else if (wavesInfo[currentWaveIndex].decreaseMusicIntensity)
        {
            gameManager.IncreaseMusicIntensity(-1);
        }
        while(waveTimerFinished == false)
        {

            foreach(WaveEnemyPacket packet in wavesInfo[currentWaveIndex].enemyTypes) 
            {
                if((int)packet.enemyType > 5)
                {
                    if(packet.spawned)
                    {
                        continue;
                    }
                    else
                    {
                        packet.spawned = true;
                    }
                }
                while (gameManager.isPaused)
                {
                    yield return null;
                }
                int enemiesToSpawn = totalEnemiesToSpawn[packet.enemyType] - activeEnemies[packet.enemyType];
                enemiesToSpawn = Mathf.Clamp(enemiesToSpawn, 0, packet.amount);
    
                //print("Enemies To Spawn: " + enemiesToSpawn);
                if (enemiesToSpawn > 0)
                {
                    yield return SpawnWave(packet, enemiesToSpawn, timesRepeated);
                }
            }
            if (lastWave)
                waveTimerFinished = true;
            timesRepeated++;
            float secondTimer = 0;
            while(secondTimer < 1)
            {
                yield return null;
                secondTimer += Time.deltaTime;
            }
        }
        if (currentWaveIndex != wavesInfo.Length -1)
        {
            currentWaveIndex++;
            StartCoroutine(WaveLoop());
        }
        else
        {
            gameManager.WinEvent();
        }
    }
    IEnumerator WaveTimer()
    {
        float timer = 0;
        float duration = wavesInfo[currentWaveIndex].duration;// * 60;
        while(timer < duration)
        {
            while(gameManager.isPaused)
            { yield return null; } 

            timer += Time.deltaTime;
            yield return null;
        }
        waveTimerFinished = true;
        
    }
    IEnumerator SpawnWave(WaveEnemyPacket waveEnemyPacket, int spawnCount, int timesRepeated = 0) 
    {

        int originalAmountToSpawn = spawnCount;
        float degPerUnit = 360f / (2f * Mathf.PI * spawnRadius) * (Mathf.PI / 180f); //Finding the length of each degree of rotation along the edge of the circle
        float length = (waveEnemyPacket.angleEnd - waveEnemyPacket.angleStart) * (2f * Mathf.PI * spawnRadius) / 360f; //The Length between each angle in units
        int amountToSpawn = originalAmountToSpawn;
        float spacing = Mathf.Clamp(length / (amountToSpawn - 1), waveEnemyPacket.minSpacing, Mathf.Infinity);
        int loops = (int)Mathf.Ceil(amountToSpawn / length); //How many times it should loop (if there isnt enough space along the edge, it will loop and move 1 further away)
        bool breakOutOfLoop = false;//Used to break out of the first for loop from the inner
        bool spawnDelay = waveEnemyPacket.delayBetweenSpawns != 0;

        for (int i = 0; i < loops; i++)
        {
            for (int i_ = 0; i_ < Mathf.Floor(length); i_++)
            {
                if (amountToSpawn == 0)
                {
                    breakOutOfLoop = true;
                    break;
                }
                amountToSpawn--;
                float angle;
                if (originalAmountToSpawn == 1)
                {
                    angle = (Mathf.Deg2Rad * (waveEnemyPacket.angleStart + timesRepeated * waveEnemyPacket.rotateAmount));
                }
                else
                {
                    angle = (Mathf.Deg2Rad * (waveEnemyPacket.angleStart + timesRepeated * waveEnemyPacket.rotateAmount)) + (degPerUnit * spacing) * i_; // The angle at which the new AI will spawn at
                }
                Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)).normalized; //Converted the angle into a dir vector
                int enemyID = aiManager.enemyPools[waveEnemyPacket.enemyType].Count;
                if (enemyID > 0)
                {
                    EnemyInfo info =  aiManager.enemyPools[waveEnemyPacket.enemyType][0];
                    aiManager.AddToActiveEnemies(info.enemyType);
                    info.trans.position = dir * (spawnRadius + i) + (Vector2)player.position;
                    info.ResetVariables();
                    info.gameObject.SetActive(true);
                    
                }
                else
                {
                    EnemyInfo info = Instantiate(enemyPrefabDictionary[waveEnemyPacket.enemyType], dir * (spawnRadius + i) + (Vector2)player.position, transform.rotation, tempGameObjectFolder).GetComponent<EnemyInfo>();
                    AssignAIInfo(info);
                    aiManager.AddToActiveEnemies(info.enemyType, info);

                }
                    yield return null;
            }
            if (breakOutOfLoop)
            {
                break;
            }
            yield return null;
        }
        yield return null;

    }
    void AssignTotalEnemiesToSpawn()
    {
        foreach(WaveEnemyPacket packet in wavesInfo[currentWaveIndex].enemyTypes)
        {
            totalEnemiesToSpawn[packet.enemyType] += packet.amount;
        }
    }

    void AssignAIInfo(EnemyInfo info)
    {
        info.active = true;
        info.rb = info.GetComponent<Rigidbody2D>();
        info.gObject = info.gameObject;
        info.trans = info.transform;
        info.manager = aiManager;
        info.player = player; 
        info.spriteRenderer = info.GetComponent<SpriteRenderer>();
        info.gameManger = gameManager;
        info.fire_PE = info.GetComponentInChildren<ParticleSystem>();
        info.currentFrame = Random.Range(0, info.frames.Length - 1);
        info.frameTemp = info.frames;
    }
    void AssignDictionaries()
    {
        foreach (var enemy in enemyPrefabs)
        {
            EnemyInfo info = enemy.GetComponent<EnemyInfo>();
            enemyPrefabDictionary.Add(info.enemyType, enemy);
            activeEnemies.Add(info.enemyType, 0);
            totalEnemiesToSpawn.Add(info.enemyType, 0);
            aiManager.enemyPools.Add(info.enemyType, new List<EnemyInfo>());
        }
    }
}
