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

        private readonly CBLocation location;

        private Action onLoadCallback;
        private Action<string> onFail;

        private Action<bool> onResult;

        #endregion

        #region Initialization

        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        /// <param name="placementId">The placement id for this ad.</param>
        public ChartboostRewardedVideoAd(string placementId)
        {
            location = CBLocation.locationFromName(placementId);
        }

        #endregion

        #region IRewardedVideoAd

        public bool IsLoaded()
        {
            return ChartboostSDK.Chartboost.hasRewardedVideo(location);
        }

        public void Load(Action onLoad = null, Action<string> onFail = null)
        {
            onLoadCallback = onLoad;
            this.onFail = onFail;

            ChartboostSDK.Chartboost.didCacheRewardedVideo += LoadCallback;
            ChartboostSDK.Chartboost.didFailToLoadRewardedVideo += LoadFailedCallback;

            ChartboostSDK.Chartboost.cacheRewardedVideo(location);
        }

        public void Show(Action<bool> onResult = null, Action<string> onFail = null)
        {
            if (!IsLoaded())
            {
                Debug.LogWarning("Rewarded video ad not loaded");
                return;
            }

            this.onResult = onResult;
            this.onFail = onFail;

            ChartboostSDK.Chartboost.didCompleteRewardedVideo += EarnRewardCallback;
            ChartboostSDK.Chartboost.didDismissRewardedVideo += SkipCallback;
            ChartboostSDK.Chartboost.didFailToLoadRewardedVideo += LoadFailedCallback;

            ChartboostSDK.Chartboost.showRewardedVideo(location);
        }

        #endregion

        #region Private methods

        private void LoadFailedCallback(CBLocation location, CBImpressionError error)
        {
            ClearLoadCallbacks();

            Debug.LogWarning("Failed to load Chartboost rewarded video: " + error.ToString());

            onFail?.Invoke(error.ToString());
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

            onResult?.Invoke(true);
        }

        private void SkipCallback(CBLocation location)
        {
            ClearShowCallbacks();

            Debug.Log("Chartboost rewarded video ad skipped");

            onResult?.Invoke(false);
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
            ChartboostSDK.Chartboost.didFailToLoadRewardedVideo -= LoadFailedCallback;
        }

        #endregion
    }
}