using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUpPopUp : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] MenuDirectory rewards;
    [SerializeField] MenuDirectory customers;
    [SerializeField] MenuDirectory features;
    
    void AssignLevel (UnlockObject Unlock){
        levelText.text = Unlock.lvl.ToString();
        if (Unlock.customers.Count > 0) {
            customers.gameObject.SetActive(true);
        }
        if (Unlock.features.Count > 0) {
            features.gameObject.SetActive(true);
        }
        
    }
}
