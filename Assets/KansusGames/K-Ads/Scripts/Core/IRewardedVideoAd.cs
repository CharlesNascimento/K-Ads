using System;

namespace KansusGames.KansusAds.Core
{
    /// <summary>
    /// Represents a rewarded video advertisement.
    /// </summary>
    public interface IRewardedVideoAd
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
        /// <param name="onFail">Callback invoked when the ad could not be loaded. Its
        /// string parameter represents a message indicating the problem.</param>
        void Load(Action onLoad = null, Action<string> onFail = null);

        /// <summary>
        /// Presents this ad to the user.
        /// </summary>
        /// <param name="onResult">Callback invoked when the video is fully played or skipped.</param>
        /// <param name="onFail">Callback invoked when the ad could not be presented. Its
        /// string parameter represents a message indicating the problem.</param>
        void Show(Action<bool> onResult = null, Action<string> onFail = null);
    }
}