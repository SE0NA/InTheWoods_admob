using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.SceneManagement;
using System.Collections;

public class GoogleMobileAdsScript : MonoBehaviour
{
    private RewardedAd rewardedAd;

    [SerializeField] GameObject toast_msg;

    private StartBtnManager btnmanager;

    private string adUnitId;
    private bool adsLoaded = false;

    public void Awake()
    {
        btnmanager = FindObjectOfType<StartBtnManager>();

#if UNITY_ANDROID
        adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
        adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
        adUnitId = "unexpected_platform";
#endif

        MobileAds.Initialize(initStatus => { });

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

        // 코루틴 시작
        StartCoroutine("AdsLoadWithNetwork");
    }

    IEnumerator AdsLoadWithNetwork()
    {
        while (!adsLoaded)
        {
            Debug.Log("adsLoaded: " + adsLoaded);
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                // 네트워크에 연결X -> 코루틴으로 0.1초마다 계속 연결 여부 확인
                yield return new WaitForSecondsRealtime(0.1f);
            }
            else if (!adsLoaded)
            {
                // 네트워크가 연결되면 광고를 받아옴
                // Create an empty ad request.
                AdRequest request = new AdRequest.Builder().Build();
                // Load the rewarded ad with the request.
                this.rewardedAd.LoadAd(request);

                while (!this.rewardedAd.IsLoaded())
                {
                    Debug.Log("loading ads...");
                }
                adsLoaded = true;
            }
        }
        Debug.Log("<color=aqua>광고 로드 완료 -> 코루틴 종료</color>");
        yield break;
    }


    public void  HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("광고 로드 성공");
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
        btnmanager.FailedGameStart();
        btnmanager.SetActive_StartBtn(false);
        toast_msg.GetComponent<Animator>().Play("toast_up");
        Invoke("ToastDown", 1f);
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
        if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            // 인터넷 연결X
            btnmanager.SetActive_StartBtn(false);
            toast_msg.GetComponent<Animator>().Play("toast_up");
            Invoke("ToastDown", 1f);
        }
        else
        {
            // 인터넷 연결O
            if (!adsLoaded)    // 광고 로딩 안됨
            {
                btnmanager.SetActive_StartBtn(false);
                toast_msg.GetComponent<Animator>().Play("toast_up");
                Invoke("ToastDown", 1f);
            }
            else
            {
                // 화면 페이드 아웃 -> 광고
                btnmanager.SetActive_StartBtn(false);
                btnmanager.anim.Play("btn_fade_out");
                btnmanager.FadeOutBeforeAds();  // 페이드 아웃 1.5초
                Invoke("ShowAds", 1f);
            }
        }
    }
    

    private void ShowAds()
    {
        this.rewardedAd.Show();
    }
    
    private void ToastDown()
    {
        btnmanager.SetActive_StartBtn(true);
    }
}
