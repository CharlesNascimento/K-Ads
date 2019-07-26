using KansusGames.KansusAds.Core;

namespace KansusGames.KansusAds.Adapter.UnityAds
{
    /// <summary>
    /// Extra configuration for the Unity Ads ad network.
    /// </summary>
    public class UnityAdsExtras : AdNetworkExtras
    {
        /// <summary>
        /// Indicates whether Unity Ads should run in test mode.
        /// </summary>
        public bool TestMode { get; set; }

        public UnityAdsExtras(bool testMode)
        {
            TestMode = testMode;
        }
    }
}