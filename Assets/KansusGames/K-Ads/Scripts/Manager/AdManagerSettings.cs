using System.Collections.Generic;
using UnityEngine;

namespace KansusGames.KansusAds.Manager
{
    /// <summary>
    /// Holds the configuration of an ad manager.
    /// </summary>
    [CreateAssetMenu(fileName = "Ad Manager Settings", menuName = "Kansus Games/K-Ads/Ad Manager Settings", order = 100)]
    public class AdManagerSettings : ScriptableObject
    {
        #region Fields

        [SerializeField]
        [Tooltip("The identifier of the application in the underlying ad network.")]
        private string appId;

        [SerializeField]
        [Tooltip("A list of banner placement IDs the manager will serve.")]
        private List<BannerAd> bannerAds;

        [SerializeField]
        [Tooltip("A list of interstitial placement IDs the manager will serve.")]
        private List<InterstitialAd> interstitalAds;

        [SerializeField]
        [Tooltip("A list of rewarded video placement IDs the manager will serve.")]
        private List<Ad> rewardedVideoAds;

        #endregion

        #region Properties

        public string AppId { get => appId; }
        public List<BannerAd> BannerAds { get => bannerAds; }
        public List<InterstitialAd> InterstitalAds { get => interstitalAds; }
        public List<Ad> RewardedVideoAds { get => rewardedVideoAds; }

        #endregion
    }
}
