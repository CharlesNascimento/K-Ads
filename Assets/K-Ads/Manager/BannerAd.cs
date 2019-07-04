using KansusGames.KansusAds.Core;
using System;
using UnityEngine;

namespace KansusGames.KansusAds.Manager
{
    [Serializable]
    public class BannerAd : Ad
    {
        [SerializeField]
        private BannerPosition adPosition;

        public BannerPosition AdPosition { get => adPosition; }
    }
}
