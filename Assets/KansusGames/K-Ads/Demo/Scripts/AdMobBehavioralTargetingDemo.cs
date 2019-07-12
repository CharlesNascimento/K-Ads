using KansusGames.KansusAds.Adapter.AdMob;
using KansusGames.KansusAds.Manager;
using UnityEngine;

namespace KansusGames.KansusAds.Demo
{
    public class AdMobBehavioralTargetingDemo : DemoBase<AdMobAdPlatform>
    {
        [SerializeField]
        private BehavioralTargetingOptInDialog optInDialog;

        protected override void Initialize()
        {
            AdMobAdPlatform adPlatform = new AdMobAdPlatform();

            adManager = new AdManager(adPlatform, adManagerSettings);

            Debug.Log("BehavioralTargetingConsentStatus: " + adManager.GetBehavioralTargetingConsentStatus());

            adManager.InitializeRequestingConsent(optInDialog, OnBehavioralTargetingDialogResult);
        }

        private void OnBehavioralTargetingDialogResult(bool result)
        {
            Debug.Log("Received consent result: " + result);
        }
    }
}
