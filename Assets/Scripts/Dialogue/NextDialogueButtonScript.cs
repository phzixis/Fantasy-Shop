using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextDialogueButtonScript : MonoBehaviour
{
    void Start() {
        DialogueManager dm = FindObjectOfType<DialogueManager>();
        GetComponent<Button>().onClick.AddListener(() => {dm.DisplayNextSentence();});
    }
}
