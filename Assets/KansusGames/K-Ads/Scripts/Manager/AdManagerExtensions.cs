using KansusGames.KansusAds.Core;
using System;

namespace KansusGames.KansusAds.Manager
{
    public static class AdManagerExtensions
    {
        /// <summary>
        /// Initializes the ad manager after requesting consent for behavioral targeting ads.
        /// If the player has provided consent already, the manager is initialized immediately.
        /// </summary>
        /// <param name="source">The ad manager.</param>
        /// <param name="optInDialog">The dialog used to request consent from the player.</param>
        /// <param name="onResult">A callback invoked with the result of the request.</param>
        public static void InitializeRequestingConsent(this IAdManager source,
            IBehavioralTargetingOptInDialog optInDialog, Action<bool> onResult)
        {
            if (source.GetBehavioralTargetingConsentStatus()
                == BehavioralTargetingConsentStatus.Unknown)
            {
                Action<bool> defaultCallback = (result) =>
                {
                    source.SetBehavioralTargetingEnabled(result);
                    source.Initialize();
                    onResult(result);
                };

                optInDialog.Show(defaultCallback);
            }
            else
            {
                source.Initialize();

                var status = source.GetBehavioralTargetingConsentStatus() == BehavioralTargetingConsentStatus.Agreed;
                onResult(status);
            }
        }
    }
}
