using System;
using UnityEngine;
#if PLAYMARKET
using YandexMobileAds;
using YandexMobileAds.Base;
#endif

public class YandexAds : MonoBehaviour
{
    private String message = "";

#if PLAYMARKET
    private InterstitialAdLoader interstitialAdLoader;
    private Interstitial interstitial;
#endif

    public void Awake()
    {
#if PLAYMARKET
        this.interstitialAdLoader = new InterstitialAdLoader();
        this.interstitialAdLoader.OnAdLoaded += this.HandleAdLoaded;
        this.interstitialAdLoader.OnAdFailedToLoad += this.HandleAdFailedToLoad;
#endif
    }

    private void Start()
    {
#if PLAYMARKET
        DontDestroyOnLoad(gameObject);
        RequestInterstitial();
#else
        Destroy(gameObject);
#endif
    }

#if PLAYMARKET
private void RequestInterstitial()
    {
        //Sets COPPA restriction for user age under 13
        MobileAds.SetAgeRestrictedUser(true);

        // Replace demo Unit ID 'demo-interstitial-yandex' with actual Ad Unit ID
        //REAL ID - 'R-M-17574579-1'
        string adUnitId = "R-M-17574579-1";

        if (this.interstitial != null)
        {
            this.interstitial.Destroy();
        }

        this.interstitialAdLoader.LoadAd(this.CreateAdRequest(adUnitId));
        this.DisplayMessage("Interstitial is requested");
    }

    public void ShowInterstitial()
    {
        if (this.interstitial == null)
        {
            this.DisplayMessage("Interstitial is not ready yet");
            return;
        }

        this.interstitial.OnAdClicked += this.HandleAdClicked;
        this.interstitial.OnAdShown += this.HandleAdShown;
        this.interstitial.OnAdFailedToShow += this.HandleAdFailedToShow;
        this.interstitial.OnAdImpression += this.HandleImpression;
        this.interstitial.OnAdDismissed += this.HandleAdDismissed;
        
        this.interstitial.Show();
    }

    private AdRequestConfiguration CreateAdRequest(string adUnitId)
    {
        return new AdRequestConfiguration.Builder(adUnitId).Build();
    }

    private void DisplayMessage(String message)
    {
        this.message = message + (this.message.Length == 0 ? "" : "\n--------\n" + this.message);
        MonoBehaviour.print(message);
    }

    #region Interstitial callback handlers

    public void HandleAdLoaded(object sender, InterstitialAdLoadedEventArgs args)
    {
        this.DisplayMessage("HandleAdLoaded event received");

        this.interstitial = args.Interstitial;
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        this.DisplayMessage($"HandleAdFailedToLoad event received with message: {args.Message}");

        PauseScript.Instance.UnPause();
    }
    public void HandleAdClicked(object sender, EventArgs args)
    {
        this.DisplayMessage("HandleAdClicked event received");
    }

    public void HandleAdShown(object sender, EventArgs args)
    {
        this.DisplayMessage("HandleAdShown event received");

        ButtonFunctions.Instance.Pause();
    }

    public void HandleAdDismissed(object sender, EventArgs args)
    {
        this.DisplayMessage("HandleAdDismissed event received");

        this.interstitial.Destroy();
        this.interstitial = null;

        PauseScript.Instance.UnPause();
    }

    public void HandleImpression(object sender, ImpressionData impressionData)
    {
        var data = impressionData == null ? "null" : impressionData.rawData;
        this.DisplayMessage($"HandleImpression event received with data: {data}");
    }

    public void HandleAdFailedToShow(object sender, AdFailureEventArgs args)
    {
        this.DisplayMessage($"HandleAdFailedToShow event received with message: {args.Message}");

        PauseScript.Instance.UnPause();
    }
    
    #endregion
#endif
}