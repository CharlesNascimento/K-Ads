using KansusGames.KansusAds.Core;
using UnityEngine.Advertisements;

namespace KansusGames.KansusAds.Adapter.UnityAds
{
    /// <summary>
    /// Unity Ads implementation of an ad network.
    /// </summary>
    public class UnityAdNetwork : IAdNetwork
    {
        #region IAdPlatform

        public void Initialize(string appId, bool servePersonalizedAds = true, AdNetworkExtras extras = null)
        {
            MetaData gdprMetaData = new MetaData("gdpr");
            gdprMetaData.Set("consent", servePersonalizedAds.ToString());
            Advertisement.SetMetaData(gdprMetaData);

            var testMode = (extras as UnityAdsExtras)?.TestMode ?? false;

            Advertisement.Initialize(appId, testMode);
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