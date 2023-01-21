using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGetListener : MonoBehaviour
{
    public string[] commands;
    public ProductObject item;
    public GameObject npc;
    public int x; 
    public Dialogue dialogue;

    ManagerManager managerManager;
    CraftingMenu craftingMenu;
    BuyingMenu buyingMenu;

    void Awake() {
        managerManager = FindObjectOfType<ManagerManager>();
    }

    void Start() {
        Button button = GetComponent<Button>();
        foreach(string command in commands){
            switch (command) {
                case "Craft Menu":
                    button.onClick.AddListener(()=>managerManager.uiManager.OpenCraftingMenu("Blacksmith"));
                    break;
                case "End Dialogue":
                    button.onClick.AddListener(()=>managerManager.dialogueManager.CloseDialogue());
                    break;
                case "Destroy Parent":
                    button.onClick.AddListener(()=>Destroy(transform.parent.gameObject));
                    break;
                case "Destroy Parent 2":
                    button.onClick.AddListener(()=>Destroy(transform.parent.parent.gameObject));
                    break;
                case "Set Item Detail":
                    craftingMenu = FindObjectOfType<CraftingMenu>();
                    button.onClick.AddListener(()=>craftingMenu.SetDetailBoxItem(item));
                    break;
                case "Craft":
                    craftingMenu = FindObjectOfType<CraftingMenu>();
                    button.onClick.AddListener(()=>craftingMenu.Craft());
                    break;
                case "Research":
                    craftingMenu = FindObjectOfType<CraftingMenu>();
                    button.onClick.AddListener(()=>craftingMenu.Research());
                    break;
                case "Switch To Craftsman":
                    craftingMenu = FindObjectOfType<CraftingMenu>();
                    button.onClick.AddListener(()=>craftingMenu.CreateItemBoxesOfWorker("Craftsman"));
                    break;
                case "Set Crafting Mode":
                    craftingMenu = FindObjectOfType<CraftingMenu>();
                    button.onClick.AddListener(()=>craftingMenu.SetMode(x));
                    break;
                case "Close Crafting Menu":
                    craftingMenu = FindObjectOfType<CraftingMenu>();
                    button.onClick.AddListener(()=>craftingMenu.CloseMenu());
                    break;
                case "Spawn NPC":
                    button.onClick.AddListener(()=>managerManager.eventsManager.AddSpecialNPCSpawn(x, npc));
                    break;
                case "Start Dialogue":
                    button.onClick.AddListener(()=>managerManager.dialogueManager.StartDialogue(dialogue));
                    break;
                case "Close Buying Menu":
                    buyingMenu = FindObjectOfType<BuyingMenu>();
                    button.onClick.AddListener(()=>buyingMenu.CloseMenu());
                    break;
                case "Sell":
                    buyingMenu = FindObjectOfType<BuyingMenu>();
                    button.onClick.AddListener(()=>buyingMenu.Sell());
                    break;
                case "Haggle":
                    buyingMenu = FindObjectOfType<BuyingMenu>();
                    button.onClick.AddListener(()=>buyingMenu.Haggle());
                    break;
                case "Return Later":
                    buyingMenu = FindObjectOfType<BuyingMenu>();
                    button.onClick.AddListener(()=>buyingMenu.Return());
                    break;
                case "Refuse":
                    buyingMenu = FindObjectOfType<BuyingMenu>();
                    button.onClick.AddListener(()=>buyingMenu.Refuse());
                    break;
            }
        }
    }
}
