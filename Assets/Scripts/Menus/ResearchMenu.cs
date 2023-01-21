using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchMenu : MonoBehaviour
{
    [SerializeField] GameObject itemBox;
    [SerializeField] Transform contentParent;
    [SerializeField] GameObject detailBox;
    ProductObject currentProduct;
    RecipeManager recipeManager;
    WorkerManager workerManager;
    string currentWorker;

    List<GameObject> itemBoxes = new List<GameObject>();

    void Awake() {
        recipeManager = GameObject.Find("RecipeManager").GetComponent<RecipeManager>();
        workerManager = GameObject.Find("WorkerManager").GetComponent<WorkerManager>();
    }

    void Start() {
        CreateItemBoxesOfWorker("Blacksmith");
    }


    void CreateItemBoxes() {
        ClearItemBoxes();
        SetDetailBoxItem(recipeManager.canResearchRecipes[0]);
        foreach (ProductObject p in recipeManager.canResearchRecipes) {
            GameObject newIB = Instantiate(itemBox, transform.position,transform.rotation,contentParent);
            newIB.GetComponent<MenuDirectory>().image.sprite = p.img;
            newIB.GetComponent<Button>().onClick.AddListener(() => {SetDetailBoxItem(p);});
            if (p.quantityToAdd > 1) {
                newIB.GetComponent<MenuDirectory>().tmiText.text = p.quantityToAdd.ToString();
            }
            itemBoxes.Add(newIB);
        }
    }

    public void CreateItemBoxesOfWorker(string worker) {
        currentWorker = worker;
        List<string> productTypes;
        ClearItemBoxes();
        switch (worker) {
            case "Armorsmith":
                productTypes = new List<string>(new string[] {"Helmet", "Chestplate", "Shoulderplate", "Pants", "Boots", "Gloves", "Accessory"});
                break;
            case "Blacksmith":
                productTypes = new List<string>(new string[] {"Sword", "Shield", "Dagger", "Axe", "Hammer", "Spear"});
                break;
            case "Craftsman":
                productTypes = new List<string>(new string[] {"Bow", "Crossbow", "Gun", "Arrows", "Bolts", "Bullets"});
                break;
            case "Sorcerer":
                productTypes = new List<string>(new string[] {"Book", "Staff", "Wand", "Potion"});
                break;
            default:
                productTypes = new List<string>();
                break;
        }
        
        ProductObject first = null;     
        foreach (string type in productTypes) {
            foreach (ProductObject p in recipeManager.GetUnknownRecipesOfType(type)) {
                if (first == null) first = p; 
                GameObject newIB = Instantiate(itemBox, transform.position,transform.rotation,contentParent);
                newIB.GetComponent<MenuDirectory>().image.sprite = p.img;
                newIB.GetComponent<Button>().onClick.AddListener(() => {SetDetailBoxItem(p);});
                itemBoxes.Add(newIB);
            }
        }
        if (first != null) SetDetailBoxItem(first);
    }

    void ClearItemBoxes() {
        foreach (GameObject g in itemBoxes) {
            Destroy(g);
        }
        itemBoxes.Clear();
    }

    void SetDetailBoxItem(ProductObject product) {
        MenuDirectory dbDirect = detailBox.GetComponent<MenuDirectory>();
        dbDirect.image.sprite = product.img;
        dbDirect.tmiText.text = product.productName;
        currentProduct = product;
    }
    
    public void Research() {
        recipeManager.RemoveForResearch(currentProduct);
        workerManager.Research(currentProduct);
        CloseMenu();
    }

    public void CloseMenu() {
        Destroy(gameObject);
    }
}
