using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

//public enum UpgradeFor {W_Turret, W_FlameThrower, W_RocketLauncher}
public enum UpgradeType {addWeapon, dmg, atkSpd, size, duration, specialOne, specialTwo}
[System.Serializable]

public class UpgradeTree 
{
    [SerializeField] string name;
    public UpgradeNode[] nodes;
    public bool treeActive;
}
[System.Serializable]
public class UpgradeNode
{
    public string name;
    public Weapons weapon;
    public UpgradeType upgradeType;
    public float amount;
    public string description;
    public UpgradeNode nextNode;
    //public float weight;

}
[System.Serializable]
public class UpgradeCard
{
    public  TextMeshProUGUI name;
    public TextMeshProUGUI description;
    public RawImage spriteRenderer;
}

public class UpgradeManager : MonoBehaviour
{
    [SerializeField, Tooltip("In Order of the Weapons enum")]  
    GameObject[] weaponGameObjects;
    [SerializeField] GameObject upgradeUI;
    [SerializeField] UpgradeTree[] upgradeTrees;
    [SerializeField] UpgradeCard[] upgradeCards;
    List<UpgradeNode> upgradePool = new List<UpgradeNode>();
    UpgradeNode[] chosenUpgrades = new UpgradeNode[3];
    WeaponStats weaponStats;
    GameManager gameManager;
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        weaponStats = new WeaponStats();
        InitializeUpgradePool();
    }
    void InitializeUpgradePool()
    {
        foreach (UpgradeTree tree in upgradeTrees)
        {
            if(tree.treeActive)
                upgradePool.Add(tree.nodes[0]);
            for(int i = 0; i < tree.nodes.Length; i++) // Set each Node's next Node in the list
            {
                if(i != tree.nodes.Length - 1)
                tree.nodes[i].nextNode = tree.nodes[i + 1];
            }
        }
    }

    void Update()
    {
        
    }

    public void UpgradeEvent()
    {
        gameManager.isPaused = true;

        int[] chosenNumbers = new int[2];
        for(int i = 0; i < 3; i++) //Loop Adds i new ugprades to the chosenUpgrades
        {
            int randomNumber = 0;
            int whileCounter = 0;
            while (true)
            {
                whileCounter++;
                if(whileCounter > 1000)
                {
                    print("InfinteLoop here");
                    break;
                }
                
                randomNumber = Random.Range(0, upgradePool.Count - 1);
                if(!chosenNumbers.Contains(randomNumber))
                {
                    chosenNumbers[i] = randomNumber;
                    break;
                }
            }
            chosenNumbers[i] = randomNumber;
            chosenUpgrades[i] = upgradePool[randomNumber];
        }
        upgradeUI.SetActive(true);
        //Assign Upgrades to Cards **
    }

    public void buttonInput(int card)
    {
        choseUpgrade(chosenUpgrades[card]);
        upgradeUI.SetActive(false);
        gameManager.isPaused = false;
    }
    public void choseUpgrade(UpgradeNode node)
    {
        if (node.nextNode != null)
            upgradePool.Add(node.nextNode);
        
        switch(node.upgradeType) 
        {
            case UpgradeType.addWeapon:
                //Add Image to UI **
                weaponGameObjects[(int)node.weapon].SetActive(true);
                foreach(UpgradeTree tree in upgradeTrees)
                {
                    UpgradeNode upNode = tree.nodes[0];
                    if (upNode.weapon == node.weapon)
                    {
                        tree.treeActive = true;
                        upgradePool.Add(upNode);
                    }
                }
                break;
            case UpgradeType.dmg:
                weaponStats.GetWeapon(node.weapon).damage += (int)node.amount;
                break;
            case UpgradeType.atkSpd:
                    weaponStats.GetWeapon(node.weapon).baseAttackSpeed *= (1 + (int)node.amount / 100);
                break;
            case UpgradeType.size:
                    weaponStats.GetWeapon(node.weapon).projectileSize += node.amount;
                break;
            case UpgradeType.duration:
                    weaponStats.GetWeapon(node.weapon).baseDuration *= (1 + (int)node.amount / 100);
                break;
            case UpgradeType.specialOne:

                break;
        }
    }

}
