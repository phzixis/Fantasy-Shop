using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int gold;
    public int gems;
    public int lvl;
    public int popularity;
    public int efficiency;
    public int charisma;
    public int exp;
    public int nextLvlExp;
    public int day;
    
    public int startExpToday;
    public int expToday;
    public int lvlToday;
    
    public bool willTutorial;
    public GameObject tutorialManager;
    ManagerManager managerManager;

    public List<UnlockObject> unlockObjects;
    Dictionary<int,UnlockObject> unlockDict = new Dictionary<int, UnlockObject>();

    void Awake() {
        managerManager = GameObject.Find("Managers").GetComponent<ManagerManager>();
    }

    void Start() {
        foreach(UnlockObject u in unlockObjects) {
            unlockDict.Add(u.lvl,u);
        }
    }

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    void SetStartingStats() {
        gold = 500;
        gems = 50;
        lvl = 1;
        popularity = 1;
        efficiency = 1;
        charisma = 1;
        exp = 0;
        nextLvlExp = 50;
        day = 0;
    }

    public void NewGame() {
        if(willTutorial) {
            GameObject manager = Instantiate(tutorialManager,transform.parent);
        }
    }

    public void SaveGame() {
        Debug.Log("Saving Game");
        SaveManager.SaveData(this, managerManager.inventoryManager,managerManager.recipeManager,managerManager.workerManager, managerManager.furnitureManager);
    }

    public void LoadGame() {
        PlayerData pd = SaveManager.LoadData();

        gold = pd.gold;
        gems = pd.gems;
        lvl = pd.lvl;
        popularity = pd.popularity;
        efficiency = pd.efficiency;
        charisma = pd.charisma;
        exp = pd.exp;
        nextLvlExp = pd.nextLvlExp;

        managerManager.inventoryManager.LoadInventory(pd.inventory);
        managerManager.inventoryManager.LoadMaterials(pd.materialID, pd.materials);
        managerManager.recipeManager.LoadRecipes(pd.recipesUnlocked,pd.recipesToResearch);
        managerManager.workerManager.LoadStats(pd.blacksmithStats, pd.armorsmithStats, pd.craftsmanStats, pd.sorcererStats);
        managerManager.workerManager.LoadTasks(pd.blacksmithTasks, pd.armorsmithTasks, pd.craftsmanTasks, pd.sorcererTasks);
        managerManager.furnitureManager.LoadFurniture(pd.furnitureStats);
        managerManager.uiManager.UpdateTopUI();

        for(int i = 2; i <= pd.lvl; i++) {
            Unlock(i);
        }
    }

    public bool CheckGold(int Amount) {
        return gold >= Amount;
    }

    public void SpendGold(int Amount) {
        gold -= Amount;
        managerManager.uiManager.UpdateTopUI();
    }

    public void GainGold(int Amount) {
        gold += Amount;
        managerManager.uiManager.UpdateTopUI();
    }

    public void AddStats(int StatsID, int Amount) {
        switch (StatsID) {
            case 0:
                popularity += Amount;
                break;
            case 1:
                efficiency += Amount;
                break;
            case 2:
                charisma += Amount;
                break;
        }
    }

    public void StartDay() {
        expToday = 0;
        startExpToday = exp;
        lvlToday = lvl;
        day++;
    }

    public void LevelUp() {
        int statIncrease;
        lvl++;
        if (lvl < 27)   statIncrease = 2;
        else            statIncrease = 1;
        popularity += statIncrease;
        efficiency += statIncrease;
        charisma += statIncrease;

        nextLvlExp = GetNextLvlExp(lvl);
        Unlock(lvl);
    }

    public int GetNextLvlExp(int lvl) {
        if (lvl == 2)       return 100;
        else if (lvl < 13)  return (lvl-2)*200;
        else                return 2000 + (lvl-12)*300; 

    }

    public void GainExp(int Exp) {
        exp += Exp;
        expToday += Exp;
        if (exp >= nextLvlExp) {
            exp -= nextLvlExp;
            LevelUp();
        }
    }

    public float GetPriceBound() {
        float lowerBound = .5f + (float) efficiency * .005f;
        return lowerBound;
    }

    void Unlock(int Lvl) {
        if(!unlockDict.ContainsKey(Lvl)) return;
        UnlockObject toUnlock = unlockDict[Lvl];
        foreach (string s in toUnlock.features) {
            UnlockFeature(s);
        }
        foreach (CustomerObject c in toUnlock.customers) {
            managerManager.npcManager.AddNPC(c);
        }
        foreach (FurnitureObject f in toUnlock.furnitures) {
            
        }
    }

    void UnlockFeature(string feature) {
        Debug.Log("unlocked "+feature);
        switch(feature) {
            case "Furniture":
                break;
            case "Town":
                break;
            case "Quests":
                break;
            case "Bank":
                break;
            case "Armorsmith":
                break;
            case "Guild":
                break;
            case "World":
                break;
            case "Merge":
                break;
            case "Sorcerer":
                break;
            default:
                Debug.LogError(feature + " feature does not exist!");
                break;
        }
    }
}
