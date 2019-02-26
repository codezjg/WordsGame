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

namespace com.moralabs.cube.popup{

    public class TipPopup : Popup
    {
        public TextMeshProUGUI hintText;
        public Animator animator;

        private string tempStr;
        PopupManager mPopupManager;


        protected override void OnEnable()
        {
            tempStr = LevelGenerate.GetHint();
            hintText.text = tempStr;
         
            base.OnEnable();
        }

        public void PopupClose()
        {
            gameObject.SetActive(false);
            Manager.Instance.Popup.Close();
        }

        public override void Clicked(string action)
        {
            switch (action){
                case "showTip":
                    //Manager.Instance.Sound.PlayEffect(Sounds.BUTTON_SOUND);
                    Manager.Instance.ShowRewardBasedVideo();
                    break;

                case ButtonConfig.POPUP_CLOSE:
                    animator.SetBool("boolSuccess", true);
                    break;
            }

            base.Clicked(action);
        }

    }

}

