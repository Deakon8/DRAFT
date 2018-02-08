using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;
using System;

/// <summary>
/// In charge of UnityAds video realization
/// </summary>
public class UnityAdsImplement  {

    private string gameId;
    private readonly string unityVideoZone = "defaultZone";
    public UnityAdsImplement(string gameIdAndroid, string gameIdIOS)
    {
        DefineAdId(gameIdAndroid, gameIdIOS);
        Advertisement.Initialize(gameId);
    }

    public void ShowVideoAd()
    {
        Advertisement.Show(unityVideoZone, new ShowOptions
        {
            resultCallback = result => {
                //code after user finished to whatch ad
                MenuManager.Instance.AfterVideoAd();
            }
        });
    }
    public bool IsReadyAd()
    {
        if (!Advertisement.isInitialized)
            Advertisement.Initialize(gameId);

        return Advertisement.IsReady(unityVideoZone);
    }

    void DefineAdId(string gameIdAndroid, string gameIdIOS)
    {
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
            gameId = gameIdAndroid;

        else if (Application.platform == RuntimePlatform.Android)
            gameId = gameIdAndroid;


        else if (Application.platform == RuntimePlatform.IPhonePlayer)
            gameId = gameIdIOS;
    }
}
