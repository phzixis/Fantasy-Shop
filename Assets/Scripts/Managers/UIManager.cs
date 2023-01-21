using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject buyingMenu;
    [SerializeField] GameObject craftingMenu;
    [SerializeField] GameObject researchMenu;
    [SerializeField] GameObject inventoryMenu;
    [SerializeField] GameObject furnitureMenu;
    [SerializeField] GameObject dayTransitionMenu;
    [SerializeField] GameObject workerMenu;
    [SerializeField] GameObject mergeMenu;
    ManagerManager managerManager;
    PlayerManager pm;
    TimeManager tm;

    [SerializeField] TextMeshProUGUI gold;
    [SerializeField] TextMeshProUGUI gems;
    [SerializeField] TextMeshProUGUI time;
    [SerializeField] TextMeshProUGUI lvl;
    [SerializeField] TextMeshProUGUI day;
    [SerializeField] Image popBar;

    void Awake() {
        managerManager = GameObject.Find("Managers").GetComponent<ManagerManager>();
        pm = managerManager.playerManager;
        tm = managerManager.timeManager;
    }

    void Start() {
        UpdateTopUI();
    }

    public void UpdateTopUI() {
        gold.text = pm.gold.ToString();
        gems.text = pm.gems.ToString();
        time.text = tm.timeInString;
        lvl.text = pm.lvl.ToString();
        popBar.fillAmount = (float)pm.exp/pm.nextLvlExp;
        day.text = "Day " + pm.day.ToString();
    }

    public void UpdateTime(string newTime) {
        time.text = newTime;
    }
    
    public void CreateBuyingMenu(string text, IndividualProductObject product, NPCScript npc) {
        BuyingMenu bm = Instantiate(buyingMenu, canvas.transform).GetComponent<BuyingMenu>();
        bm.SetProduct(product, text, npc);
    }

    public void OpenCraftingMenu(string Worker) {
        CraftingMenu cm = Instantiate(craftingMenu,canvas.transform).GetComponent<CraftingMenu>();
        cm.CreateItemBoxesOfWorker(Worker);
    }

    public void OpenWorkerMenu(string Worker) {
        WorkerMenu wm = Instantiate(workerMenu,canvas.transform).GetComponent<WorkerMenu>();
        wm.SetWorker(Worker);
    }

    // public void OpenResearchMenu() {
    //     Instantiate(researchMenu,canvas.transform);
    // }

    public void OpenInventoryMenu() {
        Instantiate(inventoryMenu,canvas.transform);
    }

    public void OpenFurnitureMenu() {
        Instantiate(furnitureMenu,canvas.transform);
    }

    public void OpenMergeMenu() {
        Instantiate(mergeMenu, canvas.transform);
    }

    public void SpawnEndTransitionMenu() {
        Instantiate(dayTransitionMenu, canvas.transform);
    }

}
