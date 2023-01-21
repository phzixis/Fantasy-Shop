using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{
    [SerializeField] GameObject itemBox;
    [SerializeField] Transform contentParent;
    [SerializeField] GameObject detailBox;
    ManagerManager managerManager;
    InventoryManager inventoryManager;
    TimeManager timeManager;

    List<GameObject> itemBoxes = new List<GameObject>();

    void Awake() {
        managerManager = FindObjectOfType<ManagerManager>();
        inventoryManager = managerManager.inventoryManager;
        timeManager = managerManager.timeManager;
    }

    void Start() {
        CreateItemBoxes();
        timeManager.Pause();
    }

    void CreateItemBoxes() {
        SetDetailBoxItem(inventoryManager.inventory[0]);
        foreach (ProductObject p in inventoryManager.inventory) {
            GameObject newIB = Instantiate(itemBox,contentParent);
            newIB.GetComponent<MenuDirectory>().image.sprite = p.img;
            newIB.GetComponent<Button>().onClick.AddListener(() => {SetDetailBoxItem(p);});
            if (p.quantity > 1) {
                newIB.GetComponent<MenuDirectory>().tmiText.text = p.quantity.ToString();
            }
            itemBoxes.Add(newIB);
        }
    }

    void CreateItemBoxesOfType(string type) {
        foreach (ProductObject p in inventoryManager.GetProductsOfType(type)) {
            GameObject newIB = Instantiate(itemBox,contentParent);
            newIB.GetComponent<MenuDirectory>().image.sprite = p.img;
            newIB.GetComponent<Button>().onClick.AddListener(() => {SetDetailBoxItem(p);});
            itemBoxes.Add(newIB);
        }
    }

    void ClearItemBoxes() {
        foreach (GameObject g in itemBoxes) {
            Destroy(g);
        }
    }

    void SetDetailBoxItem(ProductObject product) {
        MenuDirectory dbDirect = detailBox.GetComponent<MenuDirectory>();
        dbDirect.image.sprite = product.img;
        dbDirect.tmiText.text = product.productName;
    }

    public void CloseMenu() {
        timeManager.Resume();
        Destroy(gameObject);
    }
}
