using KansusGames.KansusAds.Core;
using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace KansusGames.KansusAds.Adapter.UnityAds
{
    /// <summary>
    /// Represents an interstitial advertisement by the Unity Ads network.
    /// </summary>
    public class UnityAdsInterstitialAd : IInterstitialAd
    {
        #region Fields

        private readonly string placementId;

        #endregion

        #region Initialization

        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        /// <param name="placementId">The placement id for this ad.</param>
        public UnityAdsInterstitialAd(string placementId)
        {
            this.placementId = placementId;
        }

        #endregion

        #region IInterstitialAd

        public bool IsLoaded()
        {
            return Advertisement.IsReady(placementId);
        }

        public void Load(Action onLoad = null, Action<string> onFail = null)
        {
            if (Advertisement.IsReady(placementId))
            {
                Debug.Log("Unity Ads interstitial loaded successfully");
                onLoad?.Invoke();
            }
            else
            {
                var message = "Unity Ads interstitial ad could not be loaded";
                Debug.LogWarning(message);
                onFail?.Invoke(message);
            }
        }

        public void Show(Action onClose = null, Action<string> onFail = null)
        {
            var options = new ShowOptions
            {
                resultCallback = (result) =>
                {
                    if (result == ShowResult.Finished || result == ShowResult.Skipped)
                    {
                        Debug.Log("Unty Ads interstitial ad closed");
                        onClose?.Invoke();
                    }
                    else
                    {
                        onFail?.Invoke("Failed to show Unity Ads interstitial ad");
                    }
                }
            };

            Advertisement.Show(placementId, options);
        }

        #endregion
    }
}