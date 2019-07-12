using System;
using UnityEngine;

namespace KansusGames.KansusAds.Manager
{
    [Serializable]
    public class Ad
    {
        [SerializeField]
        private string placementId;

        [SerializeField]
        private bool loadAutomatically = true;

        public string PlacementId { get => placementId; }
        public bool LoadAutomatically { get => loadAutomatically; }
    }
}
