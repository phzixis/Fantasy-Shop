using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Object/Dialogue",fileName="NewDialogue")]
public class Dialogue : ScriptableObject
{
    public bool setInactive;
    public Dialogue next;
    public GameObject endScreen;

    [TextArea(3, 10)]
    public string[] sentences;

    
}
