using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    const int maxPopularity = 150;
    const int minCustomerLower = 6;
    const int minCustomerTop = 8;
    const int maxCustomerLower = 20;
    const int maxCustomerTop = 30;

    const int minPasserbyLower = 2;
    const int minPasserbyTop = 4;
    const int maxPasserbyLower = 40;
    const int maxPasserbyTop = 60; 

    int minutesPerDay;
    List<(int,CustomerType)> customerList = new List<(int, CustomerType)>();
    List<(int,CustomerClass)> classList = new List<(int,CustomerClass)>();
    List<(int,GameObject)> specialNPCList = new List<(int, GameObject)>();

    public List<ProductObject> productsMadeToday = new List<ProductObject>();
    public List<IndividualProductObject> productsSoldToday= new List<IndividualProductObject>();
    int[] expenses = new int[4];

    ManagerManager managerManager;
    PlayerManager playerManager;
    TimeManager timeManager;
    NPCManager npcManager;

    void Awake() {
        managerManager = GameObject.Find("Managers").GetComponent<ManagerManager>();
        playerManager = managerManager.playerManager;
        timeManager = managerManager.timeManager;
        npcManager = managerManager.npcManager;
        minutesPerDay = (timeManager.timeEnd - timeManager.timeStart).minutesTotal;
    }

    Vector2Int GetCustomerBounds(float ratio) {
        int customerLower = Mathf.FloorToInt(minCustomerLower + (maxCustomerLower - minCustomerLower) * ratio);
        int customerTop = Mathf.FloorToInt(minCustomerTop + (maxCustomerTop - minCustomerTop) * ratio);
        return new Vector2Int(customerLower,customerTop);
    }

    Vector2Int GetPasserbyBounds(float ratio) {
        int passerbyLower = Mathf.FloorToInt(minPasserbyLower + (maxPasserbyLower - minPasserbyLower) * ratio);
        int passerbyTop = Mathf.FloorToInt(minPasserbyTop + (maxPasserbyTop - minPasserbyTop) * ratio);
        return new Vector2Int(passerbyLower,passerbyTop);
    }

    public void StartDay() {
        customerList.Clear();
        specialNPCList.Clear();
        productsMadeToday.Clear();
        productsSoldToday.Clear();
        for(int i = 0; i < expenses.Length; i++) {
            expenses[i] = 0;
        }
        if (playerManager.popularity > maxPopularity) {
            playerManager.popularity = maxPopularity;
        }
        float ratio = (float) playerManager.popularity/maxPopularity;

        Vector2Int customerBounds = GetCustomerBounds(ratio);
        int customerNum = Random.Range(customerBounds.x,customerBounds.y+1);
        for(int i = 0; i < customerNum; i++) {
            int timeToSpawn = Random.Range(0, minutesPerDay+1-60) + timeManager.timeStart.minutesTotal;
            customerList.Add((timeToSpawn, CustomerType.buy));
        }

        Vector2Int passerBounds = GetPasserbyBounds(ratio);
        int passerNum = Random.Range(passerBounds.x,passerBounds.y+1);

        for(int i = 0; i < passerNum; i++) {
            int timeToSpawn = Random.Range(0, minutesPerDay+1-30) + timeManager.timeStart.minutesTotal;
            customerList.Add((timeToSpawn, CustomerType.pass));
        }

        customerList.Sort((a, b) => a.Item1.CompareTo(b.Item1));
    }

    public void AddSpecialNPCSpawn(int time, GameObject npc) {
        specialNPCList.Add((time, npc));
    }
    public void AddCustomerSpawn(int time, CustomerClass npc) {
        classList.Add((time, npc));
    }

    public void Tick() {
        while(customerList.Count > 0) {
            if(customerList[0].Item1 <= timeManager.time.minutesTotal) {
                npcManager.SpawnNPC(customerList[0].Item2);
                customerList.RemoveAt(0);
            } else {
                break;
            }
        }
        while(specialNPCList.Count > 0) {
            if(specialNPCList[0].Item1 <= timeManager.time.minutesTotal) {
                npcManager.SpawnNPC(specialNPCList[0].Item2);
                specialNPCList.RemoveAt(0);
            } else {
                break;
            }
        }
        while(classList.Count > 0) {
            if(classList[0].Item1 <= timeManager.time.minutesTotal) {
                npcManager.SpawnNPC(classList[0].Item2);
                classList.RemoveAt(0);
            } else {
                break;
            }
        }
    }
}
