using System.Collections.Generic;

namespace KansusGames.KansusAds.Core
{
    /// <summary>
    /// Represents an advertising network such as AdMob, Unity Ads and Chartboost.
    /// </summary>
    public interface IAdNetwork
    {
        /// <summary>
        /// Initializes the advertising network.
        /// </summary>
        /// <param name="appId">The identification of the application registered in the
        /// advertising network.</param>
        /// <param name="servePersonalizedAds">A boolean indicating whether behavioral targeting
        /// should be enabled.</param>
        /// <param name="extras">Optional ad-network-specific configuration.</param>
        void Initialize(string appId, bool servePersonalizedAds = true, AdNetworkExtras extras = null);

        /// <summary>
        /// Creates an interstitial ad for this advertising network.
        /// </summary>
        /// <param name="placementId">The identification of the ad.</param>
        /// <returns>The interstitial ad.</returns>
        IInterstitialAd CreateInterstitial(string placementId);

        /// <summary>
        /// Creates a banner ad for this advertising network.
        /// </summary>
        /// <param name="placementId">The identification of the ad.</param>
        /// <returns>The banner ad.</returns>
        IBannerAd CreateBanner(string placementId, BannerPosition adPosition);

        /// <summary>
        /// Creates a rewarded video ad for this advertising network.
        /// </summary>
        /// <param name="placementId">The identification of the ad.</param>
        /// <returns>The rewarded video ad.</returns>
        IRewardedVideoAd CreateRewardedVideoAd(string placementId);
    }
}
