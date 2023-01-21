using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Worker : MonoBehaviour
{
    public string workerName;
    public string job;
    public int level;
    public float craftingSpeed;
    public float researchSpeed;
    public float specialSpeed;
    public int salary;
    public int queueLimit;
    public int exp;
    public int startExpToday;
    public int expToday;
    public int lvlToday;

    public ProgressIcon progressIcon;
    public bool unlocked;
    public bool unlockedSpecial;
    public string specialName;
    public Sprite sprite;

    Task currentTask;
    public List<Task> taskQueue = new List<Task>(); 
    public List<GameObject> itemsToUnlock = new List<GameObject>(); 
    public List<ProductObject> recipesToUnlock = new List<ProductObject>();

    public bool canTakeWork {
        get { return taskQueue.Count < queueLimit; }
    }

    const float craftingIncrecment = .1f;
    const float reserachIncrement = .15f;
    const float specialIncrement = .15f;

    ManagerManager managerManager;
    TimeManager timeManager;
    void Awake() {
        managerManager = FindObjectOfType<ManagerManager>();
        timeManager = managerManager.timeManager;
    }

    public float[] SaveStats() {
        float[] stats = new float[8];
        stats[0] = level;
        stats[1] = craftingSpeed;
        stats[2] = researchSpeed;
        stats[3] = specialSpeed;
        stats[4] = salary;
        stats[5] = queueLimit;
        stats[6] = exp;
        stats[7] = unlocked ? 1 : 0;
        return stats;
    }

    public void LoadStats(float[] stats) {
        level = (int) stats[0];
        craftingSpeed = stats[1];
        researchSpeed = stats[2];
        specialSpeed = stats[3];
        salary = (int) stats[4];
        queueLimit = (int) stats[5];
        exp = (int) stats[6];
        if(stats[7] == 1) {
            Unlock();
        }
    }

    public void AddStats(int StatsID, int Amount) {
        switch (StatsID) {
            case 0:
                queueLimit += Amount;
                break;
            case 1:
                craftingSpeed += Amount * craftingIncrecment;
                break;
            case 2:
                researchSpeed += Amount * reserachIncrement;
                break;
            case 3:
                specialSpeed += Amount * specialIncrement;
                break;
            case 4:
                craftingSpeed += Amount * craftingIncrecment;
                specialSpeed += Amount * specialIncrement;
                break;
        }
    }

    public int[,] SaveTasks() {
        int[,] tasks = new int[taskQueue.Count,4];
        for (int i = 0; i < taskQueue.Count; i++) {
            tasks[i,0] = taskQueue[i].duration.minutesTotal;
            tasks[i,1] = (int) taskQueue[i].taskType;
            tasks[i,2] = taskQueue[i].progress;
            tasks[i,3] = taskQueue[i].GetTaskID();
        }
        return tasks;
    }

    public void LoadTasks(int[,] tasks) {
        for(int i = 0; i < tasks.GetLength(0); i++) {
            Clock duration = new Clock(tasks[i,0]);
            TaskType taskType = (TaskType) tasks[i,1];
            int progress = tasks[i,2];
            int taskID = tasks[i,3];

            Task newTask = new Task(duration, taskType, progress, taskID);
            taskQueue.Add(newTask);
        }
        if (taskQueue.Count > 0) {
            StartWorking();
        }
    }

    public Clock GetCraftingTime(Clock time) {
        return new Clock((int)(time.minutesTotal / craftingSpeed));        
    }

    public Clock GetResearchTime(Clock time) {
        return new Clock((int)(time.minutesTotal / researchSpeed));        
    }

    public Clock GetSpecialTime(Clock time) {
        return new Clock((int)(time.minutesTotal / specialSpeed));    
    }

    public void Unlock() {
        unlocked = true;
        foreach(GameObject go in itemsToUnlock) {
            go.SetActive(true);
        }
        RecipeManager recipeManager = managerManager.recipeManager;
        foreach(ProductObject P in recipesToUnlock) {
            recipeManager.UnlockRecipe(P);
        }
    }

    void GainExp(int Exp) {
        exp += Exp;
        expToday += Exp;
        int nextLvlExp = GetNextLvlExp(level);
        if (exp >= nextLvlExp) {
            exp -= nextLvlExp;
            LevelUp(1,1,1);
        }
    }

    public int GetNextLvlExp(int lvl) {
        return level * timeManager.minutesPerDay.minutesTotal;
    }

    public void LevelUp(int crafting, int research, int special) {
        craftingSpeed += craftingIncrecment * crafting;
        researchSpeed += reserachIncrement * research;
        specialSpeed += specialIncrement * special;
        level++;
        salary += level/3 + 3;
    } 
    
    public void StartDay() {
        expToday = 0;
        startExpToday = exp;
        lvlToday = level;
    }

    public void Tick() {
        if(currentTask != null) {
            currentTask.progress++;
            if (currentTask.progress >= currentTask.duration.minutesTotal) {
                FinishWorking();
                return;
            }
            progressIcon.Tick();
        }
    }

    public void AddTask(Task Task) {
        taskQueue.Add(Task);
        if(taskQueue.Count == 1) {
            StartWorking();
        }
        progressIcon.UpdateQuantityText();
    }

    public void StartWorking() {
        currentTask = taskQueue[0];
        progressIcon.AssignTask(currentTask);
    }

    void StopWorking() {
        currentTask = null;
        progressIcon.EndTask();
    }

    public void FinishWorking() {
        GainExp(currentTask.duration.minutesTotal);
        currentTask.Finish();
        taskQueue.RemoveAt(0);
        if(taskQueue.Count > 0) {
            StartWorking();
        } else {
            StopWorking();
        }
    }

    public void DoNow(Task Task) {
        if (!taskQueue.Contains(Task) || taskQueue.Count < 2) {
            return;
        }
        int index = taskQueue.FindIndex(a => a == Task);
        Debug.Log(index);

        for(int i = index; i > 0; i--) {
            taskQueue[i] = taskQueue[i-1]; 
        }
        taskQueue[0] = Task;
        StartWorking();
    }

    
    public void DoNext(Task Task) {
        if (!taskQueue.Contains(Task) || taskQueue.Count < 3) {
            return;
        }
        int index = taskQueue.FindIndex(a => a == Task);
        

        for(int i = index; i > 1; i--) {
            taskQueue[i] = taskQueue[i-1]; 
            Debug.Log(i);
        }
        taskQueue[1] = Task;
    }

    public void DeleteTask(Task Task) {
        int index = taskQueue.FindIndex(a => a == Task);
        taskQueue.RemoveAt(index);
        if(index == 0) {
            StartWorking();
        }
    }
}
