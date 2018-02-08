using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;
using System;

/// <summary>
/// Script, where admob interstitial ad is realized
/// </summary>
public class AdmobImplement  {

    private string idBanner, idInterstitial;
    InterstitialAd interstitial;
    AdRequest requestInter;

    public AdmobImplement(string admobInterAndroid, string admobInterIOS)
    {
        DefineAdId(admobInterAndroid, admobInterIOS);
        InterstitialRequest();
    }

    public void Showinterstitial()
    {
        if (interstitial.IsLoaded())
            interstitial.Show();
    }

    /// <summary>
    ///  Load new interstitial ad, if it isn't loaded
    /// </summary>
    public void CheckInterstitialIsLoad()
    {
        if (!interstitial.IsLoaded())
            InterstitialRequest();
    }

    void InterstitialRequest()
    {
        interstitial = new InterstitialAd(idInterstitial);
        requestInter = new AdRequest.Builder().Build();
        interstitial.LoadAd(requestInter);
    }

    void DefineAdId(string admobInterAndroid, string admobInterIOS)
    {
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            idInterstitial = admobInterAndroid;
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            idInterstitial = admobInterAndroid;
        }

        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            idInterstitial = admobInterIOS;
        }
    }
}
