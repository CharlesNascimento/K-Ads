using System.Collections.Generic;
using UnityEngine;

namespace KansusGames.KansusAds.Manager
{
    [CreateAssetMenu(fileName = "Ad Manager Settings", menuName = "Kansus Games/K-Ads/Ad Manager Settings", order = 100)]
    public class AdManagerSettings : ScriptableObject
    {
        #region Fields

        [SerializeField]
        private string appId;

        [SerializeField]
        private bool testMode = false;

        [SerializeField]
        private List<string> testDevices;

        [SerializeField]
        private List<BannerAd> bannerAds;

        [SerializeField]
        private List<Ad> interstitalAds;

        [SerializeField]
        private List<Ad> rewardedVideoAds;

        #endregion

        #region Properties

        public string AppId { get => appId; }
        public bool TestMode { get => testMode; }
        public List<string> TestDevices { get => testDevices; }
        public List<BannerAd> BannerAds { get => bannerAds; }
        public List<Ad> InterstitalAds { get => interstitalAds; }
        public List<Ad> RewardedVideoAds { get => rewardedVideoAds; }

        #endregion
    }
}
