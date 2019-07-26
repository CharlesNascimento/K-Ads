using GoogleMobileAds.Api;
using KansusGames.KansusAds.Core;
using System;
using UnityEngine;

namespace KansusGames.KansusAds.Adapter.AdMob
{
    /// <summary>
    /// Represents a rewarded video advertisement by the AdMob network.
    /// </summary>
    public class AdMobRewardedVideoAd : IRewardedVideoAd
    {
        #region Fields

        private readonly string placementId;
        private readonly Func<AdRequest.Builder> adRequestBuilderFactory;
        private RewardedAd rewardedVideoAd;

        #endregion

        #region Initialization

        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        /// <param name="placementId">The placement id for this ad.</param>
        /// <param name="adRequestBuilderFactory">A factory to create ad request builders.</param>
        public AdMobRewardedVideoAd(string placementId, Func<AdRequest.Builder> adRequestBuilderFactory)
        {
            this.placementId = placementId;
            this.adRequestBuilderFactory = adRequestBuilderFactory;
        }

        #endregion

        #region IInterstitialAd

        public bool IsLoaded()
        {
            return rewardedVideoAd != null && rewardedVideoAd.IsLoaded();
        }

        public void Load(Action onLoad = null, Action<string> onFailedToLoad = null)
        {
            rewardedVideoAd = new RewardedAd(placementId);

            rewardedVideoAd.OnAdLoaded += (sender, args) =>
            {
                Debug.Log("AdMob rewarded video ad loaded successfully");
                onLoad?.Invoke();
            };

            rewardedVideoAd.OnAdFailedToLoad += (sender, args) =>
            {
                Debug.LogWarning("Failed to load AdMob rewarded video ad: " + args.Message);
                onFailedToLoad?.Invoke(args.Message);
            };

            AdRequest request = adRequestBuilderFactory().Build();

            rewardedVideoAd.LoadAd(request);
        }

        public void Show(Action onEarnReward = null, Action onSkip = null)
        {
            if (rewardedVideoAd == null || !rewardedVideoAd.IsLoaded())
            {
                Debug.LogWarning("AdMob rewarded video ad not loaded");
                return;
            }

            EventHandler<EventArgs> closeCallback = (sender, args) =>
            {
                Debug.Log("AdMob rewarded video ad skipped");
                onSkip?.Invoke();
            };

            rewardedVideoAd.OnUserEarnedReward += (sender, args) =>
            {
                rewardedVideoAd.OnAdClosed -= closeCallback;
                Debug.Log("AdMob rewarded video ad completed");
                onEarnReward?.Invoke();
            };

            rewardedVideoAd.OnAdClosed += closeCallback;

            rewardedVideoAd.Show();
        }

        #endregion
    }
}