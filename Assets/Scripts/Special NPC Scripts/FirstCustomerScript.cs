using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FirstCustomerScript : MonoBehaviour
{
    public Dialogue dialogue0;
    public Dialogue dialogue1;
    public Dialogue dialogue2;
    public Dialogue dialogue3;

    bool doneD0 = false;
    bool doneD1 = false;
    bool doneD2 = false;
    NPCScript npcScript;
    DialogueManager dialogueManager;
    InventoryManager inventoryManager;

    void Awake() {
        npcScript = GetComponent<NPCScript>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    void Start() {
        npcScript.AddIntention(Intention.buy, Vector2.zero);
    }

    void Update() {
        if(npcScript.clickState == ClickState.buy && !doneD0) {
            dialogueManager.StartDialogue(dialogue0);
            doneD0 = true;
        }
        if(inventoryManager.inventory.Count > 0 && !doneD2) {
            dialogueManager.StartDialogue(dialogue2);
            doneD2 = true;
        }
    }

    void OnMouseDown() {
        if(!EventSystem.current.IsPointerOverGameObject()) {
            if(npcScript.clickState == ClickState.buy) {
                BuyingMenu buyMenu= FindObjectOfType<BuyingMenu>();
                buyMenu.suggestButton.interactable = false;
                buyMenu.refuseButton.interactable = false;
                buyMenu.haggleButton.interactable = false;
                buyMenu.returnButton.interactable = false;
                if(!doneD1) {
                    doneD1 = true;
                    dialogueManager.StartDialogue(dialogue1);
                } else if (doneD2) {
                    dialogueManager.StartDialogue(dialogue3);
                }
            }
        }
    }
}
