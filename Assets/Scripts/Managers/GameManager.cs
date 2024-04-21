using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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


    UpgradeManager upgradeManager;
    void Start()
    {
        aiManager = FindFirstObjectByType<AIManager>();
        print("I started");
        upgradeManager = GetComponent<UpgradeManager>();
        pauseEvent += Pause;
        playEvent += Pause;
        
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
