using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialUpgradeMenu : MonoBehaviour
{
    MenuDirectory menuDirectory;
    MaterialObject material;
    ManagerManager managerManager;

    void Awake() {
        menuDirectory = GetComponent<MenuDirectory>();
        managerManager = FindObjectOfType<ManagerManager>();
    }

    void Start() {
        managerManager.timeManager.Pause();
    }
 
    public void SetMaterial(MaterialObject Material) {
        material = Material;
        UpdateInfo();
    }

    void UpdateInfo() {
        menuDirectory.image.sprite = material.img;
        menuDirectory.tmiText.text = material.materialName;
        menuDirectory.tmiText2.text = "Max Storage: " + material.maxQuantity.ToString();
        menuDirectory.tmiText3.text = "Currently have: " + material.quantity.ToString();
        menuDirectory.tmiText4.text = "<sprite=0>" + material.upgradeCost;
        menuDirectory.go.GetComponent<Button>().interactable = managerManager.playerManager.CheckGold(material.upgradeCost);
    }

    public void Upgrade() {
        managerManager.playerManager.SpendGold(material.upgradeCost);
        material.maxQuantity++;
        UpdateInfo();
    }

    public void CloseMenu() {
        managerManager.timeManager.Resume();
        Destroy(gameObject);
    }
}
