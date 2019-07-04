using KansusGames.KansusAds.Core;
using System;
using System.Collections.Generic;

namespace KansusGames.KansusAds.Adapter.UnityAds
{
    /// <summary>
    /// Unity Ads implementation of an Ad Platform.
    /// </summary>
    public class UnityAdPlatform : IAdPlatform
    {
        #region IAdPlatform

        public void Initialize(string appId, bool testMode, List<string> testDevices = null)
        {
            UnityEngine.Advertisements.Advertisement.Initialize(appId, testMode);
        }

        public IBannerAd CreateBanner(string placementId, BannerPosition adPosition)
        {
            return new UnityAdsBannerAd(placementId);
        }

        public IInterstitialAd CreateInterstitial(string placementId)
        {
            return new UnityAdsInterstitialAd(placementId);
        }

        public IRewardedVideoAd CreateRewardedVideoAd(string placementId)
        {
            return new UnityAdsRewardedVideoAd(placementId);
        }

        #endregion
    }
}