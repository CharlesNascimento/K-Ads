using KansusGames.KansusAds.Adapter.AdMob;
using KansusGames.KansusAds.Manager;
using UnityEngine;

namespace KansusGames.KansusAds.Demo.BehavioralTargeting
{
    public class AdMobBehavioralTargetingDemo : AdMobDemo
    {
        private const string ConsentStatusKey = "BehavioralTargetingConsentStatus";

        [SerializeField]
        private BehavioralTargetingOptInDialog optInDialog;

        protected override void Initialize()
        {
            AdMobAdNetwork adPlatform = new AdMobAdNetwork();
            adManager = new AdManager(adPlatform, adManagerSettings);

            var consentStatus = GetBehavioralTargetingConsentStatus();

            Debug.Log("BehavioralTargetingConsentStatus: " + consentStatus);

            if (consentStatus == BehavioralTargetingConsentStatus.Unknown)
            {
                optInDialog.Show((consent) =>
                {
                    SaveBehavioralTargetingConsent(consent);
                    StartGame(consent);
                });
            }
            else
            {
                StartGame(consentStatus == BehavioralTargetingConsentStatus.Agreed);
            }
        }

        private void StartGame(bool collectUserData)
        {
            // Here you can setup your mediation networks according to the user consent.
            // UnityAds.SetGDPRConsentMetaData(collectUserData);
            // Chartboost.RestrictDataCollection(!collectUserData);

            adManager.Initialize(collectUserData, CreateExtras());

            // mainMenu.Show();
        }

        private void SaveBehavioralTargetingConsent(bool consent)
        {
            var status = consent ?
                (int)BehavioralTargetingConsentStatus.Agreed :
                (int)BehavioralTargetingConsentStatus.Declined;

            PlayerPrefs.SetInt(ConsentStatusKey, status);
        }

        private BehavioralTargetingConsentStatus GetBehavioralTargetingConsentStatus()
        {
            var status = PlayerPrefs.GetInt(ConsentStatusKey, 0);

            return (BehavioralTargetingConsentStatus)status;
        }
    }
}
