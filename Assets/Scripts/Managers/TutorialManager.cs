using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public Dialogue tutorialStartDialogue;
    public List<GameObject> npcSpawns;
    public List<int> npcSpawnTimes;
    public List<CustomerClass> customerSpawns;
    public List<int> customerSpawnTimes;
    ManagerManager managerManager;

    void Awake() {
        managerManager = FindObjectOfType<ManagerManager>();
    }
    void Start() {
        FindObjectOfType<DialogueManager>().StartDialogue(tutorialStartDialogue);
        for(int i = 0; i < npcSpawns.Count; i++) {
            managerManager.eventsManager.AddSpecialNPCSpawn(npcSpawnTimes[i], npcSpawns[i]);
        }
        for(int i = 0; i < customerSpawns.Count; i++) {
            managerManager.eventsManager.AddCustomerSpawn(customerSpawnTimes[i], customerSpawns[i]);
        }
    }
}
