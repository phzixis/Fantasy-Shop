using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerManager : MonoBehaviour
{
    public Worker blacksmith;
    public Worker armorsmith;
    public Worker craftsman;
    public Worker sorcerer;
    public Dictionary<string,Worker> workerDict;

    void Awake() {
        workerDict = new Dictionary<string, Worker> { 
            {"Blacksmith", blacksmith},
            {"Armorsmith", armorsmith},
            {"Craftsman", craftsman},
            {"Sorcerer", sorcerer}};
    }

    public void Tick() {
        blacksmith.Tick();
        armorsmith.Tick();
        craftsman.Tick();
        sorcerer.Tick();
    }

    public void Craft (ProductObject p) {
        Worker worker = workerDict[p.worker]; 
        Task newTask = new Task(workerDict[p.worker].GetCraftingTime(p.timeToMake), TaskType.craft,p);
        worker.AddTask(newTask);

    }

    public void Craft (FurnitureObject F) {
        Worker worker = workerDict["Craftsman"]; 
        Task newTask = new Task(workerDict["Craftsman"].GetSpecialTime(F.timeToMake), F);
        worker.AddTask(newTask);
        F.isUpgrading = true;

    }

    public void StartDay() {
        foreach(Worker w in workerDict.Values) {
            w.StartDay();
        }
    }

    public void Research(ProductObject p) {
        Worker worker = workerDict[p.worker]; 
        Task newTask = new Task(workerDict[p.worker].GetResearchTime(p.timeToMake), TaskType.research,p);
        worker.AddTask(newTask);
    }

    public void AddStats(string Worker, int StatsID, int Amount) {
        workerDict[Worker].AddStats(StatsID, Amount);
        if(StatsID == 0) {
            if(!workerDict[Worker].unlocked) {
                workerDict[Worker].Unlock();
            }
        }
    }

    public void LoadStats(float[] b, float[] a, float[] c, float[] s) {
        blacksmith.LoadStats(b);
        armorsmith.LoadStats(a);
        craftsman.LoadStats(c);
        sorcerer.LoadStats(s);  
    }

    public void LoadTasks(int[,] b, int[,] a, int[,] c, int[,] s) {
        blacksmith.LoadTasks(b);
        armorsmith.LoadTasks(a);
        craftsman.LoadTasks(c);
        sorcerer.LoadTasks(s); 
    }
}
