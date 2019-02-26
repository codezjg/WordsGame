using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.moralabs.cube.util;
using com.moralabs.cube.manager;
using TMPro;

public class PayHint : MonoBehaviour {

    public TextMeshProUGUI hintCounter;

    public void PayHintClicked(){
        int tempNum = CPlayerPrefs.GetInt("LastHintCount", 5);

        if (tempNum<=0){
            Manager.Instance.Popup.Open("InAppPurchase");
            return;
        }

        tempNum--;
        hintCounter.text = tempNum.ToString();
        CPlayerPrefs.SetInt("LastHintCount", tempNum);
        Manager.Instance.Popup.Open("tip");
    }
}
