using UnityEngine;
using System.Collections.Generic;

using com.moralabs.cube.util;
using com.moralabs.cube.scene;
using UnityEngine.SceneManagement;
using System;
using com.moralabs.cube.conf;
using GoogleMobileAds.Api;

namespace com.moralabs.cube.manager
{
    public class Manager : Singleton<Manager>
    {
        private SoundManager sound;
        private PopupManager popup;
        private InterstitialAd interstitial;
        private RewardBasedVideoAd rewardBasedVideo;

        public delegate void OnUpdate(float deltaTime);
        public OnUpdate onUpdate;

        public static int levelNumber;
        public static string gameLang;
        public static int lastPage, lastHintCount;

        int soundStatus, sfxStatus;

        void Awake()
        {
            levelNumber = PlayerPrefs.GetInt("LastLevel", 5);
            lastPage = CPlayerPrefs.GetInt("LastPage", 0);
            gameLang = PlayerPrefs.GetString("lang", "en");
            soundStatus = PlayerPrefs.GetInt("SoundStatus", 1);
            sfxStatus = PlayerPrefs.GetInt("SfxStatus", 1);

            sound = new SoundManager(gameObject);
            popup = new PopupManager();
            sound.PlaySound(Sounds.BGM, true);

            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        void Start()
        {
#if UNITY_ANDROID
            string appId = Config.ANDROID_APPID;
#elif UNITY_IPHONE
            string appId = Config.IOS_APPID;
#else
            string appId = "unexpected_platform";
#endif

            MobileAds.Initialize(appId);
            MobileAds.SetiOSAppPauseOnBackground(true);

            this.rewardBasedVideo = RewardBasedVideoAd.Instance;
            this.rewardBasedVideo.OnAdLoaded += this.HandleRewardBasedVideoLoaded;
            this.rewardBasedVideo.OnAdFailedToLoad += this.HandleRewardBasedVideoFailedToLoad;
            this.rewardBasedVideo.OnAdOpening += this.HandleRewardBasedVideoOpened;
            this.rewardBasedVideo.OnAdStarted += this.HandleRewardBasedVideoStarted;
            this.rewardBasedVideo.OnAdRewarded += this.HandleRewardBasedVideoRewarded;
            this.rewardBasedVideo.OnAdClosed += this.HandleRewardBasedVideoClosed;
            this.rewardBasedVideo.OnAdLeavingApplication += this.HandleRewardBasedVideoLeftApplication;

            this.RequestRewardBasedVideo();
            this.RequestInterstitial();

        }

        private void RequestRewardBasedVideo()
        {
#if UNITY_ANDROID
            string adUnitId = Config.ANDROID_REWARDED;
#elif UNITY_IPHONE
            string adUnitId = Config.IOS_REWARDED;
#else
        string adUnitId = "unexpected_platform";
#endif

            this.rewardBasedVideo.LoadAd(this.CreateAdRequest(), adUnitId);
        }

        public void ShowInterstitial()
        {
            if (this.interstitial.IsLoaded())
            {
                this.interstitial.Show();
            }
            else
            {
                this.RequestInterstitial();
                MonoBehaviour.print("Interstitial is not ready yet");
            }
        }

        public void ShowRewardBasedVideo()
        {
            if (this.rewardBasedVideo.IsLoaded())
            {
                this.rewardBasedVideo.Show();
            }
            else
            {
                this.RequestRewardBasedVideo();
                MonoBehaviour.print("Reward based video ad is not ready yet");
            }
        }

        private void RequestInterstitial()
        {
#if UNITY_EDITOR
            string adUnitId = "unused";

#elif UNITY_ANDROID
            string adUnitId = Config.ANDROID_INTERSTITIAL;
#elif UNITY_IPHONE
            string adUnitId = Config.IOS_INTERSTITIAL;
#else
        string adUnitId = "unexpected_platform";
#endif

            if (this.interstitial != null)
            {
                this.interstitial.Destroy();
            }

            this.interstitial = new InterstitialAd(adUnitId);

            this.interstitial.OnAdLoaded += this.HandleInterstitialLoaded;
            this.interstitial.OnAdFailedToLoad += this.HandleInterstitialFailedToLoad;
            this.interstitial.OnAdOpening += this.HandleInterstitialOpened;
            this.interstitial.OnAdClosed += this.HandleInterstitialClosed;
            this.interstitial.OnAdLeavingApplication += this.HandleInterstitialLeftApplication;

            this.interstitial.LoadAd(this.CreateAdRequest());
        }


        private AdRequest CreateAdRequest()
        {
            return new AdRequest.Builder()
            .TagForChildDirectedTreatment(false)
            .AddTestDevice("8B171216A18A2C889ADF71BBCC97E39A")
            .Build();
        }


        #region interstitial

        public void HandleInterstitialLoaded(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleInterstitialLoaded event received");
        }

        public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            this.RequestInterstitial();
            MonoBehaviour.print(
                "HandleInterstitialFailedToLoad event received with message: " + args.Message);
        }

        public void HandleInterstitialOpened(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleInterstitialOpened event received");
        }

        public void HandleInterstitialClosed(object sender, EventArgs args)
        {
            this.RequestInterstitial();
            MonoBehaviour.print("HandleInterstitialClosed event received");
        }

        public void HandleInterstitialLeftApplication(object sender, EventArgs args)
        {
            this.RequestInterstitial();
            MonoBehaviour.print("HandleInterstitialLeftApplication event received");
        }

        #endregion

        #region reward based
        public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
        {
            LevelGenerate.SetTipActive();
            MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
        }


        public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            LevelGenerate.SetTipDeactive();
            this.RequestRewardBasedVideo();
            MonoBehaviour.print(
                "HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message);
        }

        public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
        }

        public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
        {
            MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
        }

        public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
        {
            this.RequestRewardBasedVideo();
            MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
        }

        public void OpenTip(){
                Popup.Open("tip");
        }

        public void HandleRewardBasedVideoRewarded(object sender, Reward args)
        {
            this.RequestRewardBasedVideo();

            string type = args.Type;
            double amount = args.Amount;

            Invoke("OpenTip", 0.5f);
        }

        public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
        {
            this.RequestRewardBasedVideo();
            MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
        }
        #endregion



        public SoundManager Sound
        {
            get
            {
                return sound;
            }
        }

        public PopupManager Popup
        {
            get
            {
                return popup;
            }
        }

        public void Init(){
        }

        public void Create(){
        }


    }

}