using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance;

    public UnityEvent OnBannerAdLoaded;
    public UnityEvent OnBannerAdLoadFailed;
    public UnityEvent OnBannerAdOpened;
    public UnityEvent OnBannerAdClosed;
    public UnityEvent OnBannerClickLeaveApplication;

    public UnityEvent OnInterstitialAdLoaded;
    public UnityEvent OnInterstitialAdLoadFailed;
    public UnityEvent OnInterstitialAdOpened;
    public UnityEvent OnInterstitialAdClosed;
    public UnityEvent OnInterstitialClickLeaveApplication;

    private string editorBannerAdUnit = "unused";
    private string androidBannerAdUnit = "ca-app-pub-9862166203319203/7397366975";
    private string androidInterstitialAdUnit = "ca-app-pub-9862166203319203/2070686970";

    private float timeBetweenAds;

    private BannerView activeBanner;
    private InterstitialAd activeInterstitial;

    void Awake()
    {
        Instance = this;
        timeBetweenAds = 30.0f;
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void GetBannerAd()
    {
        activeBanner = new BannerView(androidBannerAdUnit, AdSize.SmartBanner, AdPosition.Bottom);
        activeBanner.LoadAd(GetAdRequest());

        activeBanner.OnAdLoaded += BannerAdLoaded;
        activeBanner.OnAdFailedToLoad += BannerAdLoadFailed;
        activeBanner.OnAdOpening += BannerAdOpened;
        activeBanner.OnAdClosed += BannerAdClosed;
        activeBanner.OnAdLeavingApplication += BannerAdClickLeaveApplication;
    }

    public void CloseBannerAd()
    {
        if (activeBanner == null) return;

        activeBanner.Destroy();
        activeBanner = null;
    }

    public void GetInterstitialAd()
    {
        var interstitial_ad = new InterstitialAd(androidInterstitialAdUnit);
        interstitial_ad.LoadAd(GetAdRequest());
        StartCoroutine(WaitForIntersitialAd(interstitial_ad));

        interstitial_ad.OnAdLoaded += InterstitialAdLoaded;
        interstitial_ad.OnAdFailedToLoad += InterstitialAdLoadFailed;
        interstitial_ad.OnAdOpening += InterstitialAdOpened;
        interstitial_ad.OnAdClosed += InterstitialAdClosed;
        interstitial_ad.OnAdLeavingApplication += InterstitialAdClickLeaveApplication;

        activeInterstitial = interstitial_ad;
    }

    public AdRequest GetAdRequest()
    {
        AdRequest request = new AdRequest.Builder().Build();
        return request;
    }

    private void BannerAdLoaded(object sender, EventArgs args)
    {
        OnBannerAdLoaded.Invoke();
    }

    private void BannerAdLoadFailed(object sender, AdFailedToLoadEventArgs args)
    {
        OnBannerAdLoadFailed.Invoke();
    }

    private void BannerAdOpened(object sender, EventArgs args)
    {
        OnBannerAdOpened.Invoke();
    }

    private void BannerAdClosed(object sender, EventArgs args)
    {
        OnBannerAdClosed.Invoke();

        if (activeBanner == null) return;

        activeBanner.Destroy();
        activeBanner = null;
    }

    private void BannerAdClickLeaveApplication(object sender, EventArgs args)
    {
        OnBannerClickLeaveApplication.Invoke();

        if (activeBanner == null) return;

        activeBanner.Destroy();
        activeBanner = null;
    }

    private void InterstitialAdLoaded(object sender, EventArgs args)
    {
        OnInterstitialAdLoaded.Invoke();
    }

    private void InterstitialAdLoadFailed(object sender, AdFailedToLoadEventArgs args)
    {
        OnInterstitialAdLoadFailed.Invoke();
    }

    private void InterstitialAdOpened(object sender, EventArgs args)
    {
        OnInterstitialAdOpened.Invoke();
    }

    private void InterstitialAdClosed(object sender, EventArgs args)
    {
        OnInterstitialAdClosed.Invoke();

        if (activeInterstitial == null) return;

        activeInterstitial.Destroy();
        activeInterstitial = null;
    }

    private void InterstitialAdClickLeaveApplication(object sender, EventArgs args)
    {
        OnInterstitialClickLeaveApplication.Invoke();

        if (activeInterstitial == null) return;

        activeInterstitial.Destroy();
        activeInterstitial = null;
    }

    private IEnumerator WaitForIntersitialAd(InterstitialAd ad)
    {
        while (!ad.IsLoaded())
            yield return null;
        ad.Show();
    }
}
