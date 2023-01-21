using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

enum CraftMode {
    Craft = 0,
    Research = 1,
    Special = 2
}

public class CraftingMenu : MonoBehaviour
{
    [SerializeField] TabDirectory mainTab;
    [SerializeField] GameObject itemBox;
    [SerializeField] Transform contentParent;
    [SerializeField] Transform contentParentResearch;
    Transform currentParent;
    [SerializeField] GameObject detailBox;
    [SerializeField] Transform resourcesContentParent;
    [SerializeField] GameObject resourceItemObject;
    [SerializeField] GameObject timeDetail;
    [SerializeField] TextMeshProUGUI craftingMessage;
    [SerializeField] RectTransform topWorkerButton;
    [SerializeField] List<GameObject> workerButtons;
    const int workerButtonGap = 75;
    const int workerButtonSelect = 35; 
    ManagerManager managerManager;
    TimeManager timeManager;
    ProductObject currentProduct;
    InventoryManager inventoryManager;
    RecipeManager recipeManager;
    WorkerManager workerManager;

    [SerializeField] GameObject blacksmithTab;
    [SerializeField] GameObject armorsmithTab;
    [SerializeField] GameObject craftsmanTab;
    [SerializeField] GameObject sorcererTab;
    string currentWorker;
    GameObject currentTab;
    List<GameObject> currentTabsList;
    List<Button> currentTabButtons;
    List<string> productTypes;

    List<GameObject> itemBoxes = new List<GameObject>();
    List<GameObject> resourceObjectsList = new List<GameObject>();

    [SerializeField] Button craftButton;
    [SerializeField] Button researchButton;
    CraftMode mode;


    void Awake() {
        managerManager = GameObject.Find("Managers").GetComponent<ManagerManager>();
        inventoryManager = managerManager.inventoryManager;
        recipeManager = managerManager.recipeManager;
        workerManager = managerManager.workerManager;
        timeManager = managerManager.timeManager;
    }

    void Start() {
        timeManager.Pause();
        SetMode((int) CraftMode.Craft);
        AlignWorkerButtons();
    }

    void Update() {
        SetDetailBoxItem(currentProduct);
    }

    void AlignWorkerButtons() {
        for (int i = 0; i < 4; i++) {
            workerButtons[i].SetActive(true);
            workerButtons[i].GetComponent<RectTransform>().anchoredPosition = topWorkerButton.anchoredPosition + (Vector2.down * workerButtonGap * i);
        }
        if (!workerManager.workerDict["Craftsman"].unlocked) {
            workerButtons[2].SetActive(false);
        } 
        if (!workerManager.workerDict["Armorsmith"].unlocked) {
            workerButtons[1].SetActive(false);
            workerButtons[2].GetComponent<RectTransform>().anchoredPosition += Vector2.up * workerButtonGap;
            workerButtons[3].GetComponent<RectTransform>().anchoredPosition += Vector2.up * workerButtonGap * 2;
        }
        if (!workerManager.workerDict["Sorcerer"].unlocked) {
            workerButtons[3].SetActive(false);
        }
        switch (currentWorker) {
            case "Blacksmith":
                workerButtons[0].GetComponent<RectTransform>().anchoredPosition += Vector2.left * workerButtonSelect;
                break;
            case "Armorsmith":
                workerButtons[1].GetComponent<RectTransform>().anchoredPosition += Vector2.left * workerButtonSelect;
                break;
            case "Craftsman":
                workerButtons[2].GetComponent<RectTransform>().anchoredPosition += Vector2.left * workerButtonSelect;
                break;
            case "Sorcerer":
                workerButtons[3].GetComponent<RectTransform>().anchoredPosition += Vector2.left * workerButtonSelect;
                break;
        }
    }

    void CreateItemBoxes() {
        ClearItemBoxes();
        SetDetailBoxItem(recipeManager.unlockedRecipes[0]);
        foreach (ProductObject p in recipeManager.unlockedRecipes) {
            GameObject newIB = Instantiate(itemBox, transform.position,transform.rotation,currentParent);
            newIB.GetComponent<MenuDirectory>().image.sprite = p.img;
            newIB.GetComponent<Button>().onClick.AddListener(() => {SetDetailBoxItem(p);});
            if (p.quantity > 1) {
                newIB.GetComponent<MenuDirectory>().tmiText.text = p.quantity.ToString();
            }
            itemBoxes.Add(newIB);
        }
    }

    public void CreateItemBoxesOfType(string type) {
        ClearItemBoxes();
        foreach (ProductObject p in recipeManager.GetRecipesOfType(type)) {
            GameObject newIB = Instantiate(itemBox, transform.position,transform.rotation,currentParent);
            newIB.GetComponent<MenuDirectory>().image.sprite = p.img;
            newIB.GetComponent<Button>().onClick.AddListener(() => {SetDetailBoxItem(p);});
            itemBoxes.Add(newIB);
        }
    }

    public void CreateItemBoxesOfWorker(string worker) {
        ClearItemBoxes();
        Destroy(currentTab);
        currentWorker = worker;
        GameObject tabToMake = null;
        switch (worker) {
            case "Armorsmith":
                tabToMake = armorsmithTab;
                productTypes = new List<string>(new string[] {"Helmet", "Chestplate", "Shoulderplate", "Pants", "Boots", "Gloves", "Accessory"});
                break;
            case "Blacksmith":
                tabToMake = blacksmithTab;
                productTypes = new List<string>(new string[] {"Sword", "Shield", "Dagger", "Axe", "Hammer", "Spear"});
                break;
            case "Craftsman":
                tabToMake = craftsmanTab;
                productTypes = new List<string>(new string[] {"Bow", "Crossbow", "Gun", "Arrows", "Bolts", "Bullets"});
                break;
            case "Sorcerer":
                tabToMake = sorcererTab;
               productTypes = new List<string>(new string[] {"Book", "Staff", "Wand", "Potion"});
                break;
            default:
                productTypes = new List<string>();
                break;
        }
        switch (mode) {
            case CraftMode.Craft:
                currentTab = Instantiate(tabToMake, transform, false);
                currentTabsList = currentTab.GetComponent<TabDirectory>().tabs;
                currentTabButtons = currentTab.GetComponent<TabDirectory>().buttons;

                for(int i = 0; i < currentTabButtons.Count; i++) {
                    int temp = i;
                    currentTabButtons[i].onClick.AddListener(()=>SetTab(temp));
                }
                SetTab(0);
                break;
            case CraftMode.Research:
                ProductObject first = null;     
                foreach (string type in productTypes) {
                    foreach (ProductObject p in recipeManager.GetUnknownRecipesOfType(type)) {
                        if (first == null) first = p; 
                        GameObject newIB = Instantiate(itemBox, transform.position,transform.rotation,currentParent);
                        newIB.GetComponent<MenuDirectory>().image.sprite = p.img;
                        newIB.GetComponent<Button>().onClick.AddListener(() => {SetDetailBoxItem(p);});
                        itemBoxes.Add(newIB);
                    }
                }
                if (first != null) SetDetailBoxItem(first);
                break;
        }
        AlignWorkerButtons();
    }

    void ClearItemBoxes() {
        foreach (GameObject g in itemBoxes) {
            Destroy(g);
        }
        itemBoxes.Clear();
    }

    public void SetDetailBoxItem(ProductObject product) {
        MenuDirectory dbDirect = detailBox.GetComponent<MenuDirectory>();
        dbDirect.image.sprite = product.img;
        dbDirect.tmiText.text = product.productName;
        Clock timeToMake = new Clock(0);
        currentProduct = product;
        switch (mode) {
            case CraftMode.Craft:
                foreach(GameObject g in resourceObjectsList) {
                    Destroy(g);
                }

                bool canCraft = true;
                string message0 = "";
                resourceObjectsList.Clear();
                if (!workerManager.workerDict[product.worker].canTakeWork) {
                    canCraft = false;
                    message0 = "Worker queue full!";
                }

                for (int i = 0; i < product.materialName.Count; i++) {
                    GameObject newBox = Instantiate(resourceItemObject,Vector3.zero, transform.rotation, resourcesContentParent);
                    newBox.GetComponent<MenuDirectory>().image.sprite = inventoryManager.materialDict[product.materialName[i]].img;
                    newBox.GetComponent<MenuDirectory>().tmiText.text = "x" + product.materialQuantity[i].ToString();
                    resourceObjectsList.Add(newBox);

                    if (!inventoryManager.HasMaterial(product.materialName[i], product.materialQuantity[i])) {
                        canCraft = false;
                        newBox.GetComponent<MenuDirectory>().tmiText.color = Color.red;
                        message0 = "Insufficient materials.";
                    }
                }
                craftingMessage.text = message0;
                craftButton.interactable = canCraft;   
                timeToMake = workerManager.workerDict[currentProduct.worker].GetCraftingTime(product.timeToMake);
                break;
            case CraftMode.Research:
                foreach(GameObject g in resourceObjectsList) {
                    Destroy(g);
                }
                bool canResearch = true;
                string message1 = "";
                if (!workerManager.workerDict[product.worker].canTakeWork) {
                    canResearch = false;
                    message1 = "Worker queue full!";
                }
                craftingMessage.text = message1;
                researchButton.interactable = canResearch;
                timeToMake = workerManager.workerDict[currentProduct.worker].GetResearchTime(product.timeToMake);
                break;
        }
        timeDetail.GetComponent<MenuDirectory>().tmiText.text = timeToMake.hours.ToString() + ":" + timeToMake.minutes.ToString("D2");
    }

    void SetTab(int t) {
        ClearItemBoxes();
        for (int i = 0; i < currentTabsList.Count; i++) {
            if (i == t) {
                currentTabsList[i].SetActive(true);
            } else {
                currentTabsList[i].SetActive(false);
            }
        }
        ProductObject first = null;     
        if(t == 0) {   
            foreach (string type in productTypes) {
                foreach (ProductObject p in recipeManager.GetRecipesOfType(type)) {
                    if (first == null) first = p; 
                    CreateCraftingItemBox(p);
                }
            }
        } else {
            foreach (ProductObject p in recipeManager.GetRecipesOfType(productTypes[t-1])) {
                if (first == null) first = p;
                CreateCraftingItemBox(p);
            }
        }
        if (first != null) SetDetailBoxItem(first);
    }

    void CreateCraftingItemBox(ProductObject p) {
        GameObject newIB = Instantiate(itemBox, transform.position,transform.rotation,currentParent);
        newIB.GetComponent<MenuDirectory>().image.sprite = p.img;
        newIB.GetComponent<Button>().onClick.AddListener(() => {SetDetailBoxItem(p);});
        ProductObject existing = inventoryManager.SearchProduct(p);
        if(existing != null) {
            if (existing.quantity > 0) {
                newIB.GetComponent<MenuDirectory>().tmiText.text = existing.quantity.ToString();
            }
        }
        itemBoxes.Add(newIB);
    }
    
    public void Craft() {
        switch (mode) {
            case CraftMode.Craft:
                for (int i = 0; i < currentProduct.materialName.Count; i++) {
                    inventoryManager.RemoveMaterial(currentProduct.materialName[i], currentProduct.materialQuantity[i]);
                }
                workerManager.Craft(currentProduct);
                SetDetailBoxItem(currentProduct);
                break;
            case CraftMode.Research:
                recipeManager.RemoveForResearch(currentProduct);
                workerManager.Research(currentProduct);
                CreateItemBoxesOfWorker(currentWorker);
                break;
        }
    }

    public void Research() {
        recipeManager.RemoveForResearch(currentProduct);
        workerManager.Research(currentProduct);
    }

    public void Special() {
        
    }

    public void SetMode(int Mode) {
        mode = (CraftMode) Mode;
        if (Mode == 0) {
            currentParent = contentParent;
            craftButton.gameObject.SetActive(true);
            researchButton.gameObject.SetActive(false);
            contentParent.parent.gameObject.SetActive(true);
            contentParentResearch.parent.gameObject.SetActive(false);
        } else if (Mode == 1) {
            currentParent = contentParentResearch;
            craftButton.gameObject.SetActive(false);
            researchButton.gameObject.SetActive(true);
            contentParent.parent.gameObject.SetActive(false);
            contentParentResearch.parent.gameObject.SetActive(true);
        } else if (Mode == 2) {
            currentParent = contentParent;
            craftButton.gameObject.SetActive(false);
            researchButton.gameObject.SetActive(false);
            contentParent.parent.gameObject.SetActive(false);
            contentParentResearch.parent.gameObject.SetActive(false);
        }
        CreateItemBoxesOfWorker(currentWorker);
        for(int i = 0; i < 3; i++) {
            if (i == Mode) {
                mainTab.tabs[i].SetActive(true);
            } else {
                mainTab.tabs[i].SetActive(false);
            }
        }
    }

    public void CloseMenu() {
        Destroy(gameObject);
        timeManager.Resume();
    }
}
