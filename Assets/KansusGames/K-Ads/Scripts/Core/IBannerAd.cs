using System;

namespace KansusGames.KansusAds.Core
{
    /// <summary>
    /// Represents a banner advertisement.
    /// </summary>
    public interface IBannerAd
    {
        /// <summary>
        /// Shows this banner ad to the user.
        /// </summary>
        /// <param name="onShow">Callback invoked when the ad is loaded and shown successfully.</param>
        /// <param name="onFailedToLoad">Callback invoked when the ad could not be loaded. Its
        /// string parameter represents a message indicating the problem.</param>
        void Show(Action onShow = null, Action<string> onFailedToLoad = null);

        /// <summary>
        /// Hides this banner ad.
        /// </summary>
        void Hide();
    }
}