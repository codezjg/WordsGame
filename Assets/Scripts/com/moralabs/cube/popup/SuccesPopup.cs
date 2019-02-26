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
using UnityEngine.SceneManagement;

namespace com.moralabs.cube.popup
{

    public class SuccesPopup : Popup
    {
        public static float percentOfTime;
        public TextMeshProUGUI succText;

        PopupManager mPopupManager;
        LevelGenerate mLevelGenerate;
        Transform star1, star2, star3;

        private Animator animator;

        protected override void Start()
        {
            mLevelGenerate = FindObjectOfType<LevelGenerate>();
            animator = GetComponent<Animator>();

            if (mPopupManager == null)
                mPopupManager = Manager.Instance.Popup;

            base.Start();
        }

        protected override void OnEnable()
        {
            if (mLevelGenerate == null)
                mLevelGenerate = FindObjectOfType<LevelGenerate>();

            star1 = gameObject.transform.GetChild(1).GetChild(0);
            star2 = gameObject.transform.GetChild(1).GetChild(1);
            star3 = gameObject.transform.GetChild(1).GetChild(2);

            star1.GetComponent<Image>().SetTransparency(20f / 255f);
            star2.GetComponent<Image>().SetTransparency(20f / 255f);
            star3.GetComponent<Image>().SetTransparency(20f / 255f);

            int lastStarScore = PlayerPrefs.GetInt("LevelStars" + LevelGenerate.levelNum.ToString());

            if (percentOfTime > 0.82f){
                succText.text = "super";
                succText.color = Color.red;
                PlayerPrefs.SetInt("LevelStars" + LevelGenerate.levelNum.ToString(), 3);
                star1.GetComponent<Image>().SetTransparency(255f);
                star2.GetComponent<Image>().SetTransparency(255f);
                star3.GetComponent<Image>().SetTransparency(255f);
            }else if(percentOfTime >= 0.60f){
                if (lastStarScore < 2)
                    PlayerPrefs.SetInt("LevelStars" + LevelGenerate.levelNum.ToString(), 2);

                succText.text = "ok";
                succText.color = Color.green;

                star1.GetComponent<Image>().SetTransparency(255f);
                star3.GetComponent<Image>().SetTransparency(255f);
            }else if(percentOfTime >= 0.15f){
                if (lastStarScore < 1)
                    PlayerPrefs.SetInt("LevelStars" + LevelGenerate.levelNum.ToString(), 1);
                    

                succText.text = "ok";
                succText.color = Color.green;
                star1.GetComponent<Image>().SetTransparency(255f);
            }else{
                if (lastStarScore < 0)
                    PlayerPrefs.SetInt("LevelStars" + LevelGenerate.levelNum.ToString(), 0);
                    
                succText.text = "ok";
                succText.color = Color.green;
            }

            base.OnEnable();
        }

        public void PopupClose(){
            gameObject.SetActive(false);
            Manager.Instance.Popup.Close();
        }

        public override void Clicked(string action)
        {
            if (mPopupManager == null)
                mPopupManager = Manager.Instance.Popup;

            switch (action)
            {
                case "continue":
                    //Manager.Instance.Sound.PlayEffect(Sounds.BUTTON_SOUND);
                    //Manager.Instance.Popup.Close();
                    animator.SetBool("boolSuccess", true);
                    if(LevelGenerate.levelNum <= 99){
                        LevelGenerate.levelNum++;
                    }else{
                        PlayerPrefs.SetInt("LastLevel", 99);
                        Manager.Instance.Popup.Close();
                        gameObject.SetActive(false);
                        SceneManager.LoadScene("Dashboard");
                    }

                    break;

                case ButtonConfig.POPUP_CLOSE:
                    Manager.Instance.Popup.Close();
                    break;

                case "exit":
                    gameObject.SetActive(false);
                    break;

                case "home":
                    //Manager.Instance.Sound.PlayEffect(Sounds.BUTTON_SOUND);
                    gameObject.SetActive(false);
                    mPopupManager.Close();
                    SceneManager.LoadScene(0);
                    break;

                case "restart":
                    //Manager.Instance.Sound.PlayEffect(Sounds.BUTTON_SOUND);
                    animator.SetBool("boolSuccess", true);
                    mPopupManager.Close();
                    mLevelGenerate.Reset();
                    break;

                case null:
                    break;

            }

            base.Clicked(action);
        }

    }

}

