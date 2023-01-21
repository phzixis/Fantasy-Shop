using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    Queue<string> sentences;

    [SerializeField] GameObject topDialogueBox;
    [SerializeField] GameObject botDialogueBox;
    GameObject currBox;
    GameObject endScreen;
    MenuDirectory currDirectory;
    Dialogue currDialogue;
    string currBoxType;
    public float dialogueSpeed;
    TimeManager timeManager;
    public Canvas canvas;

    void Awake() {
        timeManager = GameObject.FindObjectOfType<TimeManager>();
    }
    void Start() {
        sentences = new Queue<string>();
        currBoxType = "none";
    }

    public void StartDialogue(Dialogue dialogue) {
        if(currBoxType != "none") {
            CloseDialogue();
        }
        timeManager.Pause();
        sentences.Clear();
        foreach(string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }
        
        endScreen = dialogue.endScreen;
        currDialogue = dialogue;
        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
        if (sentences.Count == 0 && endScreen == null) {
            CloseDialogue();
            return;
        }
        string[] sentence = sentences.Dequeue().Split('|');
        
        if(currDialogue.setInactive) {
            sentence = new string[3];
            sentence[0] = "";
            sentence[1] = "";
            sentence[2] = "";
        }

        if(sentence[1] != currBoxType) {
            if(currBoxType != "none") {
                Destroy(currBox);
            } 
            currBoxType = sentence[1];
            if(sentence[1] == "top") {
                currBox = Instantiate(topDialogueBox, canvas.transform);
            } else {
                currBox = Instantiate(botDialogueBox, canvas.transform);
            }
        } else {
            if(currBoxType == "none") {
                currBoxType = sentence[1];
                if(sentence[1] == "top") {
                    currBox = Instantiate(topDialogueBox, canvas.transform);
                } else {
                    currBox = Instantiate(botDialogueBox, canvas.transform);
                }
            } 
        }

        currDirectory = currBox.GetComponent<MenuDirectory>();
        
        currDirectory.tmiText2.text = sentence[0];
        currDirectory.go2.SetActive(sentences.Count > 0);
        currBox.SetActive(!currDialogue.setInactive);

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence[2]));
        if (sentences.Count == 0 && endScreen != null) {
            GameObject screen = Instantiate(endScreen, canvas.transform);
            screen.transform.SetSiblingIndex(currBox.transform.GetSiblingIndex());
            currDirectory.go.SetActive(false);
        }
    }

    IEnumerator TypeSentence(string sentence) {
        currDirectory.tmiText.text = "";
        foreach (char letter in sentence.ToCharArray()) {
            currDirectory.tmiText.text += letter;
            if(letter == '.') {
                yield return new WaitForSeconds(dialogueSpeed*15);
            } else {
                yield return new WaitForSeconds(dialogueSpeed);
            }
        }
    }

    public void CloseDialogue() {
        Destroy(currBox);
        currBoxType = "none";
        timeManager.Resume();

        if(currDialogue.next != null) {
            StartDialogue(currDialogue.next);
        }
    }
}
