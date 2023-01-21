using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnitureMenu : MonoBehaviour
{
    public Transform contentParent;

    ManagerManager managerManager;
    FurnitureManager furnitureManager;
    PlayerManager playerManager;
    WorkerManager workerManager;
    TimeManager timeManager;

    public GameObject furnitureBox;
    List<GameObject> boxesList = new List<GameObject>();

    void Awake() {
        managerManager = FindObjectOfType<ManagerManager>();
        furnitureManager = managerManager.furnitureManager;
        playerManager = managerManager.playerManager;
        workerManager = managerManager.workerManager;
        timeManager = managerManager.timeManager;
    }

    void Start() {
        CreateFurnitureBoxes();
        timeManager.Pause();
    }

    public void CreateFurnitureBoxes() {
        foreach (GameObject g in boxesList) {
            Destroy(g);
        }

        List<FurnitureObject> craftableList = new List<FurnitureObject>(); 
        List<FurnitureObject> nonCraftableList = new List<FurnitureObject>();

        foreach (FurnitureObject f in furnitureManager.furnitureDict.Values) {
            if(f.locked) {
                nonCraftableList.Add(f);
            } else {
                craftableList.Add(f);
            }
        }

        foreach(FurnitureObject f in craftableList) {
            CreateSingleBox(f);
        }
        foreach(FurnitureObject f in nonCraftableList) {
            CreateSingleBox(f);
        }
    }

    void CreateSingleBox(FurnitureObject F) {
        GameObject newBox = Instantiate(furnitureBox,contentParent);
        boxesList.Add(newBox);
        MenuDirectory boxDirectory = newBox.GetComponent<MenuDirectory>();
        boxDirectory.tmiText.text = "Lvl "+ (F.lvl+1).ToString() + " " + F.furnitureName;
        
        if(F.statsToAdd == 0 && F.lvl == 0) {
            boxDirectory.tmiText2.text = "Unlock " + F.worker;
        } else {
            boxDirectory.tmiText2.text = F.description;
        }
        boxDirectory.tmiText3.text = F.upgradeCost.ToString();
        boxDirectory.image.sprite = F.sprite;
        
        if(F.locked) {
            boxDirectory.go2.SetActive(true);
        } else {
            Button craftButton = boxDirectory.go.GetComponent<Button>();
            if(!(playerManager.CheckGold(F.upgradeCost) && workerManager.workerDict["Craftsman"].canTakeWork && !F.isUpgrading)) {
                craftButton.interactable = false;
            } else {
                craftButton.onClick.AddListener(()=>furnitureManager.LevelUpFurniture(F.id));
                craftButton.onClick.AddListener(()=>CreateFurnitureBoxes());
            }
            boxDirectory.go.SetActive(true);
        }
    }

    public void Close() {
        timeManager.Resume();
        Destroy(gameObject);
    }

}
