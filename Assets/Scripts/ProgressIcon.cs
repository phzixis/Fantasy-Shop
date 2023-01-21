using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressIcon : MonoBehaviour
{
    [SerializeField] Image circle;
    [SerializeField] Image icon;
    [SerializeField] Image iconAlpha;
    [SerializeField] Image circleAlpha;
    [SerializeField] Image workerImage;
    [SerializeField] TextMeshProUGUI queueAmt;
    [SerializeField] List<Sprite> circlesList; 
    Task task;
    public Worker worker;

    public void AssignTask(Task Task) {
        icon.gameObject.SetActive(true);
        iconAlpha.gameObject.SetActive(true);
        circleAlpha.gameObject.SetActive(true);
        workerImage.gameObject.SetActive(false);
        task = Task;
        icon.sprite = Task.sprite;
        iconAlpha.sprite = Task.sprite;

        switch (task.taskType) {
            case TaskType.craft:
                circle.sprite = circlesList[0];
                circleAlpha.sprite = circlesList[0];
                break;
            case TaskType.research:
                circle.sprite = circlesList[1];
                circleAlpha.sprite = circlesList[1];
                break;
            case TaskType.furniture:
                circle.sprite = circlesList[2];
                circleAlpha.sprite = circlesList[2];
                break;
        }
        circle.type = Image.Type.Filled;
        icon.type = Image.Type.Filled;
        circle.fillAmount = 0;
        icon.fillAmount = 0;

        UpdateQuantityText();
    }

    public void EndTask() {
        circle.fillAmount = 1;
        circle.sprite = circlesList[0];
        circleAlpha.sprite = circlesList[0];
        icon.gameObject.SetActive(false);
        iconAlpha.gameObject.SetActive(false);
        circleAlpha.gameObject.SetActive(false);
        workerImage.gameObject.SetActive(true);
    }

    public void UpdateQuantityText() {
        if(worker.taskQueue.Count <= 1) {
            queueAmt.gameObject.SetActive(false);
        } else {
            queueAmt.gameObject.SetActive(true);
            queueAmt.text = worker.taskQueue.Count.ToString();
            if(worker.taskQueue.Count >= worker.queueLimit) {  
                queueAmt.color = Color.green;
            } else {
                queueAmt.color = Color.white;
            }
        }
    }

    public void Tick() {
        float newFill = task.progress/(float)task.duration.minutesTotal;
        circle.fillAmount = newFill;
        icon.fillAmount = newFill;
    }

    public void Remove() {
        Destroy(gameObject);
    }
}
