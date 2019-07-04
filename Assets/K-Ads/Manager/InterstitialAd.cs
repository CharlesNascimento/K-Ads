using System;
using UnityEngine;

namespace KansusGames.KansusAds.Manager
{
    [Serializable]
    public class InterstitialAd : Ad
    {
        [SerializeField]
        private long timeCap = 0;

        public long TimeCap { get => timeCap; set => timeCap = value; }
    }
}
