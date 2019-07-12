using ChartboostSDK;
using KansusGames.KansusAds.Core;
using System;
using UnityEngine;

namespace KansusGames.KansusAds.Adapter.Chartboost
{
    /// <summary>
    /// Represents a rewarded video advertisement by the Chartboost network.
    /// </summary>
    public class ChartboostRewardedVideoAd : IRewardedVideoAd
    {
        #region Fields

        private readonly string placementId;

        private Action onLoadCallback;
        private Action<string> onFailedToLoadCallback;

        private Action onEarnRewardCallback;
        private Action onSkipCallback;

        #endregion

        #region Initialization

        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        /// <param name="placementId">The placement id for this ad.</param>
        public ChartboostRewardedVideoAd(string placementId)
        {
            this.placementId = placementId;
        }

        #endregion

        #region IRewardedVideoAd

        public bool IsLoaded()
        {
            return ChartboostSDK.Chartboost.hasRewardedVideo(CBLocation.Default);
        }

        public void Load(Action onLoad = null, Action<string> onFailedToLoad = null)
        {
            onLoadCallback = onLoad;
            onFailedToLoadCallback = onFailedToLoad;

            ChartboostSDK.Chartboost.didCacheRewardedVideo += LoadCallback;
            ChartboostSDK.Chartboost.didFailToLoadRewardedVideo += LoadFailedCallback;

            ChartboostSDK.Chartboost.cacheRewardedVideo(CBLocation.Default);
        }

        public void Show(Action onEarnReward = null, Action onSkip = null)
        {
            if (!IsLoaded())
            {
                Debug.LogWarning("Rewarded video ad not loaded");
                return;
            }

            onEarnRewardCallback = onEarnReward;
            onSkipCallback = onSkip;

            ChartboostSDK.Chartboost.didCompleteRewardedVideo += EarnRewardCallback;
            ChartboostSDK.Chartboost.didDismissRewardedVideo += SkipCallback;

            ChartboostSDK.Chartboost.showRewardedVideo(CBLocation.Default);
        }

        #endregion

        #region Private methods

        private void LoadFailedCallback(CBLocation location, CBImpressionError error)
        {
            ClearLoadCallbacks();

            Debug.LogWarning("Failed to load Chartboost rewarded video: " + error.ToString());

            onFailedToLoadCallback?.Invoke(error.ToString());
        }

        private void LoadCallback(CBLocation location)
        {
            ClearLoadCallbacks();

            Debug.Log("Chartboost rewarded video loaded successfully");

            onLoadCallback?.Invoke();
        }

        private void EarnRewardCallback(CBLocation location, int amount)
        {
            ClearShowCallbacks();

            Debug.Log("Chartboost rewarded video ad completed");

            onEarnRewardCallback?.Invoke();
        }

        private void SkipCallback(CBLocation location)
        {
            ClearShowCallbacks();

            Debug.Log("Chartboost rewarded video ad skipped");

            onSkipCallback?.Invoke();
        }

        private void ClearLoadCallbacks()
        {
            ChartboostSDK.Chartboost.didCacheRewardedVideo -= LoadCallback;
            ChartboostSDK.Chartboost.didFailToLoadRewardedVideo -= LoadFailedCallback;
        }

        private void ClearShowCallbacks()
        {
            ChartboostSDK.Chartboost.didCompleteRewardedVideo -= EarnRewardCallback;
            ChartboostSDK.Chartboost.didDismissRewardedVideo -= SkipCallback;
        }

        #endregion
    }
}