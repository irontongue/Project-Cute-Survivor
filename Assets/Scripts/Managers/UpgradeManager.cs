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
public enum UpgradeType {addWeapon, dmg, atkSpd, size, duration, projectileSpeed, specialOne, specialTwo}
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
    public Texture image;
    public UpgradeNode nextNode;
    public string nextNodeName;
    //public float weight;

}
[System.Serializable]
public class UpgradeCard
{
    public  TextMeshProUGUI name;
    public TextMeshProUGUI description;
    public RawImage rawImage;
}

public class UpgradeManager : MonoBehaviour
{
    [SerializeField, Tooltip("In Order of the Weapons enum")]  
    GameObject[] weaponGameObjects;
    [SerializeField] GameObject upgradeUI;
    [SerializeField] UpgradeTree[] upgradeTrees;
    [SerializeField] UpgradeCard[] upgradeCards;
    public List<UpgradeNode> upgradePool = new List<UpgradeNode>();
    public UpgradeNode[] chosenUpgrades = new UpgradeNode[3];
    WeaponStats weaponStats;
    GameManager gameManager;
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        weaponStats = FindFirstObjectByType<WeaponStats>();
        InitializeUpgradePool();
        AddWeapon(upgradeTrees[0].nodes[0]);
        for (int i = 0; i < chosenUpgrades.Length; i++)
        {
            chosenUpgrades[i] = null;
            upgradeCards[i].name.text = "You've Run Out Of Upgrades!";
            upgradeCards[i].description.text = "|_-(0.0)-_|";
        }

    }
    void InitializeUpgradePool()
    {
        foreach (UpgradeTree tree in upgradeTrees)
        {
            //if(tree.treeActive)
              //  upgradePool.Add(tree.nodes[0]);
            for(int i = 0; i < tree.nodes.Length; i++) // Set each Node's next Node in the list
            {
                if(i != tree.nodes.Length - 1)
                {
                    tree.nodes[i].nextNode = tree.nodes[i + 1];
                    tree.nodes[i].nextNodeName = tree.nodes[i + 1].name;
                }
                if(tree.treeActive && tree.nodes[i].weapon != Weapons.W_Turret)
                    upgradePool.Add(tree.nodes[i]);
            }
        }
    }

    void Update()
    {
        //if(gameManager.isPaused != true)
        //    UpgradeEvent();
        if(Input.GetKeyDown(KeyCode.I)) 
        {
            gameManager.GiveExp(10);
        }
    }

    public void UpgradeEvent()
    {
        if (upgradePool.Count == 0)
            return;
        //foreach(UpgradeNode upgradeNode in upgradePool)
        //{
        //    print(upgradeNode.name);
        //}
        gameManager.pauseEvent();
        int iterations = 3;
        if(upgradePool.Count < 3)
        {
            iterations = upgradePool.Count;
        }
        
        HashSet<int> chosenNumbers = new HashSet<int>(); //Using HasSet To gaurentee unique numbers being pulled;
        for(int i = 0; i < iterations; i++) //Loop Adds i new ugprades to the chosenUpgrades
        {
            int randomNumber = 0;
            int maxLoops = 0; //Stop Infinite loop.
            while (chosenNumbers.Count - 1 < i)
            {
                if (maxLoops > 100)
                {
                    print("failed");
                    break;
                }
                maxLoops++;
                randomNumber = Random.Range(0, upgradePool.Count);
                chosenNumbers.Add(randomNumber);
            }
            chosenUpgrades[i] = upgradePool[randomNumber];

            //Assign Upgrades to Cards **
            upgradeCards[i].name.text = chosenUpgrades[i].name;
            upgradeCards[i].rawImage.texture = chosenUpgrades[i]?.image;
            upgradeCards[i].description.text = chosenUpgrades[i].description;
            upgradeCards[i].name.gameObject.SetActive(true);
        }

        upgradeUI.SetActive(true);
    }


    public void buttonInput(int card)
    {
        if (chosenUpgrades[card].name == null)
            return;
        
        ChooseUpgrade(chosenUpgrades[card]);
        upgradeUI.SetActive(false);
        gameManager.playEvent();
        foreach(UpgradeCard upgradeCard in upgradeCards) 
        {
            upgradeCard.name.gameObject.SetActive(false);
        }
    }
    public void ChooseUpgrade(UpgradeNode node)
    {
        if (node.nextNode != null) 
            upgradePool.Add(node.nextNode);
        
        switch(node.upgradeType) 
        {
            case UpgradeType.addWeapon:
                //Add Image to UI **
                AddWeapon(node);
                break;
            case UpgradeType.dmg:
                weaponStats.GetWeapon(node.weapon).damage += (int)node.amount;
                break;
            case UpgradeType.atkSpd:
                weaponStats.GetWeapon(node.weapon).attackSpeed += weaponStats.GetWeapon(node.weapon).baseAttackSpeed * Mathf.Ceil(1 + node.amount / 100);
                break;
            case UpgradeType.size:
                    weaponStats.GetWeapon(node.weapon).projectileSize += node.amount;
                break;
            case UpgradeType.projectileSpeed:
                weaponStats.GetWeapon(node.weapon).projectileSpeed += node.amount;
                break;
            case UpgradeType.duration:
                weaponStats.GetWeapon(node.weapon).duration = weaponStats.GetWeapon(node.weapon).baseDuration *= Mathf.Ceil(1 + node.amount / 100);
                break;
            case UpgradeType.specialOne:

                break;
        }
        for(int i = 0; i < chosenUpgrades.Length; i++)
        {
            chosenUpgrades[i] = null;
            upgradeCards[i].name.text = "You've Run Out Of Upgrades!";
            upgradeCards[i].description.text = "|_-(0.0)-_|";
        }
        upgradePool.Remove(node);
    }
    private void AddWeapon(UpgradeNode node)
    {
        weaponGameObjects[(int)node.weapon].SetActive(true);
        foreach (UpgradeTree tree in upgradeTrees)
        {
            UpgradeNode upNode = tree.nodes[0];
            if (upNode.upgradeType == UpgradeType.addWeapon)
                continue;
            if (upNode.weapon == node.weapon)
            {
                tree.treeActive = true;
                upgradePool.Add(upNode);
            }
        }
    }

}
