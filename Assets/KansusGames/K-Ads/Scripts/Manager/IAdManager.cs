using KansusGames.KansusAds.Core;
using System;

namespace KansusGames.KansusAds.Manager
{
    /// <summary>
    /// Specifies a manager responsible for loading and showing ads.
    /// </summary>
    public interface IAdManager
    {
        /// <summary>
        /// Initializes the ad manager.
        /// </summary>
        /// <param name="servePersonalizedAds">A boolean indicating whether behavioral targeting
        /// should be enabled.</param>
        /// <param name="extras">Optional ad-network-specific configuration.</param>
        void Initialize(bool servePersonalizedAds = true, AdNetworkExtras extras = null);

        /// <summary>
        /// Shows a banner advertisement.
        /// </summary>
        /// <param name="placementId">The placement id which identifies the ad. If not provided,
        /// a default one will be used.</param>
        /// <param name="onShow">Callback invoked when the ad is shown successfully.</param>
        /// <param name="onFailedToLoad">Callback invoked when the ad could not be loaded. Its
        /// string parameter represents a message indicating the problem.</param>
        void ShowBannerAd(string placementId = null, Action onShow = null, Action<string> onFailedToLoad = null);

        /// <summary>
        /// Hides a banner ad.
        /// </summary>
        /// <param name="placementId">The placement id which identifies the ad. If not provided,
        /// a default one will be used.</param>
        void HideBannerAd(string placementId = null);

        /// <summary>
        /// Loads an interstitial ad. It is important to notice that the request may not be
        /// filled by the ad network.
        /// </summary>
        /// <param name="placementId">The placement id which identifies the ad. If not provided,
        /// a default one will be used.</param>
        /// <param name="onLoad">Callback invoked when the ad is loaded successfully.</param>
        /// <param name="onFail">Callback invoked when the ad could not be loaded. Its
        /// string parameter represents a message indicating the problem.</param>
        void LoadInterstitialAd(Action onLoad = null, Action<string> onFail = null, string placementId = null);

        /// <summary>
        /// Show a loaded interstitial ad.
        /// </summary>
        /// <param name="onClose">Callback invoked when the ad is closed.</param>
        /// <param name="onFail">Callback invoked when the ad could not be presented. Its
        /// string parameter represents a message indicating the problem.</param>
        /// <param name="placementId">The placement id which identifies the ad. If not provided,
        /// a default one will be used.</param>
        void ShowInterstitialAd(Action onClose = null, Action<string> onFail = null, string placementId = null);

        /// <summary>
        /// Loads a rewarded video ad. It is important to notice that the request may not be
        /// filled by the ad network.
        /// </summary>
        /// <param name="onLoad">Callback invoked when the ad is loaded successfully.</param>
        /// <param name="onFail">Callback invoked when the ad could not be loaded. Its
        /// string parameter represents a message indicating the problem.</param>
        /// <param name="placementId">The placement id which identifies the ad. If not provided,
        /// a default one will be used.</param>
        void LoadRewardedVideoAd(Action onLoad = null, Action<string> onFail = null, string placementId = null);

        /// <summary>
        /// Show a loaded rewarded video ad.
        /// </summary>
        /// <param name="onResult">Callback invoked when the video is fully played or skipped.</param>
        /// <param name="onFail">Callback invoked when the ad could not be presented. Its
        /// string parameter represents a message indicating the problem.</param>
        /// <param name="placementId">The placement id which identifies the ad. If not provided,
        /// a default one will be used.</param>
        void ShowRewardedVideoAd(Action<bool> onResult = null, Action<string> onFail = null, string placementId = null);
    }
}
