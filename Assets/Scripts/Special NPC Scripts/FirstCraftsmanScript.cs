using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstCraftsmanScript : MonoBehaviour
{
    public Dialogue d0;
    bool doneD0 = false;

    NPCScript npcScript;
    DialogueManager dialogueManager;

    void Awake() {
        npcScript = GetComponent<NPCScript>();
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    void Start() {
        npcScript.AddIntention(Intention.wait, Vector2.right);
        npcScript.AddIntention(Intention.move, new Vector2(-16.5f,27.5f));
        FindObjectOfType<NPCManager>().customerCount--;
    }

    void Update() {
        if(npcScript.currentIntention.intention == Intention.wait && !doneD0) {
            FindObjectOfType<WorkerManager>().workerDict["Craftsman"].Unlock();
            dialogueManager.StartDialogue(d0);
            doneD0 = true;
        }
    }

}
