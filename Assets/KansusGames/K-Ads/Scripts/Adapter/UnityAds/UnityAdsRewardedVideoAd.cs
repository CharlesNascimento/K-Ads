using KansusGames.KansusAds.Core;
using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace KansusGames.KansusAds.Adapter.UnityAds
{
    /// <summary>
    /// Represents a rewarded video advertisement by the Unity Ads network.
    /// </summary>
    public class UnityAdsRewardedVideoAd : IRewardedVideoAd
    {
        #region Fields

        private readonly string placementId;

        #endregion

        #region Initialization

        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        /// <param name="placementId">The placement id for this ad.</param>
        public UnityAdsRewardedVideoAd(string placementId)
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
                Debug.Log("Unity Ads rewarded video loaded successfully");
                onLoad?.Invoke();
            }
            else
            {
                const string message = "Unity Ads rewarded video ad could not be loaded";
                Debug.LogWarning(message);
                onFail?.Invoke(message);
            }
        }

        public void Show(Action<bool> onResult = null, Action<string> onFail = null)
        {
            var options = new ShowOptions
            {
                resultCallback = (result) =>
                {
                    if (result == ShowResult.Finished)
                    {
                        Debug.Log("Unity Ads rewarded video ad completed");
                        onResult?.Invoke(true);
                    }
                    else if (result == ShowResult.Skipped)
                    {
                        Debug.Log("Unity Ads rewarded video ad skipped");
                        onResult?.Invoke(false);
                    }
                    else
                    {
                        const string message = "Failed to show Unity Ads rewarded video ad";
                        Debug.Log(message);
                        onFail?.Invoke(message);
                    }
                }
            };

            Advertisement.Show(placementId, options);
        }

        #endregion
    }
}