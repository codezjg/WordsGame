using UnityEngine;
using System.Collections;
using com.moralabs.cube.manager;
using com.moralabs.cube.popup;
using com.moralabs.cube.animation;
using com.moralabs.cube.scene;
using com.moralabs.cube.conf;
using com.moralabs.cube.util;
using System;
using UnityEngine.UI;
using TMPro;

namespace com.moralabs.cube.popup
{
    public class IAPPopup : Popup
    {
        PopupManager mPopupManager;
        LevelGenerate mLevelGenerate;


        protected override void Start()
        {
            if (mPopupManager == null)
                mPopupManager = Manager.Instance.Popup;

            if (mLevelGenerate == null)
                mLevelGenerate = FindObjectOfType<LevelGenerate>();

            base.Start();
        }

        protected override void OnEnable()
        {
            if (mLevelGenerate == null)
                mLevelGenerate = FindObjectOfType<LevelGenerate>();

            base.OnEnable();
        }


        public override void Clicked(string action)
        {
            if (mPopupManager == null)
                mPopupManager = Manager.Instance.Popup;

            switch (action)
            {
                case "open":
                    Manager.Instance.Popup.Open("InAppPurchase");
                    break;

                case "buy_tip50":
                    //Manager.Instance.Sound.PlayEffect(Sounds.BUTTON_SOUND);
                    Purchaser.Instance.Buy50Tip();
                    break;
                    

                case "buy_tip150":
                    Purchaser.Instance.Buy150Tip();
                    break;


                case "buy_tip300":
                    Purchaser.Instance.Buy300Tip();
                    break;


                case ButtonConfig.POPUP_CLOSE:
                    Manager.Instance.Popup.Close();
                    break;

                case "exit":
                    gameObject.SetActive(false);
                    break;
            }

            base.Clicked(action);
        }

    }

}

