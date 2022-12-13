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

        // �ڷ�ƾ ����
        StartCoroutine("AdsLoadWithNetwork");
    }

    IEnumerator AdsLoadWithNetwork()
    {
        while (!adsLoaded)
        {
            Debug.Log("adsLoaded: " + adsLoaded);
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                // ��Ʈ��ũ�� ����X -> �ڷ�ƾ���� 0.1�ʸ��� ��� ���� ���� Ȯ��
                yield return new WaitForSecondsRealtime(0.1f);
            }
            else if (!adsLoaded)
            {
                // ��Ʈ��ũ�� ����Ǹ� ���� �޾ƿ�
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
        Debug.Log("<color=aqua>���� �ε� �Ϸ� -> �ڷ�ƾ ����</color>");
        yield break;
    }


    public void  HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("���� �ε� ����");
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
            // ���ͳ� ����X
            btnmanager.SetActive_StartBtn(false);
            toast_msg.GetComponent<Animator>().Play("toast_up");
            Invoke("ToastDown", 1f);
        }
        else
        {
            // ���ͳ� ����O
            if (!adsLoaded)    // ���� �ε� �ȵ�
            {
                btnmanager.SetActive_StartBtn(false);
                toast_msg.GetComponent<Animator>().Play("toast_up");
                Invoke("ToastDown", 1f);
            }
            else
            {
                // ȭ�� ���̵� �ƿ� -> ����
                btnmanager.SetActive_StartBtn(false);
                btnmanager.anim.Play("btn_fade_out");
                btnmanager.FadeOutBeforeAds();  // ���̵� �ƿ� 1.5��
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
