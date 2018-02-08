using UnityEngine;
using System.Collections;

/// <summary>
/// Class in charge to ads showing
/// </summary>
/// <remarks>
///  Attached to AdsManager gameObject
/// </remarks>
public class AdsManager : PersistentSingleton<AdsManager> {

    [Header("UnityAds")]
    public string unityGameIdAndroid;
    public string unityGameIdIOS;
    [Space(10)]

    [Space(5)]
    //quantity of interstitial ad shows
    public int qtyOfGamesToShowInter = 3;
    [Header("Admob")]
    public string admobInterAndroid;
    public string admobInterIOS;

    UnityAdsImplement unityAds;
    AdmobImplement admob;
   
    void Start ()
    {
        unityAds = new UnityAdsImplement(unityGameIdAndroid, unityGameIdIOS);
        admob = new AdmobImplement(admobInterAndroid, admobInterIOS);
    }

    /// <summary>
    /// Show Interstitial ad
    /// </summary>
    public void ShowInterstitial(int gamesCount)
    {

        admob.CheckInterstitialIsLoad();
        if (gamesCount % qtyOfGamesToShowInter == 0)
        {
            Debug.Log("<color=yellow>ShowInterstitial</color>");
            admob.Showinterstitial();
        }
    }

    /// <summary>
    /// Show rewarded video
    /// </summary>
    public void ShowRewardedVideo()
    {
        unityAds.ShowVideoAd();
    }

    /// <summary>
    /// Check if is ready video add to be shown
    /// </summary>
    /// <returns>Is rady ad or not</returns>
    public bool IsReadyAd()
    {
        return unityAds.IsReadyAd();
    }
}
