using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Object/Customer", fileName = "New Customer")]
public class CustomerObject : ScriptableObject {

    public string customerClass;
    public GameObject prefab;
    public List<string> canBuy;
    public List<string> buyDialogue;
}
