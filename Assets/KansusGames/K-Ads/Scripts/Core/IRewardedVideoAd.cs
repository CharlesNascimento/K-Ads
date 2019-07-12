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
        /// <param name="onFailedToLoad">Callback invoked when the ad could not be loaded. Its
        /// string parameter represents a message indicating the problem.</param>
        void Load(Action onLoad = null, Action<string> onFailedToLoad = null);

        /// <summary>
        /// Presents this ad to the user.
        /// </summary>
        /// <param name="onEarnReward">Callback invoked when the video is fully played.</param>
        /// <param name="onSkip">Callback invoked when the ad is closed.</param>
        void Show(Action onEarnReward = null, Action onSkip = null);
    }
}