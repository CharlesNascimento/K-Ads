using KansusGames.KansusAds.Core;
using System.Collections.Generic;

namespace KansusGames.KansusAds.Adapter.AdMob
{
    /// <summary>
    /// Extra configuration for the AdMob ad network.
    /// </summary>
    public class AdMobExtras : AdNetworkExtras
    {
        /// <summary>
        /// Indicates whether AdMob should only serve child-directed advertisements.
        /// </summary>
        public bool ChildDirected { get; set; }

        /// <summary>
        /// A list of device identifiers which should only receive test ads.
        /// </summary>
        public List<string> TestDevices { get; set; }

        public AdMobExtras(bool childDirected, List<string> testDevices = null)
        {
            ChildDirected = childDirected;
            TestDevices = testDevices;
        }
    }
}