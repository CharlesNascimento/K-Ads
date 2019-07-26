using KansusGames.KansusAds.Adapter.AdMob;
using KansusGames.KansusAds.Core;
using System.Collections.Generic;
using UnityEngine;

namespace KansusGames.KansusAds.Demo
{
    public class AdMobDemo : DemoBase<AdMobAdNetwork>
    {
        [SerializeField]
        protected bool childDirected = false;

        [SerializeField]
        protected List<string> testDevices;

        protected override AdNetworkExtras CreateExtras()
        {
            return new AdMobExtras(childDirected, testDevices);
        }
    }
}
