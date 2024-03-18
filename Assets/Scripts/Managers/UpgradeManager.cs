using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeFor {W_Turret, W_FlameThrower, W_RocketLauncher}
public enum UpgradeType {dmg, atkSpd, size, duration, specialOne, specialTwo}
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
    public UpgradeFor upgrade;
    public UpgradeType upgradeType;
    public float amount;
    public string description;
    //public float weight;

}
public class UpgradeManager : MonoBehaviour
{
    [SerializeField] GameObject upgradeUI;
    [SerializeField] UpgradeTree[] upgradeTrees;
    List<UpgradeNode> upgradePool = new List<UpgradeNode>();
    void Start()
    {
        InitializeUpgradePool();
    }
    void InitializeUpgradePool()
    {
        foreach (UpgradeTree tree in upgradeTrees)
        {
            if(tree.treeActive)
            upgradePool.Add(tree.nodes[0]);
        }
    }

    void Update()
    {
        
    }

    void UpgradeEvent()
    {
        upgradeUI.SetActive(true);
        //Get Three Upgrades
        //Assign Upgrades to Cards
        
    }


}
