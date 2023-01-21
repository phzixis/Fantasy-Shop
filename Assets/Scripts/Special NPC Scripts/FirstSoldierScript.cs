using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FirstSoldierScript : MonoBehaviour
{
    public Dialogue d0;
    public Dialogue d1;

    bool doneD0 = false;
    bool doneD1 = false;

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
            dialogueManager.StartDialogue(d0);
            doneD0 = true;
        }
    }

    void OnMouseDown() {
        if(!EventSystem.current.IsPointerOverGameObject()) {
            if(npcScript.clickState == ClickState.buy) {
                BuyingMenu buyMenu = FindObjectOfType<BuyingMenu>();
                buyMenu.suggestButton.interactable = false;
                if(!doneD1) {
                    doneD1 = true;
                    dialogueManager.StartDialogue(d1);
                }
            }
        }
    }
}
