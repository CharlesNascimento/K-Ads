using GoogleMobileAds.Api;
using KansusGames.KansusAds.Core;
using UnityEngine;
using BannerPosition = KansusGames.KansusAds.Core.BannerPosition;

namespace KansusGames.KansusAds.Adapter.AdMob
{

    /// <summary>
    /// AdMob implementation of an ad network.
    /// </summary>
    public class AdMobAdNetwork : IAdNetwork
    {
        #region Fields

        private bool servePersonalizedAds;
        
        private AdMobExtras adMobConfig;

        #endregion

        #region IAdPlatform

        public void Initialize(string appId, bool servePersonalizedAds = true, AdNetworkExtras extras = null)
        {
            MobileAds.Initialize(appId);
            this.servePersonalizedAds = servePersonalizedAds;
            adMobConfig = extras as AdMobExtras;
        }

        public IBannerAd CreateBanner(string placementId, BannerPosition adPosition)
        {
            return new AdMobBannerAd(placementId, adPosition, CreateRequestBuilder);
        }

        public IInterstitialAd CreateInterstitial(string placementId)
        {
            return new AdMobInterstitialAd(placementId, CreateRequestBuilder);
        }

        public IRewardedVideoAd CreateRewardedVideoAd(string placementId)
        {
            return new AdMobRewardedVideoAd(placementId, CreateRequestBuilder);
        }

        #endregion

        #region Private methods

        private AdRequest.Builder CreateRequestBuilder()
        {
            AdRequest.Builder requestBuilder = new AdRequest.Builder();

            if (!servePersonalizedAds)
            {
                requestBuilder.AddExtra("npa", "1");
            }

            var isChildDirected = adMobConfig?.ChildDirected ?? false;

            Debug.Log("ServePersonalizedAds: " + servePersonalizedAds);
            Debug.Log("ChildDirected: " + isChildDirected);

            if (isChildDirected)
            {
                requestBuilder
                    .TagForChildDirectedTreatment(true)
                    .AddExtra("tag_for_under_age_of_consent", "true")
                    .AddExtra("max_ad_content_rating", "G");
            }

            adMobConfig?.TestDevices?.ForEach(td => requestBuilder.AddTestDevice(td));

            return requestBuilder;
        }

        #endregion
    }
}