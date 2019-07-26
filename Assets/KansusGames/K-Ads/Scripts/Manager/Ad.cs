using System;
using UnityEngine;

namespace KansusGames.KansusAds.Manager
{
    /// <summary>
    /// Represents an advertisement.
    /// </summary>
    [Serializable]
    public class Ad
    {
        [SerializeField]
        [Tooltip("The identifier of this ad in the ad network.")]
        private string placementId;

        [SerializeField]
        [Tooltip("Whether this ad should be automatically loaded on manager initialization or after it finishes.")]
        private bool loadAutomatically = true;

        public string PlacementId { get => placementId; }
        public bool LoadAutomatically { get => loadAutomatically; }
    }
}
