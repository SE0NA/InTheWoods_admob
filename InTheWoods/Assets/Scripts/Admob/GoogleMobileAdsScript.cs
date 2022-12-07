using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class GoogleMobileAdsScript : MonoBehaviour
{
    private InterstitialAd intersitial;

    [SerializeField] GameObject toast_msg;

    public void Start()
    {
        string adUnitId;
#if UNITY_ANDROID
        adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
        adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        adUnitId = "unexpected_platform";
#endif

        // Initialize the Google Mobile Ads SDK.
        // MobileAds.Initialize(initStatus => { });

        // Initialize an InterstitialAd.
        this.intersitial = new InterstitialAd(adUnitId);

        // Called when the ad is closed.
        this.intersitial.OnAdClosed += HandleOnAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.intersitial.LoadAd(request);
    }

  
    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        SceneManager.LoadScene("Game");
    }

    public void GameStart()
    {
        if (this.intersitial.IsLoaded())
        {
            this.intersitial.Show();
        }
        else
        {
            StartBtnManager btnmanager = FindObjectOfType<StartBtnManager>();
            btnmanager.FailedGameStart();
            toast_msg.SetActive(true);
            toast_msg.GetComponent<Animator>().Play("toast_up");
            Invoke("Toast_False", 0.5f);
        }
    }
    private void Toast_False()
    {
        toast_msg.SetActive(false);
    }
}
