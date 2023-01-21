using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Furniture", menuName = "Object/Furniture")]
public class FurnitureObject : ScriptableObject
{
    public int id;
    public string furnitureName;
    public string worker;
    public int statsToAdd;
    public int lvl;
    public int maxLvl;
    public int minCost;
    public int maxCost;
    public int upgradeCost {
        get {return minCost+((maxCost-minCost)/maxLvl)*lvl;}
    }
    public int minutesToMake;
    [TextArea]public string description;
    public Clock timeToMake {
        get { return new Clock(minutesToMake);}
        set { minutesToMake = value.minutesTotal;}
    }
    public bool locked;
    public bool isUpgrading;
    public List<int> furnitureToUnlock;
    public Sprite sprite;
}
