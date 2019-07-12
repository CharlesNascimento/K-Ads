using System;

namespace KansusGames.KansusAds.Core
{
    /// <summary>
    /// Represents an interstitial advertisement.
    /// </summary>
    public interface IInterstitialAd
    {
        /// <summary>
        /// Checks whether this ad is loaded and ready to be presented.
        /// </summary>
        /// <returns>A boolean indicating whether this ad is loaded.</returns>
        bool IsLoaded();

        /// <summary>
        /// Loads this ad from its network. It is important to notice that the request may not be
        /// filled.
        /// </summary>
        /// <param name="onLoad">Callback invoked when the ad is loaded successfully.</param>
        /// <param name="onFailedToLoad">Callback invoked when the ad could not be loaded. Its
        /// string parameter represents a message indicating the problem.</param>
        void Load(Action onLoad = null, Action<string> onFailedToLoad = null);

        /// <summary>
        /// Presents this ad to the user.
        /// </summary>
        /// <param name="onOpening">Callback invoked when the user opens the ad.</param>
        /// <param name="onClose">Callback invoked when the ad is closed.</param>
        void Show(Action onOpening = null, Action onClose = null);
    }
}