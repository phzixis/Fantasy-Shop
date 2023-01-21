using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int gold;
    public int gems;
    public int lvl;
    public int popularity;
    public int efficiency;
    public int charisma;
    public int exp;
    public int nextLvlExp;

    public int[,] inventory;
    public string[] materialID;
    public int[] materials;

    public int[] recipesUnlocked;
    public int[] recipesToResearch;
    
    public float[] blacksmithStats;
    public float[] armorsmithStats;
    public float[] craftsmanStats;
    public float[] sorcererStats;

    public int[,] blacksmithTasks;
    public int[,] armorsmithTasks;
    public int[,] craftsmanTasks;
    public int[,] sorcererTasks;

    public int[,] furnitureStats;

    public PlayerData (PlayerManager pm, InventoryManager im, RecipeManager rm, WorkerManager wm, FurnitureManager fm) {
        gold = pm.gold;
        gems = pm.gems;
        lvl = pm.lvl;
        popularity = pm.popularity;
        efficiency = pm.efficiency;
        charisma = pm.charisma;
        exp = pm.exp;
        nextLvlExp = pm.nextLvlExp;
        
        inventory = im.SaveInventory();
        (materialID, materials) = im.SaveMaterials();
        
        recipesUnlocked = rm.SaveRecipesUnlocked();
        recipesToResearch = rm.SaveCanResearchRecipes();

        blacksmithStats = wm.blacksmith.SaveStats();
        armorsmithStats = wm.armorsmith.SaveStats();
        craftsmanStats = wm.craftsman.SaveStats();
        sorcererStats = wm.sorcerer.SaveStats();

        blacksmithTasks = wm.blacksmith.SaveTasks();
        armorsmithTasks = wm.armorsmith.SaveTasks();
        craftsmanTasks = wm.craftsman.SaveTasks();
        sorcererTasks = wm.sorcerer.SaveTasks();

        furnitureStats = fm.SaveFurniture();
    }
}
