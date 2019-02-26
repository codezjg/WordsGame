using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.moralabs.cube.manager;
using UnityEngine.UI;

public class Flag : MonoBehaviour {

    public string flagID;

    private GameObject flagMaskObj, flagObj;

    public void OnClick()
    {
        //Manager.Instance.Sound.PlayEffect(Sounds.BUTTON_SOUND);
        Manager.gameLang = flagID;
        PlayerPrefs.SetString("lang", flagID);
        Debug.Log(flagID);
        //flagMaskObj = GameObject.Find("FlagMask");
        //flagObj = flagMaskObj.transform.GetChild(0).gameObject;
        flagObj = GameObject.Find("Flag");
        flagObj.GetComponent<Image>().sprite = gameObject.GetComponent<Image>().sprite;
        Manager.Instance.Popup.Close();
    }


}
