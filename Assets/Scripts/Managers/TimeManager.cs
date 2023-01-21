using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public Clock time = new Clock(0);
    public Clock timeEnd {
        get {return new Clock(20,00);}
    }
    public Clock timeStart {
        get {return new Clock(8,00);}
    }
    public Clock minutesPerDay {
        get {return timeEnd - timeStart;}
    }
    float tickCounter = 0;
    public float ticksPerSecond = 1;
    public string timeInString {
        get { 
                int additional = time.hours > 12 ? 1:0;
                string suffix = time.hours > 11 ? "PM":"AM";

                return (time.hours % 13 + additional).ToString() + ":" + time.minutes.ToString("D2") + " " + suffix;
            }
    }

    int pauseCounter = 0;
    bool readyToClose;
    bool closed = true;
    bool paused = false;

    ManagerManager managerManager;
    EventsManager eventsManager;
    NPCManager npcManager;
    UIManager uiManager;
    WorkerManager workerManager;
    PlayerManager playerManager;

    void Awake() {
        managerManager = GameObject.Find("Managers").GetComponent<ManagerManager>();
        eventsManager = managerManager.eventsManager;
        uiManager = managerManager.uiManager;
        workerManager = managerManager.workerManager;
        npcManager = managerManager.npcManager;
        playerManager = managerManager.playerManager;
    }

    void Update () {
        if (!closed && !paused) {
            tickCounter += Time.deltaTime;
            if (time.minutesTotal < timeEnd.minutesTotal) {
                while (tickCounter >= (1/ticksPerSecond)) {
                    tickCounter -= 1/ticksPerSecond;
                    Tick();
                }
            } else {
                readyToClose = true;
            }
            if (readyToClose && npcManager.ReadyToClose) {
                EndDay();
            }
        }
    }

    public void StartDay() {
        closed = false;
        readyToClose = false;
        tickCounter = 0;
        time = timeStart;
        eventsManager.StartDay();
        workerManager.StartDay();
        playerManager.StartDay();
    }

    public void StartDayWithoutCustomers() {
        closed = false;
        readyToClose = false;
        tickCounter = 0;
        time = timeStart;
        workerManager.StartDay();
    }

    public void EndDay() {
        closed = true;
        uiManager.SpawnEndTransitionMenu();
    }

    public void Tick() {
        time += 1;
        uiManager.UpdateTime(timeInString);
        workerManager.Tick();
        eventsManager.Tick();
    }

    public void Resume() {
        pauseCounter--;
        if(pauseCounter == 0) {
            paused = false;
            npcManager.Resume();
        }
    }

    public void Pause() {
        if(pauseCounter == 0) {
            paused = true;
            npcManager.Pause();
        }
        pauseCounter++;
    }
}
