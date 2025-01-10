using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdManager : MonoBehaviour
{
    private float originalVolume;

    public AudioSource audioSource;
    public AudioClip levelUpSound;

    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;

    private void Start()
    {
        originalVolume = AudioListener.volume;

        MobileAds.Initialize(initStatus => { });

        InterstitialAd.Load("ca-app-pub-3304006097050661/1203863547", new AdRequest.Builder().Build(),
     (InterstitialAd ad, LoadAdError error) =>
     {
         if (error != null || ad == null)
         {
             Debug.LogError("Failed to load interstitial ad with error: " + error);
             return;
         }
         interstitial = ad;
         interstitial.OnAdFullScreenContentClosed += HandleOnAdClosed;
     });

        RewardedAd.Load("ca-app-pub-3304006097050661/3067609558", new AdRequest.Builder().Build(),
    (RewardedAd ad, LoadAdError error) =>
    {
        if (error != null || ad == null)
        {
            Debug.LogError("Failed to load rewarded ad with error: " + error);
            return;
        }
        rewardedAd = ad;
        rewardedAd.OnAdFullScreenContentClosed += HandleOnAdClosed;

        Invoke("ShowInterstatial", 1f);
    });
    }

    private void HandleOnAdClosed()
    {
        AudioListener.volume = originalVolume;
        Time.timeScale = 1;
    }

    private void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        AudioListener.volume = originalVolume;
        Time.timeScale = 1;
    }

    public void ShowInterstatial()
    {
        interstitial.Show();
    }
}