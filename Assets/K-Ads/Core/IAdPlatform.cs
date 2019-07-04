using System.Collections.Generic;

namespace KansusGames.KansusAds.Core
{
    /// <summary>
    /// Represents an advertising network such as AdMob, Unity Ads and Chartboost.
    /// </summary>
    public interface IAdPlatform
    {
        /// <summary>
        /// Initializes the advertising platform.
        /// </summary>
        /// <param name="appId">The identification of the application registered in the
        /// advertising platform.</param>
        /// <param name="testMode">Tells whether this platform should be consumed in test mode.</param>
        /// <param name="testDevices">A list of test devices which should be only served
        /// with test ads.</param>
        void Initialize(string appId, bool testMode, List<string> testDevices = null);

        /// <summary>
        /// Creates an interstitial ad for this advertising platform.
        /// </summary>
        /// <param name="placementId">The identification of the ad.</param>
        /// <returns>The interstitial ad.</returns>
        IInterstitialAd CreateInterstitial(string placementId);

        /// <summary>
        /// Creates a banner ad for this advertising platform.
        /// </summary>
        /// <param name="placementId">The identification of the ad.</param>
        /// <returns>The banner ad.</returns>
        IBannerAd CreateBanner(string placementId, BannerPosition adPosition);

        /// <summary>
        /// Creates a rewarded video ad for this advertising platform.
        /// </summary>
        /// <param name="placementId">The identification of the ad.</param>
        /// <returns>The rewarded video ad.</returns>
        IRewardedVideoAd CreateRewardedVideoAd(string placementId);
    }
}
