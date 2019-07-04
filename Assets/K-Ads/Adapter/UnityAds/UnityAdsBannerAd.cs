using KansusGames.KansusAds.Core;
using System;
using UnityEngine;

namespace KansusGames.KansusAds.Adapter.UnityAds
{
    /// <summary>
    /// Represents a banner advertisement by the Unity Ads network.
    /// </summary>
    public class UnityAdsBannerAd : IBannerAd
    {
        #region Fields

        private readonly string placementId;

        #endregion

        #region Initialization

        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        /// <param name="placementId">The placement id for this banner ad.</param>
        public UnityAdsBannerAd(string placementId)
        {
            this.placementId = placementId;
        }

        #endregion

        #region IInterstitialAd

        public void Show(Action onShow = null, Action<string> onFailedToLoad = null)
        {
            var message = "Unity Ads currently does not support banner ads.";

            Debug.LogError(message);

            onFailedToLoad?.Invoke(message);
        }

        public void Hide()
        {
            Debug.LogError("Unity Ads currently does not support banner ads.");
        }

        #endregion
    }
}