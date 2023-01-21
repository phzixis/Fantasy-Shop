using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Object/Material",fileName = "New Material")]
public class MaterialObject : ScriptableObject
{
    
    public string materialName = "New Material";
    public string materialID = "New Material";
    public int quantity = 0;
    public int toAdd = 0;
    public int maxQuantity = 5;
    public Sprite img;
    public int cost;
    public int upgradeCost {
        get {return maxQuantity * 15;}
    }

    public void init(string id, string name, int qty, Sprite sprite) {
        materialID = id; 
        materialName = name;
        quantity = qty;
        img = sprite;
    }

    public void init(int qty) {
        quantity = qty;
    }
} 
