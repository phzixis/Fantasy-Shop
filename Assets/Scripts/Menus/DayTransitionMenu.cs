using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DayTransitionMenu : MonoBehaviour
{
    [SerializeField] GameObject contents;
    [SerializeField] Image fade;
    [SerializeField] float fadeDuration;
    [SerializeField] GameObject dailyReport;
    [SerializeField] GameObject levelUp;
    [SerializeField] GameObject restock;
    [SerializeField] Transform incomeContent;
    [SerializeField] Transform producedContent;
    [SerializeField] GameObject productImageObject;
    [SerializeField] TextMeshProUGUI incomeTotal;
    [SerializeField] List<MenuDirectory> expDirectories;
    [SerializeField] TextMeshProUGUI playerName;
    [SerializeField] GameObject nextScreenButton;
    [SerializeField] List<MenuDirectory> materialButtonList;
    [SerializeField] List<MenuDirectory> materialBoxList;
    [SerializeField] TextMeshProUGUI goldText;
    Dictionary<string,MenuDirectory> materialButtonDict = new Dictionary<string, MenuDirectory>();
    
    ManagerManager managerManager;
    TimeManager timeManager;
    EventsManager eventsManager;
    PlayerManager playerManager;
    WorkerManager workerManager;
    InventoryManager inventoryManager;
    
    bool fadingIn = false;
    bool fadingOut = false;
    string state;
    string currentMaterial;
    float period = .20f;
    string[] matNames = {"Metal", "Wood", "Cloth", "Thread", "Rune", "Herb"};

    void Awake() {
        managerManager = FindObjectOfType<ManagerManager>();
        timeManager = managerManager.timeManager;
        eventsManager = managerManager.eventsManager;
        playerManager = managerManager.playerManager;
        workerManager = managerManager.workerManager;
        inventoryManager = managerManager.inventoryManager;
    }

    void Start() {
        for(int i = 0; i < 6; i++) {
            materialButtonDict.Add(matNames[i],materialButtonList[i]);
        }
        ShowScreen();
    }

    void Update() {
        if (fadingIn) {
            Color fadeColor = fade.color;
            fadeColor.a += 1/fadeDuration * Time.deltaTime;
            fade.color = fadeColor;
            if (fadeColor.a >= 1) {
                fadingIn = false;
                fadingOut = true;
                if (state == "start") {
                    contents.SetActive(true);
                    StartCoroutine(ShowDailyReport());
                } else if (state == "end") {
                    contents.SetActive(false);
                }
            }
        }

        if (fadingOut) {
            Color fadeColor = fade.color;
            fadeColor.a -= 1/fadeDuration * Time.deltaTime;
            fade.color = fadeColor;
            if (fadeColor.a <= 0) {
                fadingOut = false;
                fade.gameObject.SetActive(false);
                if (state == "end") {
                    Destroy(gameObject);
                }
            }
        }
    }

    IEnumerator ShowDailyReport() {
        nextScreenButton.SetActive(true);
        dailyReport.SetActive(true);
        state = "showing daily report";
        int total = 0;
        foreach(IndividualProductObject p in eventsManager.productsSoldToday) {
            GameObject go = Instantiate(productImageObject, incomeContent);
            go.GetComponent<Image>().sprite = p.info.img;
            total += p.sellPrice;
            incomeTotal.text = "Total:<color=#FFFF00>"+total.ToString()+"g</color>"; 
            yield return new WaitForSeconds(period);
        }

        foreach(ProductObject p in eventsManager.productsMadeToday) {
            GameObject go = Instantiate(productImageObject, producedContent);
            go.GetComponent<Image>().sprite = p.img; 
            yield return new WaitForSeconds(period);
        }
        state = "daily report";
        period = .20f;
    }

    IEnumerator ShowLeveUp() {
        dailyReport.SetActive(false);
        levelUp.SetActive(true);
        state = "showing level up";
        int inc = 50;
        while (true) {
            bool willBreak = true;
            if(playerManager.expToday > inc) {
                playerManager.expToday -= inc;
                playerManager.startExpToday += inc;
                willBreak = false;
            } else {
                playerManager.expToday = 0;
                playerManager.startExpToday += playerManager.expToday;
            }
            
            if(playerManager.expToday >= playerManager.GetNextLvlExp(playerManager.lvlToday)) {
                playerManager.expToday -= playerManager.GetNextLvlExp(playerManager.lvlToday);
                playerManager.lvlToday++;
            }
            expDirectories[0].tmiText.text = playerManager.lvlToday.ToString(); 
            expDirectories[0].tmiText2.text = "Exp left: " + (playerManager.GetNextLvlExp(playerManager.lvlToday) - playerManager.expToday).ToString();
            expDirectories[0].image.fillAmount = playerManager.startExpToday/(float)playerManager.GetNextLvlExp(playerManager.lvlToday);
            int i = 1;
            foreach(Worker w in workerManager.workerDict.Values) {
                if(w.expToday > inc) {
                    w.expToday -= inc;
                    w.startExpToday += inc;
                    willBreak = false;
                } else {
                    w.expToday = 0;
                    w.startExpToday += w.expToday;
                }
                
                if(w.expToday >= w.GetNextLvlExp(w.lvlToday)) {
                    w.expToday -= w.GetNextLvlExp(w.lvlToday);
                    w.lvlToday++;
                }
                string[] text = expDirectories[i].tmiText.text.Split(' ');
                text[1] = w.lvlToday.ToString();
                expDirectories[i].tmiText.text = string.Join(" ", text); 
                expDirectories[i].tmiText2.text = "Exp left: " + (w.GetNextLvlExp(w.lvlToday) - w.expToday).ToString();
                expDirectories[i].image.fillAmount = w.startExpToday/(float)w.GetNextLvlExp(w.lvlToday);
                i++;
            }
            if (willBreak) {
                break;
            }
            yield return new WaitForSeconds(period/5);
        }
        state = "level up";
        nextScreenButton.SetActive(false);
    }

    public void ShowRestock() {
        levelUp.SetActive(false);
        restock.SetActive(true);
        UpdateGold();
        SetMaterial("Metal");

        RefreshMaterialButtons();
    }

    void RefreshMaterialButtons() {
        for(int i = 0; i < 6; i++) {
            int cur = 0, max = 0;
            for(int j = 0; j < 4; j++) {
                string curMat = matNames[i] + " " + (j+1).ToString();
                MaterialObject mat = inventoryManager.materialDict[curMat];
                cur += mat.quantity + mat.toAdd;
                max += mat.maxQuantity;
            }
            materialButtonList[i].tmiText.text = cur.ToString()+"/"+max.ToString();
        }
    }

    void ShowScreen() {
        fadingIn = true;
        state = "start";
    }

    public void RemoveScreen() {
        timeManager.StartDay();
        playerManager.SaveGame();
        fade.gameObject.SetActive(true);
        
        fadingIn = true;
        state = "end";
    }

    public void NextScreen() {
        switch(state) {
            case "showing daily report":
                period = 0;
                break;
            case "daily report":
                StartCoroutine(ShowLeveUp());
                break;
            case "showing level up":
                period = 0;
                break;
            case "level up":
                ShowRestock();
                break;
        }
    } 

    public void SetMaterial(string name) {
        currentMaterial = name;
        for(int i = 0; i < 4; i++) {
            string id = name + " " + (i+1).ToString();
            MaterialObject mat = inventoryManager.materialDict[id];
            materialBoxList[i].image.sprite = mat.img;
            materialBoxList[i].tmiText.text = mat.materialName + " <color=yellow>" + mat.cost.ToString() + "g</color>";
            string lhs = mat.toAdd > 0 ? "<color=green>"+(mat.toAdd + mat.quantity).ToString()+"</color>" : mat.quantity.ToString();
            materialBoxList[i].tmiText2.text = lhs + "/" + mat.maxQuantity.ToString();
            materialBoxList[i].go.GetComponent<Button>().interactable = mat.quantity + mat.toAdd != mat.maxQuantity && playerManager.CheckGold(mat.cost);
            materialBoxList[i].go2.GetComponent<Button>().interactable = mat.toAdd > 0;
        }
    }

    public void AddMaterial(int i) {
        string id = currentMaterial + " " + (i+1).ToString();
        MaterialObject mat = inventoryManager.materialDict[id];
        mat.toAdd += 1;
        playerManager.SpendGold(mat.cost);
        string lhs = "<color=green>"+(mat.toAdd + mat.quantity).ToString()+"</color>";
        materialBoxList[i].tmiText2.text = lhs + "/" + mat.maxQuantity.ToString();
        materialBoxList[i].go.GetComponent<Button>().interactable = mat.quantity + mat.toAdd != mat.maxQuantity && playerManager.CheckGold(mat.cost);
        materialBoxList[i].go2.GetComponent<Button>().interactable = true;
        RefreshMaterialButtons();
        UpdateGold();
    }

    public void SubMaterial(int i) {
        string id = currentMaterial + " " + (i+1).ToString();
        MaterialObject mat = inventoryManager.materialDict[id];
        mat.toAdd -= 1;
        playerManager.GainGold(mat.cost);
        string lhs = mat.toAdd > 0 ? "<color=green>"+(mat.toAdd + mat.quantity).ToString()+"</color>" : mat.quantity.ToString();
        materialBoxList[i].tmiText2.text = lhs + "/" + mat.maxQuantity.ToString();
        materialBoxList[i].go.GetComponent<Button>().interactable = playerManager.CheckGold(mat.cost);
        materialBoxList[i].go2.GetComponent<Button>().interactable = mat.toAdd > 0;
        RefreshMaterialButtons();
        UpdateGold();
    }

    public void RefillAll() {
        for(int i = 0; i < 6; i++) {
            for(int j = 0; j < 4; j++) {
                string id = matNames[i] + " " + (j+1).ToString();
                MaterialObject mat = inventoryManager.materialDict[id];
                while (mat.quantity+mat.toAdd < mat.maxQuantity) {
                    if(!playerManager.CheckGold(mat.cost)) {
                        break;
                    }
                    mat.toAdd += 1;
                    playerManager.SpendGold(mat.cost);
                    if(matNames[i] == currentMaterial) {
                        string lhs = mat.toAdd > 0 ? "<color=green>"+(mat.toAdd + mat.quantity).ToString()+"</color>" : mat.quantity.ToString();
                        materialBoxList[j].tmiText2.text = lhs + "/" + mat.maxQuantity.ToString();
                        materialBoxList[j].go.GetComponent<Button>().interactable = mat.quantity + mat.toAdd != mat.maxQuantity && playerManager.CheckGold(mat.cost);
                        materialBoxList[j].go2.GetComponent<Button>().interactable = true;
                    }
                }
                
            }
        }
        RefreshMaterialButtons();
        UpdateGold();
    }

    public void Revert() {
        for(int i = 0; i < 6; i++) {
            for(int j = 0; j < 4; j++) {
                string id = matNames[i] + " " + (j+1).ToString();
                MaterialObject mat = inventoryManager.materialDict[id];

                while (mat.toAdd > 0) {
                    mat.toAdd -= 1;
                    playerManager.GainGold(mat.cost);
                    if(matNames[i] == currentMaterial) {
                        materialBoxList[j].tmiText2.text = mat.quantity.ToString() + "/" + mat.maxQuantity.ToString();
                        materialBoxList[j].go.GetComponent<Button>().interactable = playerManager.CheckGold(mat.cost);
                        materialBoxList[j].go2.GetComponent<Button>().interactable = false;
                    }
                }
            }
        }
        RefreshMaterialButtons();
        UpdateGold();
    }

    public void UpdateGold() {
        goldText.text = playerManager.gold.ToString();
    }
}
