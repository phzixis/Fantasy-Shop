using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureManager : MonoBehaviour
{
    [SerializeField] List<FurnitureObject> furnitureList = new List<FurnitureObject>();
    public Dictionary<int, FurnitureObject> furnitureDict = new Dictionary<int, FurnitureObject>();

    ManagerManager managerManager;
    WorkerManager workerManager;
    PlayerManager playerManager;

    void Awake() {
        managerManager = GameObject.FindObjectOfType<ManagerManager>();
        workerManager = managerManager.workerManager;
        playerManager = managerManager.playerManager;
    }

    void Start() {
        foreach (FurnitureObject f in furnitureList) {
            furnitureDict.Add(f.id,Instantiate(f));
        }
    }

    public int[,] SaveFurniture() {
        int[,] data = new int[furnitureDict.Count,3];
        foreach(FurnitureObject F in furnitureDict.Values) {
            data[F.id,0] = F.lvl;
            data[F.id,1] = F.locked ? 1 : 0;
            data[F.id,2] = F.isUpgrading ? 1 : 0; 
        }
        return data;
    }

    public void LoadFurniture(int[,] data) {
        for(int i = 0; i < data.GetLength(0); i++) {
            furnitureDict[i].lvl = data[i,0];
            furnitureDict[i].locked = data[i,1] == 1;
            furnitureDict[i].isUpgrading = data[i,2] == 1;
        }
    }

    public void AddFurniture(int id, int val) {
        FurnitureObject F = furnitureDict[id];
        if(F.lvl == 0) {
            foreach(int i in F.furnitureToUnlock) {
                furnitureDict[i].locked = false;
            }
        }
        F.lvl += val;
        if(F.worker == "Player") {
            playerManager.AddStats(F.statsToAdd, val);
        } else {
            workerManager.AddStats(F.worker, F.statsToAdd, val);
        }
    }

    public void LevelUpFurniture(int id) {
        workerManager.Craft(furnitureDict[id]);
        playerManager.SpendGold(furnitureDict[id].upgradeCost);
    }

    public int ReturnLvl(int id) {
        return furnitureDict[id].lvl;
    }
}
