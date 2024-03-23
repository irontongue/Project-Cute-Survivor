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

    UpgradeManager upgradeManager;
    void Start()
    {
        upgradeManager = GetComponent<UpgradeManager>();
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
        xpToLevelUp = (int)((float)xpToLevelUp * levelUpMultiplier);
        upgradeManager.UpgradeEvent();
    }

}
