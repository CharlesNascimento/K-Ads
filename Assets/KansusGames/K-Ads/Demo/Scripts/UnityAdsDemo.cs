using KansusGames.KansusAds.Adapter.UnityAds;
using KansusGames.KansusAds.Core;
using UnityEngine;

namespace KansusGames.KansusAds.Demo
{
    public class UnityAdsDemo : DemoBase<UnityAdNetwork>
    {
        [SerializeField]
        protected bool testMode = true;

        protected override AdNetworkExtras CreateExtras()
        {
            return new UnityAdsExtras(testMode);
        }
    }
}