using ChartboostSDK;
using KansusGames.KansusAds.Core;
using System;
using UnityEngine;

namespace KansusGames.KansusAds.Adapter.Chartboost
{
    /// <summary>
    /// Represents an interstitial advertisement by the Chartboost network.
    /// </summary>
    public class ChartboostInterstitialAd : IInterstitialAd
    {
        #region Fields

        private readonly CBLocation location;

        private Action onLoadCallback;
        private Action<string> onFailCallback;

        private Action onCloseCallback;

        #endregion

        #region Initialization

        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        /// <param name="placementId">The placement id for this ad.</param>
        public ChartboostInterstitialAd(string placementId)
        {
            location = CBLocation.locationFromName(placementId);
        }

        #endregion

        #region IInterstitialAd

        public bool IsLoaded()
        {
            return ChartboostSDK.Chartboost.hasInterstitial(location);
        }

        public void Load(Action onLoad = null, Action<string> onFail = null)
        {
            onLoadCallback = onLoad;
            onFailCallback = onFail;

            ChartboostSDK.Chartboost.didCacheInterstitial += LoadCallback;
            ChartboostSDK.Chartboost.didFailToLoadInterstitial += LoadFailedCallback;

            ChartboostSDK.Chartboost.cacheInterstitial(location);
        }

        public void Show(Action onClose = null, Action<string> onFail = null)
        {
            if (!IsLoaded())
            {
                Debug.LogWarning("Interstitial ad not loaded");
                return;
            }

            onCloseCallback = onClose;
            onFailCallback = onFail;

            ChartboostSDK.Chartboost.didDismissInterstitial += CloseCallback;
            ChartboostSDK.Chartboost.didFailToLoadInterstitial += LoadFailedCallback;

            ChartboostSDK.Chartboost.showInterstitial(location);
        }

        private void LoadFailedCallback(CBLocation location, CBImpressionError error)
        {
            ClearLoadCallbacks();

            Debug.LogWarning("Failed to load Chartboost interstitial: " + error.ToString());

            onFailCallback?.Invoke(error.ToString());
        }

        private void LoadCallback(CBLocation location)
        {
            ClearLoadCallbacks();

            Debug.Log("Chartboost interstitial loaded successfully");

            onLoadCallback?.Invoke();
        }

        private void CloseCallback(CBLocation location)
        {
            ClearShowCallbacks();

            Debug.Log("Chartboost interstitial ad closed");

            onCloseCallback?.Invoke();
        }

        private void ClearLoadCallbacks()
        {
            ChartboostSDK.Chartboost.didCacheInterstitial -= LoadCallback;
            ChartboostSDK.Chartboost.didFailToLoadInterstitial -= LoadFailedCallback;
        }

        private void ClearShowCallbacks()
        {
            ChartboostSDK.Chartboost.didFailToLoadInterstitial -= LoadFailedCallback;
            ChartboostSDK.Chartboost.didDismissInterstitial -= CloseCallback;
        }

        #endregion
    }
}