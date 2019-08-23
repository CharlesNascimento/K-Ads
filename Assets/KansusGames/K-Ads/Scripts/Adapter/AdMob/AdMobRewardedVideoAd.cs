using GoogleMobileAds.Api;
using KansusGames.KansusAds.Core;
using System;
using System.Threading;
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

        public void Load(Action onLoad = null, Action<string> onFail = null)
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
                onFail?.Invoke(args.Message);
            };

            AdRequest request = adRequestBuilderFactory().Build();

            rewardedVideoAd.LoadAd(request);
        }

        public void Show(Action<bool> onResult = null, Action<string> onFail = null)
        {
            bool hasEarnedReward = false;

            if (rewardedVideoAd == null || !rewardedVideoAd.IsLoaded())
            {
                Debug.LogWarning("AdMob rewarded video ad not loaded");
                return;
            }

            EventHandler<EventArgs> closeCallback = (sender, args) =>
            {
                // Workaround for when the close callback is wrongly called when the user earns the reward
                Thread.Sleep(50);

                if (!hasEarnedReward)
                {
                    Debug.Log("AdMob rewarded video ad skipped");

                    onResult?.Invoke(false);
                }
            };

            rewardedVideoAd.OnUserEarnedReward += (sender, args) =>
            {
                Debug.Log("AdMob rewarded video ad completed");

                rewardedVideoAd.OnAdClosed -= closeCallback;
                hasEarnedReward = true;
                onResult?.Invoke(true);
            };

            rewardedVideoAd.OnAdFailedToShow += (sender, args) =>
            {
                Debug.Log("Failed to show AdMob rewarded video ad");

                onFail?.Invoke(args.Message);
            };

            rewardedVideoAd.OnAdClosed += closeCallback;

            rewardedVideoAd.Show();
        }

        #endregion
    }
}