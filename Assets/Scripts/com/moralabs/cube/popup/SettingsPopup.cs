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

namespace com.moralabs.cube.popup{

    public class SettingsPopup : Popup
    {
        PopupManager mPopupManager;
        GameObject turnoffSound, turnoffSfx;
        int soundStatus, sfxStatus;
        Color colorSoundOff, colorSfxOff;

        protected override void Start()
        {
            turnoffSound = GameObject.Find("BtnMusic");
            turnoffSfx = GameObject.Find("BtnSfx");

            colorSoundOff = turnoffSound.GetComponent<Image>().color;
            colorSfxOff = turnoffSfx.GetComponent<Image>().color;

            base.Start();
        }

        protected override void OnEnable()
        {
            turnoffSound = GameObject.Find("BtnMusic");
            turnoffSfx = GameObject.Find("BtnSfx");

            soundStatus = PlayerPrefs.GetInt("SoundStatus", 1);
            sfxStatus = PlayerPrefs.GetInt("SfxStatus", 1);

            GetPrefs();

            base.OnEnable();
        }

        private void GetPrefs()
        {
            if (soundStatus == 1){
                turnoffSound.GetComponent<Image>().SetTransparency(255f);
            }else{
                turnoffSound.GetComponent<Image>().SetTransparency(100f / 255f);
            }

            if (sfxStatus == 1){
                turnoffSfx.GetComponent<Image>().SetTransparency(255f);
            }
            else{
                turnoffSfx.GetComponent<Image>().SetTransparency(100f/255f);
            }
        }

        public override void Clicked(string action)
        {
            if(mPopupManager == null){
                mPopupManager = Manager.Instance.Popup;
            }

            switch(action){
                case "settings":
					//Manager.Instance.Sound.PlayEffect(Sounds.BUTTON_SOUND);
                    mPopupManager.Open("settings");
                    break;

                case "sfx":
					//Manager.Instance.Sound.PlayEffect(Sounds.TOGGLE_SOUND);
                    if (turnoffSfx.GetComponent<Image>().color.a.Equals(100f / 255f))
                    {
                        Options.Instance.SetOption(Options.EFFECT, Options.ON);
                        turnoffSfx.GetComponent<Image>().SetTransparency(255f);
                        PlayerPrefs.SetInt("SfxStatus", 1); //aktif et
                    }
                    else
                    {
                        turnoffSfx.GetComponent<Image>().SetTransparency(100f / 255f);
						Options.Instance.SetOption(Options.EFFECT,Options.OFF);
                        PlayerPrefs.SetInt("SfxStatus", 0);
                    }

                    break;

                case "sound":
					//Manager.Instance.Sound.PlayEffect(Sounds.TOGGLE_SOUND);
                    if (turnoffSound.GetComponent<Image>().color.a.Equals(100f / 255f))
                    {
                        turnoffSound.GetComponent<Image>().SetTransparency(255f);
                        Options.Instance.SetOption(Options.SOUND,Options.ON);
                        Manager.Instance.Sound.PlaySound(Sounds.BGM,true);
                        PlayerPrefs.SetInt("SoundStatus", 1); //aktif et
                    }
                    else
                    {
                        turnoffSound.GetComponent<Image>().SetTransparency(100f / 255f);
                        Options.Instance.SetOption(Options.SOUND,Options.OFF);
                        Manager.Instance.Sound.StopSounds();
                        PlayerPrefs.SetInt("SoundStatus", 0);
                    }
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

