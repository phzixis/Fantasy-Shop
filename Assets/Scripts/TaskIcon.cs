using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskIcon : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Image circle;
    [SerializeField] List<Sprite> circlesList;
    [SerializeField] Button button; 
    [SerializeField] List<GameObject> popUpButtons;
    List<Vector3> buttonPositions = new List<Vector3>();
    public WorkerMenu workerMenu;

    bool opened = false;
    const float popUpDuration = 0.6f;
    
    Task currentTask;

    void Start() {
        foreach(GameObject go in popUpButtons) {
            buttonPositions.Add(go.transform.position - transform.position);
            go.transform.position = transform.position;
        }
    }

    void Update() {
        if (opened) {
            for(int i = 0; i < popUpButtons.Count; i++) {
                Vector3 curPos = popUpButtons[i].transform.position;
                popUpButtons[i].transform.position = Vector3.MoveTowards(curPos, buttonPositions[i]+transform.position, popUpDuration);
            }
        } else {
            for(int i = 0; i < popUpButtons.Count; i++) {
                Vector3 curPos = popUpButtons[i].transform.position;
                popUpButtons[i].transform.position = Vector3.MoveTowards(curPos, transform.position, popUpDuration);
            }
        }
    }

    public void SetTask(Task Task) {
        currentTask = Task;
        icon.sprite = Task.sprite;
        circle.sprite = circlesList[(int) Task.taskType];
    }

    public void OpenTask() {
        opened = !opened;
        workerMenu.OpenTaskIcon(this);
    }

    public void CloseTask() {
        opened = false;
    }

    public void DoNow() {
        workerMenu.DoNow(currentTask);
    }

    public void DoNext() {
        workerMenu.DoNext(currentTask);
    }

    public void DeleteTask() {
        workerMenu.DeleteTask(currentTask);
    }
}
