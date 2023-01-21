using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MergeMenu : MonoBehaviour
{
    [SerializeField] Transform content;
    [SerializeField] List<GameObject> workerContents;
    [SerializeField] GameObject itemFrame;
    [SerializeField] List<ItemFrame> mergeFrames;
    [SerializeField] Button mergeButton;
    [SerializeField] List<GameObject> workerCursors;
    Dictionary<string,int> workerIndex = new Dictionary<string, int>() {
        {"Blacksmith",0},{"Armorsmith",1},{"Craftsman",2},{"Sorcerer",3}
    };

    List<GameObject> currentFrames = new List<GameObject>();
    string currentWorker;
    string currentType;
    Rarity currentRarity;
    ProductObject currentProduct;
    Rarity currentProductRarity;

    InventoryManager inventoryManager;
    TimeManager timeManager;
    

    void Awake() {
        ManagerManager mm = FindObjectOfType<ManagerManager>();
        inventoryManager = mm.inventoryManager;
        timeManager = mm.timeManager;
    }

    void Start() {
        timeManager.Pause();
        SetWorker("Blacksmith");
        ClearProduct();
    }

    public void SetWorker(string Worker) {
        currentWorker = Worker;
        for(int i = 0; i < workerContents.Count; i++) {
            if(i == workerIndex[Worker]) {
                workerContents[i].SetActive(true);
                workerCursors[i].SetActive(true);
            } else {
                workerContents[i].SetActive(false);
                workerCursors[i].SetActive(false);
            }
        }
        ListItemsOfType("None");
    }

    public void ListItemsOfType(string Type) {
        currentType = Type == currentType ? "None" : Type; 
        ListItemsOfRarity(Rarity.none);
    } 

    void ListItemsOfRarity(Rarity Rarity) {
        DeleteCurrentFrames();
        currentRarity = Rarity == currentRarity ? Rarity.none : Rarity;

        foreach(ProductObject p in inventoryManager.inventory) {
            if(p.worker != currentWorker) continue;
            if(currentType != "None" && currentType != p.productType) continue;
            if(Rarity == Rarity.none) {
                AddFrame(p,Rarity.unique);
                AddFrame(p,Rarity.rare);
                AddFrame(p,Rarity.common);
            } else {
                AddFrame(p,Rarity);
            }
        }
    }

    void AddFrame(ProductObject p, Rarity Rarity) {
        int quantity = p.inventory[Rarity];
        if(quantity > 0) {
            GameObject go = Instantiate(itemFrame, content);
            currentFrames.Add(go);
            ItemFrame frame = go.GetComponent<ItemFrame>();
            frame.SetProduct(p, Rarity);
            ProductObject prod = p;
            Rarity rar = Rarity;
            go.GetComponent<Button>().onClick.AddListener(()=> SetProduct(prod, rar));
        }
    }

    void DeleteCurrentFrames() {
        foreach(GameObject go in currentFrames) {
            Destroy(go);
        }
        currentFrames.Clear();
    }

    public void SetProduct(ProductObject Product, Rarity Rarity) {
        currentProduct = Product;
        currentProductRarity = Rarity;
        if(Product.inventory[Rarity] < 2) {
            mergeFrames[0].SetProduct(Product,Rarity);
            mergeFrames[1].SetProduct(Rarity);
            mergeFrames[2].SetProduct(Rarity+1);
            mergeButton.interactable = false;
        } else {
            mergeFrames[0].SetProduct(Product,Rarity);
            mergeFrames[1].SetProduct(Product,Rarity);
            mergeFrames[2].SetProduct(Product,Rarity+1);
            mergeButton.interactable = true;
        }
    }

    void ClearProduct() {
        mergeFrames[0].SetProduct(Rarity.common);
        mergeFrames[1].SetProduct(Rarity.common);
        mergeFrames[2].SetProduct(Rarity.common+1);
        mergeButton.interactable = false;
    }

    public void Merge() {
        currentProduct.inventory[currentProductRarity]-=2;
        currentProduct.inventory[currentProductRarity+1]++;
        ListItemsOfRarity(currentRarity);
        SetProduct(currentProduct, currentProductRarity);
    }

    public void CloseMenu() {
        timeManager.Resume();
        Destroy(gameObject);
    }
}
