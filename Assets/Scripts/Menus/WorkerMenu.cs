using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WorkerMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI statValues;
    [SerializeField] TextMeshProUGUI queueText;
    [SerializeField] Transform queueContent;
    [SerializeField] GameObject taskIcon;
    [SerializeField] GameObject AddTaskButton;
    [SerializeField] RectTransform queueParent;
    ContentSizeFitter queueCSF;

    List<GameObject> currentQueue = new List<GameObject>();
    Worker worker;
    TaskIcon currentOpen = null;

    TimeManager timeManager;


    void Awake() {
        timeManager = FindObjectOfType<TimeManager>();
    }   

    void Start() {
        timeManager.Pause();
        queueCSF = queueText.gameObject.AddComponent<ContentSizeFitter>();
        queueCSF.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
    }
    
    public void SetWorker(string Worker) {
        worker = FindObjectOfType<WorkerManager>().workerDict[Worker];
        nameText.text = worker.workerName;
        image.sprite = worker.sprite;
        //set image here also
        UpdateStatValues();
        UpdateQueue();
    }

    void UpdateStatValues() {
        string statsText = "<color=yellow>";
        statsText += worker.job + "\n"
                    + worker.craftingSpeed.ToString() + "\n"
                    + worker.researchSpeed.ToString() + "\n\n";
        statsText += worker.unlockedSpecial ? worker.specialName + "\n" + worker.specialSpeed.ToString() : "???\n???";
        statValues.text = statsText;
    }
    
    void UpdateQueue() {
        queueText.text = "Queue " + worker.taskQueue.Count.ToString() + "/" + worker.queueLimit.ToString();
        
        foreach(GameObject go in currentQueue) {
            Destroy(go);
        }
        currentQueue.Clear();

        foreach(Task t in worker.taskQueue) {
            GameObject go = Instantiate(taskIcon, queueContent);
            go.GetComponent<TaskIcon>().SetTask(t);
            go.GetComponent<TaskIcon>().workerMenu = this;
            currentQueue.Add(go);
        }
        if(worker.taskQueue.Count < worker.queueLimit) {
            GameObject go = Instantiate(AddTaskButton, queueContent);
            UIManager um = FindObjectOfType<UIManager>();
            go.GetComponent<Button>().onClick.AddListener(()=>{CloseMenu(); um.OpenCraftingMenu(worker.job);});
        }
    }

    public void DoNow(Task Task) {
        worker.DoNow(Task);
        UpdateQueue();
    }

    public void DoNext(Task Task) {
        worker.DoNext(Task);
        UpdateQueue();
    }

    public void DeleteTask(Task Task) {
        worker.DeleteTask(Task);
        UpdateQueue();
    }

    public void CloseMenu() {
        timeManager.Resume();
        Destroy(gameObject);
    }

    public void OpenTaskIcon(TaskIcon TaskIcon) {
        if(currentOpen != null) {
            currentOpen.CloseTask();
        }
        currentOpen = TaskIcon;
    }
}
