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

    public class CreditsPopup : Popup
    {
        PopupManager mPopupManager;

        protected override void Start()
        {
            if (mPopupManager == null)
                mPopupManager = Manager.Instance.Popup;

            base.Start();
        }


        public override void Clicked(string action)
        {
            if (mPopupManager == null)
                mPopupManager = Manager.Instance.Popup;

            switch (action)
            {

                case "info":
                    Manager.Instance.Popup.Open("credits");
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

