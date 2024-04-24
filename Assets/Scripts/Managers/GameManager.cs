using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    float timer;
    public int gameTimer = 0;
    public bool isPaused = false;
    public int exp;
    [SerializeField] int xpToLevelUp = 5;
    [SerializeField] float levelUpMultiplier;
    public int playerlevel = 0;

    public GameObject gameOverUI;
    public GameObject gameWinUI;

    public delegate void PauseDelegate();
    public PauseDelegate pauseEvent;
    public PauseDelegate playEvent;
    public MusicSystem musicSystem;
    [HideInInspector] public AIManager aiManager;

    [Header("PickUpItems")]
    public int healthPacksAvaliable;
    [SerializeField] public GameObject healthPack;
    [SerializeField] int timeBetweenHealthPacks = 60;

    UpgradeManager upgradeManager;
    void Start()
    {
        aiManager = FindFirstObjectByType<AIManager>();
        print("I started");
        upgradeManager = GetComponent<UpgradeManager>();
        pauseEvent += Pause;
        playEvent += Pause;
        
    }
    bool latch;
    private void Update()
    {
        if (isPaused)
            return;
        timer += Time.deltaTime;
        gameTimer = Mathf.FloorToInt(timer);
        if(gameTimer % timeBetweenHealthPacks == 0)
        {
            latch = true;
        }
        else if(latch)
        {
            latch = false;
            healthPacksAvaliable ++;
        }
    }
    public void DropHealthPack(Vector2 position)
    {
        --healthPacksAvaliable;
        Instantiate(healthPack, position, Quaternion.identity);
    }

    public void GiveExp(int xp)
    {
        exp += xp;
        if(exp >= xpToLevelUp)
        {
            exp = exp - xpToLevelUp;
            LevelUp();
        }
    }
    void LevelUp()
    {
        playerlevel++;
        xpToLevelUp = (int)(xpToLevelUp * levelUpMultiplier);
        upgradeManager.UpgradeEvent();
    }
    public void EliteUpgradeEvent()
    {
        upgradeManager.UpgradeEvent();
    }

    void Pause()
    {
        isPaused = !isPaused;
    }

    public void DeathEvent()
    {
        pauseEvent();
        gameOverUI.SetActive(true);   
    }
    public void WinEvent()
    {
        pauseEvent();
        gameWinUI.SetActive(true);
    }
    public void RestartScene()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void QuitApplication()
    {
        Application.Quit();
    }

    public void IncreaseMusicIntensity()
    {
        musicSystem.IncreaseIntensity();
    }
}
