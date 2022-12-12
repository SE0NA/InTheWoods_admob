using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.SceneManagement;

public class GoogleMobileAdsScript : MonoBehaviour
{
    private RewardedAd rewardedAd;

    [SerializeField] GameObject toast_msg;

    private StartBtnManager btnmanager;

    private string adUnitId;
    public bool adLoad = true;

    public void Awake()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });
        btnmanager = FindObjectOfType<StartBtnManager>();

#if UNITY_ANDROID
        adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
        adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
        adUnitId = "unexpected_platform";
#endif
        // Initialize an RewardedAd.
        this.rewardedAd = new RewardedAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    public void  HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("광고 로드 성공");
        adLoad = true;
    }
    public void HandleUserEarnedReward(object sender, EventArgs args)
    {
        SceneManager.LoadScene("Game");
    }
    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        btnmanager.anim.Play("btn_fade_in");
        this.CreateAndLoadRewardedAd();
    }
    public void HandleRewardedAdFailedToLoad(object sender, EventArgs args)
    {
        adLoad = false;
        btnmanager.FailedGameStart();
        ToastUp();
    }

    public RewardedAd CreateAndLoadRewardedAd()
    {
        RewardedAd rewardedAd = new RewardedAd(adUnitId);

        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);

        return rewardedAd;
    }

    public void GameStart()
    {
        if (this.rewardedAd.IsLoaded())
        {
            // 화면 페이드 아웃 -> 광고
            btnmanager.anim.Play("btn_fade_out");
            btnmanager.FadeOutBeforeAds();  // 페이드 아웃 1.5초
            Invoke("ShowAds", 2f);
        }
        else
        {
            ToastUp();
        }
    }
    private void ShowAds()
    {
        this.rewardedAd.Show();
    }
    
    private void ToastUp()
    {
        toast_msg.SetActive(true);
        toast_msg.GetComponent<Animator>().Play("toast_up");
        Invoke("ToastDown", 1f);
    }
    private void ToastDown()
    {
        toast_msg.SetActive(false);
    }
}
