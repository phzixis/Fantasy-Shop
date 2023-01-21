using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Object/Unlock",fileName = "New Unlock")]
public class UnlockObject : ScriptableObject
{
    public int lvl;
    public List<string> features;
    public List<CustomerObject> customers;
    public List<FurnitureObject> furnitures;
    public int gems; 
}
