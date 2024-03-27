using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isPaused = false;
    public int exp;
    [SerializeField] int xpToLevelUp = 5;
    [SerializeField] float levelUpMultiplier;
    public int playerlevel = 0;

    public delegate void PauseDelegate();
    public PauseDelegate pauseEvent;
    public PauseDelegate playEvent;


    UpgradeManager upgradeManager;
    void Start()
    {
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


}
