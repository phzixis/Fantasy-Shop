using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<ProductObject> inventory = new List<ProductObject>();
    public Dictionary<string,List<ProductObject>> typesDict = new Dictionary<string, List<ProductObject>>();
    [SerializeField] List<MaterialObject> materialList;
    public Dictionary<string, MaterialObject> materialDict = new Dictionary<string,MaterialObject>();
    
    ManagerManager managerManager;
    PlayerManager playerManager;
    UIManager uiManager;

    string[] typesList = {"Sword", "Shield", "Dagger", "Axe", "Hammer", "Spear", 
        "Helmet", "Shoulderplate", "Pants", "Boots", "Gloves", "Accessory", 
        "Bow", "Crossbow", "Gun", "Arrows", "Bolts", "Bullets", 
        "Book", "Staff", "Wand", "Potion"};

    void Awake() {
        managerManager = GameObject.Find("Managers").GetComponent<ManagerManager>();
        playerManager = managerManager.playerManager;
        uiManager = managerManager.uiManager;
        foreach (string s in typesList) {
            typesDict.Add(s,new List<ProductObject>());
        }
        foreach (MaterialObject m in materialList) {
            materialDict.Add(m.materialID, Instantiate(m));
        }
    }

    public ProductObject SearchProduct (IndividualProductObject p) {
        return SearchProduct(p.info);
    }

    public ProductObject SearchProduct (ProductObject p) {
        foreach (ProductObject x in inventory) {
            if (p.ID == x.ID) {
                return x;
            }
        }
        return null;
    }

    public void AddProduct (ProductObject p, Rarity r) {
        ProductObject existing = SearchProduct(p);
        if (existing != null) {
            existing.inventory[r] += p.quantityToAdd;
        } else {
            inventory.Add(Instantiate(p));
        }
    }

    public List<ProductObject> GetProductsOfType(string type) {
        if (typesDict[type].Count == 0) {
            foreach (ProductObject p in inventory) {
                if (p.productType == type) {
                    typesDict[type].Add(p);
                }
            }
        }
        return typesDict[type];
    }

    bool RemoveProduct (IndividualProductObject p) {
        ProductObject existing = SearchProduct(p);
        if (existing != null) {
            if (existing.inventory[p.rarity] >= p.quantity) {
                existing.inventory[p.rarity] -= p.quantity;
                if (existing.inventory[p.rarity] <= 0) {
                    existing.inventory[p.rarity] = 0;
                }
                return true;
            }
        }
        return false;
    }

    public bool HasMaterial(string name, int quantity) {
        return materialDict[name].quantity >= quantity;
    }

    public void RemoveMaterial(string name, int quantity) {
        materialDict[name].quantity -= quantity;
    }

    public void AddMaterial(string name, int quantity) {
        materialDict[name].quantity += quantity;
    }

    public void SellProduct(IndividualProductObject p) {
        playerManager.GainExp(p.sellPrice);
        RemoveProduct(p);
        playerManager.gold += p.sellPrice * p.quantity;
        uiManager.UpdateTopUI();
    }

    public (string[],int[]) SaveMaterials() {
        string[] IDs = new string[materialDict.Count];
        int [] MaxQuantity = new int[materialDict.Count];
        int i = 0;
        foreach(MaterialObject m in materialDict.Values) {
            IDs[i] = m.materialID;
            MaxQuantity[i] = m.maxQuantity;
            i++;
        }
        return (IDs, MaxQuantity);
    }

    public void LoadMaterials(string[] MaterialID, int[] Materials) {
        for(int i = 0; i < MaterialID.Length; i++) {
            materialDict[MaterialID[i]].maxQuantity = Materials[i];
        }
    }

    public int[,] SaveInventory() {
        int[,] save = new int[inventory.Count, 5];
        for (int i = 0; i < inventory.Count; i++) {
            save[i,0] = inventory[i].ID;
            save[i,1] = inventory[i].inventory[Rarity.common];
            save[i,2] = inventory[i].inventory[Rarity.rare];
            save[i,3] = inventory[i].inventory[Rarity.unique];
            save[i,4] = inventory[i].inventory[Rarity.legendary];
        }
        return save;
    }

    public void LoadInventory(int[,] load) {
        inventory = new List<ProductObject>();
        Dictionary<int,ProductObject> idDict = managerManager.productDictionary.productIdDict;
        for (int i = 0; i < load.GetLength(0); i++) {
            if (!idDict.ContainsKey(i)) continue;
            ProductObject newProd = Instantiate(idDict[load[i,0]]);
            newProd.inventory[Rarity.common] = load[i,1];
            newProd.inventory[Rarity.rare] = load[i,2];
            newProd.inventory[Rarity.unique] = load[i,3];
            newProd.inventory[Rarity.legendary] = load[i,4];
            inventory.Add(newProd);
        }
    }
}
