using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialButtonScript : MonoBehaviour
{
    public string materialID;
    public GameObject materialUpgradeMenu;
    public Canvas canvas;
    MaterialObject material;

    MenuDirectory menuDirectory;

    void Awake() {
        menuDirectory = GetComponent<MenuDirectory>();
    }

    void Start() {
        material = FindObjectOfType<InventoryManager>().materialDict[materialID];
        
        menuDirectory.image.sprite = material.img;
        menuDirectory.tmiText.text = material.quantity.ToString();

        GetComponent<Button>().onClick.AddListener(()=>CreateMaterialUpgradeMenu());
    }

    void Update() {
        if (material.quantity == 0) {
            menuDirectory.image.gameObject.SetActive(false);
            menuDirectory.tmiText.gameObject.SetActive(false);
        } else {
            menuDirectory.tmiText.text = material.quantity.ToString();
            menuDirectory.image.gameObject.SetActive(true);
            menuDirectory.tmiText.gameObject.SetActive(true);
        }
    }

    public void CreateMaterialUpgradeMenu() {
        GameObject menu = Instantiate(materialUpgradeMenu, canvas.transform);
        menu.GetComponent<MaterialUpgradeMenu>().SetMaterial(material);
    }
}
