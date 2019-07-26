using System;
using UnityEngine;

namespace KansusGames.KansusAds.Manager
{
    /// <summary>
    /// Represents an interstitial advertisement.
    /// </summary>
    [Serializable]
    public class InterstitialAd : Ad
    {
        [SerializeField]
        [Tooltip("Minimum time interval between the previous and the next presentation of this ad.")]
        private long timeCap = 0;

        public long TimeCap { get => timeCap; set => timeCap = value; }
    }
}
