using KansusGames.KansusAds.Core;
using System;
using UnityEngine;

namespace KansusGames.KansusAds.Manager
{
    /// <summary>
    /// Represents a banner advertisement.
    /// </summary>
    [Serializable]
    public class BannerAd : Ad
    {
        [SerializeField]
        [Tooltip("The screen position where the banner will be shown.")]
        private BannerPosition adPosition;

        public BannerPosition AdPosition { get => adPosition; }
    }
}
