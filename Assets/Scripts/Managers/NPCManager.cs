using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    [SerializeField] List<CustomerObject> customerObjects;
    [SerializeField] List<Transform> spawnLocations;

    const float xOffset = 1;
    const float yOffset = -1;

    public List<NPCScript> npcList = new List<NPCScript>();
    public List<Transform> frontOfCounter;
    public int customerCount = 0;
    public bool ReadyToClose { get {return customerCount <= 0;}}

    Vector3 FindOpenPosition() {
        int x = 0, y = 0;
        for(int i = 0; i < 100; i++) {
            x = Random.Range(0, (int)(frontOfCounter[0].position.x-frontOfCounter[1].position.x)+1);
            y = Random.Range(0, (int)(frontOfCounter[0].position.y-frontOfCounter[1].position.y)+1);
            RaycastHit2D hit = Physics2D.Raycast(frontOfCounter[1].position + new Vector3(x,y,0), -Vector2.up, .4f);
            if (hit.collider != null) {
                if(hit.collider.CompareTag("NPC")) continue;
            }
            break;
        }
        return new Vector3(x - xOffset,y - yOffset);
    }

    public void SpawnNPC(CustomerType type) {
        switch (type) {
            case CustomerType.buy:
                Vector3 pos = FindOpenPosition();
                int c = Random.Range(0,customerObjects.Count);
                int s = Random.Range(0,spawnLocations.Count);
                GameObject go = Instantiate(customerObjects[c].prefab, spawnLocations[s].position, transform.rotation);
                NPCScript n = go.GetComponent<NPCScript>();
                n.AddIntention(Intention.move, frontOfCounter[1].position + pos);
                n.AddIntention(Intention.buy, Vector2.zero);
                n.npcManager = this;
                npcList.Add(n);
                customerCount++;
                break;
            default:
                break;
        }
    }

    public void SpawnNPC(CustomerClass type) {
        Vector3 pos = FindOpenPosition();
        int s = Random.Range(0,spawnLocations.Count);
        GameObject go = Instantiate(customerObjects[(int)type].prefab, spawnLocations[s].position, transform.rotation);
        NPCScript n = go.GetComponent<NPCScript>();
        n.AddIntention(Intention.move, frontOfCounter[1].position + pos);
        n.AddIntention(Intention.buy, Vector2.zero);
        n.npcManager = this;
        npcList.Add(n);
        customerCount++;
    }

    public void SpawnNPC(GameObject customer) {
        Vector3 pos = FindOpenPosition();
        int s = Random.Range(0,spawnLocations.Count);
        GameObject go = Instantiate(customer, spawnLocations[s].position, transform.rotation);
        NPCScript n = go.GetComponent<NPCScript>();
        n.AddIntention(Intention.move, frontOfCounter[1].position + pos);
        n.npcManager = this;
        npcList.Add(n);
        customerCount++;
    }

    public void AddNPC(CustomerObject customer) {
        customerObjects.Add(customer);
    }

    public void Pause() {
        foreach (NPCScript n in npcList) {
            n.GetComponent<NPCMovementScript>().Pause();
        }
    }

    public void Resume() {
        foreach (NPCScript n in npcList) {
            n.GetComponent<NPCMovementScript>().Resume();
        }
    }
}
