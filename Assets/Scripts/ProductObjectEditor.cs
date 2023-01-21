// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;
// using UnityEditor.SceneManagement;

// [CustomEditor(typeof(ProductObject))]
// public class ProductObjectEditor : Editor
// {
//     ProductObject comp;
    
//     public void OnEnable() {
//         comp = (ProductObject)target;
//     }
    
//     public override void OnInspectorGUI() {
//         serializedObject.Update();
//         comp.ID =  EditorGUILayout.IntField("ID", comp.ID);
//         comp.productName =  EditorGUILayout.TextField("Name", comp.productName);
//         comp.productType =  EditorGUILayout.TextField("Type", comp.productType);
//         comp.worker =  EditorGUILayout.TextField("Worker", comp.worker);
//         comp.quantity =  EditorGUILayout.IntField("Quantity", comp.quantity);
//         comp.img = (Sprite)EditorGUILayout.ObjectField("Sprite", comp.img, typeof(Sprite), false); 
//         comp.desc = EditorGUILayout.TextField("Description", comp.desc);
//         comp.price =  EditorGUILayout.IntField("Price", comp.price);
//         comp.minutesToMake = EditorGUILayout.IntField("Crafting Time", comp.minutesToMake); //Formula: ((comp.price-comp.cost)/2f*9f+120f)

//         EditorGUILayout.IntField("Cost", comp.cost);
//         GUILayout.BeginHorizontal();  
//         EditorGUILayout.PropertyField(serializedObject.FindProperty("materialQuantity"));
//         EditorGUILayout.PropertyField(serializedObject.FindProperty("materialName"));
//         GUILayout.EndHorizontal();
//         serializedObject.ApplyModifiedProperties();
//     }
// }
