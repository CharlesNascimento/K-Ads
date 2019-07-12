using ChartboostSDK;
using KansusGames.KansusAds.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KansusGames.KansusAds.Adapter.Chartboost
{
    /// <summary>
    /// Chartboost implementation of an Ad Platform.
    /// </summary>
    public class ChartboostAdPlatform : IAdPlatform
    {
        #region IAdPlatform

        public void Initialize(string appId, bool testMode, List<string> testDevices = null)
        {
            var credentials = appId.Split(';');

            if (credentials.Length != 2)
            {
                throw new InvalidOperationException("Chartboost credentials incorrectly provided. The" +
                    " appId parameter should be a concatenation of the your Chartboost app ID and" +
                    " signature separated by a semicolon: <id>;<signature>");
            }

            Debug.Log("Chartboost App ID: " + credentials[0]);
            Debug.Log("Chartboost App Signature: " + credentials[1]);

            ChartboostSDK.Chartboost.CreateWithAppId(credentials[0], credentials[1]);
        }

        public void SetBehavioralTargetingEnabled(bool enable)
        {
            var consent = enable ? CBPIDataUseConsent.YesBehavioral : CBPIDataUseConsent.NoBehavioral;

            ChartboostSDK.Chartboost.setPIDataUseConsent(consent);
        }

        public IBannerAd CreateBanner(string placementId, BannerPosition adPosition)
        {
            throw new InvalidOperationException("Chartboost does not support banner ads");
        }

        public IInterstitialAd CreateInterstitial(string placementId)
        {
            return new ChartboostInterstitialAd(placementId);
        }

        public IRewardedVideoAd CreateRewardedVideoAd(string placementId)
        {
            return new ChartboostRewardedVideoAd(placementId);
        }

        #endregion
    }
}
