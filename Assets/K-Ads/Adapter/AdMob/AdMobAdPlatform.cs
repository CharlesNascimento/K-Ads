using GoogleMobileAds.Api;
using KansusGames.KansusAds.Core;
using System.Collections.Generic;
using BannerPosition = KansusGames.KansusAds.Core.BannerPosition;

namespace KansusGames.KansusAds.Adapter.AdMob
{
    /// <summary>
    /// AdMob implementation of an Ad Platform.
    /// </summary>
    public class AdMobAdPlatform : IAdPlatform
    {
        #region Fields

        private AdRequestBuilderFactory adRequestBuilderFactory;

        #endregion

        #region IAdPlatform

        public void Initialize(string appId, bool testMode, List<string> testDevices = null)
        {
            MobileAds.Initialize(appId);
            adRequestBuilderFactory = new AdRequestBuilderFactory(testDevices);
        }

        public IBannerAd CreateBanner(string placementId, BannerPosition adPosition)
        {
            return new AdMobBannerAd(placementId, adPosition, adRequestBuilderFactory);
        }

        public IInterstitialAd CreateInterstitial(string placementId)
        {
            return new AdMobInterstitialAd(placementId, adRequestBuilderFactory);
        }

        public IRewardedVideoAd CreateRewardedVideoAd(string placementId)
        {
            return new AdMobRewardedVideoAd(placementId, adRequestBuilderFactory);
        }

        #endregion

        #region AdRequestBuilderFactory

        public class AdRequestBuilderFactory
        {
            private List<string> testDevices;

            public AdRequestBuilderFactory(List<string> testDevices)
            {
                this.testDevices = testDevices;
            }

            public AdRequest.Builder CreateBuilder()
            {
                AdRequest.Builder requestBuilder = new AdRequest.Builder();

                testDevices.ForEach(td => requestBuilder.AddTestDevice(td));

                return requestBuilder;
            }
        }

        #endregion
    }
}