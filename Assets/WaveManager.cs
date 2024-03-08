using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[System.Serializable]
public class WavePacket
{
    [Tooltip("In Minutes")]public float duration;
    public WaveEnemyPacket[] enemyTypes;
}
[System.Serializable]
public class WaveEnemyPacket
{
    public EnemyType enemyType;
    public int amount = 1;
    public float angleStart = 0;
    public float angleEnd = 90;
    public float rotateAmount = 0;
}

public class WaveManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;
    public Dictionary<EnemyType, int> activeEnemies = new Dictionary<EnemyType, int>();
    public Dictionary<EnemyType, GameObject> enemyPrefabDictionary = new Dictionary<EnemyType, GameObject>();


    [SerializeField] WavePacket[] wavesInfo;
    
    public float spawnRadius = 15f;
    [SerializeField] int currentWaveIndex = 0;

    GameManager gameManager;
    AIManager aiManager;
    float unitLegnth; //The length of one unit(meter) in degrees;
    bool waveTimerFinished = false;
    Transform player;


    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        gameManager = FindObjectOfType<GameManager>();
        aiManager = FindObjectOfType<AIManager>();
        AssignDictionaries();

        unitLegnth = 1 / (spawnRadius * Mathf.Deg2Rad);

        StartCoroutine(WaveLoop());
    }

    IEnumerator WaveLoop(int timesRepeated = 0)
    {
        while(gameManager.isPaused)
            yield return null;

        //make new wave packets depending on how many enemeis are killed.
        //spawn a wave every 5 seconds 
        waveTimerFinished = false;
        StartCoroutine(WaveTimer());
        print(waveTimerFinished);
        while(waveTimerFinished == false)
        {
            print("in loop");
            foreach(WaveEnemyPacket packet in wavesInfo[currentWaveIndex].enemyTypes) 
            {
                while (gameManager.isPaused)
                {
                    yield return null;
                }
                int enemiesToSpawn = packet.amount - activeEnemies[packet.enemyType];
                print("Enemies To Spawn: " + enemiesToSpawn);
                if (enemiesToSpawn > 0)
                {
                    yield return SpawnWave(packet, enemiesToSpawn, timesRepeated);
                }
            }
            timesRepeated++;
            float secondTimer = 0;
            while(secondTimer < 1)
            {
                yield return new WaitForEndOfFrame();
                secondTimer += Time.deltaTime;
            }
        }
    }
    IEnumerator WaveTimer()
    {
        float timer = 0;
        float duration = wavesInfo[currentWaveIndex].duration / 60;
        print("duration: " + duration);
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
        float spacing = Mathf.Clamp(length / (amountToSpawn - 1), 1, Mathf.Infinity);
        int loops = (int)Mathf.Ceil(amountToSpawn / length); //How many times it should loop (if there isnt enough space along the edge, it will loop and move 1 further away)
        bool breakOutOfLoop = false;//Used to break out of the first for loop from the inner

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
                yield return null;
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
                    EnemyInfo info = Instantiate(enemyPrefabDictionary[waveEnemyPacket.enemyType], dir * (spawnRadius + i) + (Vector2)player.position, transform.rotation).GetComponent<EnemyInfo>();
                    AssignAIInfo(info);
                    aiManager.AddToActiveEnemies(info.enemyType, info);

                }
            }
            if (breakOutOfLoop)
            {
                break;
            }
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
    }
    void AssignDictionaries()
    {
        foreach (var enemy in enemyPrefabs)
        {
            EnemyInfo info = enemy.GetComponent<EnemyInfo>();
            enemyPrefabDictionary.Add(info.enemyType, enemy);
            activeEnemies.Add(info.enemyType, 0);
            aiManager.enemyPools.Add(info.enemyType, new List<EnemyInfo>());
        }
    }
}
