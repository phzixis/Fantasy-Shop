using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductDictionary : MonoBehaviour
{
    public List<ProductObject> productDict;
    public Dictionary<string,List<ProductObject>> typesDict = new Dictionary<string, List<ProductObject>>();
    public Dictionary<int, ProductObject> productIdDict = new Dictionary<int, ProductObject>();


    string[] typesList = {"Sword", "Shield", "Dagger", "Axe", "Hammer", "Spear", 
        "Helmet", "Shoulderplate", "Pants", "Boots", "Gloves", "Accessory", 
        "Bow", "Crossbow", "Gun", "Arrows", "Bolts", "Bullets", 
        "Book", "Staff", "Wand", "Potion"};

    void Awake() {
        foreach (string s in typesList) {
            typesDict.Add(s,new List<ProductObject>());
        }
        foreach (ProductObject p in productDict) {
            productIdDict.Add(p.ID,p);
        }
    }

    List<ProductObject> GetProductsOfType(string type) {
        if (typesDict[type].Count == 0) {
            foreach (ProductObject p in productDict) {
                if (p.productType == type) {
                    typesDict[type].Add(p);
                }
            }
        }
        return typesDict[type];
    }

    public ProductObject GetRandomWeaponOfType(string type) {
        List<ProductObject> productsList = GetProductsOfType(type);
        return productsList[Random.Range(0,productsList.Count)];
    }

    public ProductObject GetProductFromID(int id) {
        return productIdDict[id];
    }
}
