using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Object/Product",fileName = "New Product")]
public class ProductObject : ScriptableObject
{
    public int ID = 0;
    public string productName = "New Product";
    public string productType = "None";
    public string worker = "Blacksmith";
    public int price = 100;
    public int quantityToAdd = 1;
    public int minutesToMake;
    public int commonStrength = 1;
    public int commonMaxDurability = 100;
    public int quantity {
        get {   int sum = 0;
                foreach(int i in inventory.Values) sum += i;
                return sum; }
    }
    public Dictionary<Rarity,int> inventory = new Dictionary<Rarity, int>() {
        {Rarity.common,0}, {Rarity.rare,0}, {Rarity.unique,0}, {Rarity.legendary,0}};
    public Dictionary<Rarity,float> multiplier = new Dictionary<Rarity, float>() {
        {Rarity.common,1f}, {Rarity.rare,2.5f}, {Rarity.unique,6.25f}, {Rarity.legendary,15.625f}};
    public List<IndividualProductObject> specialInventory = new List<IndividualProductObject>();
    public Sprite img;
    public string desc = "Item Description";
    public List<ProductObject> unlockNext;
    public List<int> materialQuantity;
    public List<string> materialName;
    public Clock timeToMake {
        get { return new Clock(minutesToMake);}
        set { minutesToMake = value.minutesTotal;}
    }

    Dictionary<string,int> materialDict = new Dictionary<string, int>() {
        {"Wood 1", 10}, {"Wood 2", 50}, {"Wood 3", 250},
        {"Metal 1", 25}, {"Metal 2", 125}, {"Metal 3", 625},
        {"Thread 1", 10}, {"Thread 2", 50}, {"Thread 3", 250},
        {"Cloth 1", 25}, {"Cloth 2", 125}, {"Cloth 3", 625},
        {"Herb 1", 10}, {"Herb 2", 50}, {"Herb 3", 250},
        {"Rune 1", 25}, {"Rune 2", 125}, {"Rune 3", 625}};

    public int cost {
        get {
            int sum = 0;
            for (int i = 0; i < materialName.Count; i++) {
                if (materialDict.ContainsKey(materialName[i])){
                    sum += materialDict[materialName[i]]*materialQuantity[i];
                }
            }
            return sum;
        }
    }
}
