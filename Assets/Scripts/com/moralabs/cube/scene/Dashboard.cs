using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.moralabs.cube.scene;
using com.moralabs.cube.popup;
using com.moralabs.cube.manager;
using com.moralabs.cube.conf;
using com.moralabs.cube.util;
using UnityEngine.SceneManagement;
using com.moralabs.cube.conf;

namespace com.moralabs.cube.scene
{
    public class Dashboard : Scene
    {
        public GameObject[] flagArr;
        public Animator animator;
        public int sceneID;

        SettingsPopup mSettingsPopup;
        GameObject tempObj, mFlagMask, mFlag;
        private int soundStatus;

        public Dashboard()
        {
            
        }

        protected override void Awake()
        {
            base.Awake();

            soundStatus = PlayerPrefs.GetInt("SoundStatus", 1);

            if(soundStatus == 1)
                Manager.Instance.Sound.PlaySound(Sounds.BGM, true);

            mSettingsPopup = GetComponent<SettingsPopup>();
            //mFlagMask = GameObject.Find("FlagMask");
            //mFlag = mFlagMask.transform.GetChild(0).gameObject;
            mFlag = GameObject.Find("Flag");
            foreach(GameObject flag in flagArr){
                if(flag.GetComponent<Flag>().flagID == Manager.gameLang){
                    mFlag.GetComponent<Image>().sprite = flag.GetComponent<Image>().sprite;
                }
            }
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

        public void StartGame(){
            Manager.levelNumber = PlayerPrefs.GetInt("LastLevel", 0);
            SceneManager.LoadScene(sceneID);
        }

        public override void OnClick(string action)
        {
            base.OnClick(action);

            switch(action)
            {
                case "buton1":
                    //Manager.Instance.Sound.PlayEffect(Sounds.PLAY_BUTTON);
                    sceneID = 2;
                    animator.SetBool("boolStartGame", true);
                    break;
                case "buton2":
                    mSettingsPopup.Clicked("settings");
                    break;
                case "buton3":
                    //Manager.Instance.Sound.PlayEffect(Sounds.BUTTON_SOUND);
                    sceneID = 1;
                    animator.SetBool("boolStartGame", true);
                    break;

                case "butonQuestion":
                    Manager.levelNumber = 1;
                    SceneManager.LoadScene(2);
                    break;


                case null:
                    break;
            }


        }


    }

}