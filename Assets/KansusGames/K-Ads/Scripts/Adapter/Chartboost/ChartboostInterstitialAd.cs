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

        private readonly string placementId;

        private Action onLoadCallback;
        private Action<string> onFailedToLoadCallback;

        private Action onOpeningCallback;
        private Action onCloseCallback;

        #endregion

        #region Initialization

        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        /// <param name="placementId">The placement id for this ad.</param>
        public ChartboostInterstitialAd(string placementId)
        {
            this.placementId = placementId;
        }

        #endregion

        #region IInterstitialAd

        public bool IsLoaded()
        {
            return ChartboostSDK.Chartboost.hasInterstitial(CBLocation.Default);
        }

        public void Load(Action onLoad = null, Action<string> onFailedToLoad = null)
        {
            onLoadCallback = onLoad;
            onFailedToLoadCallback = onFailedToLoad;

            ChartboostSDK.Chartboost.didCacheInterstitial += LoadCallback;
            ChartboostSDK.Chartboost.didFailToLoadInterstitial += LoadFailedCallback;

            ChartboostSDK.Chartboost.cacheInterstitial(CBLocation.Default);
        }

        public void Show(Action onOpening = null, Action onClose = null)
        {
            if (!IsLoaded())
            {
                Debug.LogWarning("Interstitial ad not loaded");
                return;
            }

            onOpeningCallback = onOpening;
            onCloseCallback = onClose;

            ChartboostSDK.Chartboost.didClickInterstitial += OpeningCallback;
            ChartboostSDK.Chartboost.didDismissInterstitial += CloseCallback;

            ChartboostSDK.Chartboost.showInterstitial(CBLocation.Default);
        }

        private void LoadFailedCallback(CBLocation location, CBImpressionError error)
        {
            ClearLoadCallbacks();

            Debug.LogWarning("Failed to load Chartboost interstitial: " + error.ToString());

            onFailedToLoadCallback?.Invoke(error.ToString());
        }

        private void LoadCallback(CBLocation location)
        {
            ClearLoadCallbacks();

            Debug.Log("Chartboost interstitial loaded successfully");

            onLoadCallback?.Invoke();
        }

        private void OpeningCallback(CBLocation location)
        {
            ClearShowCallbacks();

            Debug.Log("Opening Chartboost interstitial ad");

            onOpeningCallback?.Invoke();
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
            ChartboostSDK.Chartboost.didClickInterstitial -= OpeningCallback;
            ChartboostSDK.Chartboost.didDismissInterstitial -= CloseCallback;
        }

        #endregion
    }
}