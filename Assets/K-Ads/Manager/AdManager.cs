using KansusGames.KansusAds.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KansusGames.KansusAds.Manager
{
    /// <summary>
    /// A manager which deals with advertisements.
    /// </summary>
    public class AdManager : IAdManager
    {
        #region Fields

        private readonly IAdPlatform adPlatform;
        private readonly AdManagerSettings settings;

        private readonly Dictionary<string, IBannerAd> bannersMap;
        private readonly Dictionary<string, IInterstitialAd> interstitialsMap;
        private readonly Dictionary<string, IRewardedVideoAd> rewardedVideosMap;

        #endregion

        #region Initialization

        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        /// <param name="adPlatform">The advertisement network used by the manager.</param>
        /// <param name="settings">The manager settings.</param>
        public AdManager(IAdPlatform adPlatform, AdManagerSettings settings)
        {
            this.adPlatform = adPlatform;
            this.settings = settings;

            bannersMap = new Dictionary<string, IBannerAd>();
            interstitialsMap = new Dictionary<string, IInterstitialAd>();
            rewardedVideosMap = new Dictionary<string, IRewardedVideoAd>();
        }

        #endregion

        #region IAdManager

        public void Initialize()
        {
            adPlatform.Initialize(settings.AppId, settings.TestMode, settings.TestDevices);

            InitializeInterstitialAds();
            InitializeRewardedVideoAds();
        }

        public void ShowBannerAd(string placementId, Action onShow = null, Action<string> onFailedToLoad = null)
        {
            var adSettings = settings.BannerAds
                .Where(x => x.PlacementId == placementId)
                .FirstOrDefault();

            if (adSettings == null)
            {
                Debug.LogWarning("Banner ad not loaded");
                return;
            }

            var bannerAd = adPlatform.CreateBanner(placementId, adSettings.AdPosition);

            if (bannerAd == null)
            {
                Debug.LogWarning("Banner ad not loaded or not supported by the current platform");
                return;
            }

            bannerAd.Show(onShow, onFailedToLoad);

            bannersMap[placementId] = bannerAd;
        }

        public void HideBannerAd(string placementId)
        {
            var bannerAd = bannersMap[placementId];

            bannerAd.Hide();
        }

        public void LoadInterstitialAd(string placementId, Action onLoad = null, Action<string> onFailedToLoad = null)
        {
            var interstitial = adPlatform.CreateInterstitial(placementId);

            interstitial.Load(onLoad, onFailedToLoad);

            interstitialsMap[placementId] = interstitial;
        }

        public void ShowInterstitialAd(string placementId, Action onOpening = null, Action onClose = null)
        {
            var interstitialAd = interstitialsMap[placementId];

            if (interstitialAd == null || !interstitialAd.IsLoaded())
            {
                Debug.LogWarning("Interstitial ad not loaded");
                return;
            }

            Action onCloseCallback = onClose;

            var adSetting = settings.InterstitalAds
                .Where(x => x.PlacementId == placementId)
                .FirstOrDefault();

            if (adSetting.LoadAutomatically)
            {
                onCloseCallback = () =>
                {
                    LoadInterstitialAd(placementId);
                    onClose?.Invoke();
                };
            };

            interstitialAd.Show(onOpening, onCloseCallback);
        }

        public void LoadRewardedVideoAd(string placementId, Action onLoad = null, Action<string> onFailedToLoad = null)
        {
            var rewardedVideo = adPlatform.CreateRewardedVideoAd(placementId);

            rewardedVideo.Load(onLoad, onFailedToLoad);

            rewardedVideosMap[placementId] = rewardedVideo;
        }

        public void ShowRewardedVideoAd(string placementId, Action onEarnReward = null, Action onSkip = null)
        {
            var rewardedVideo = rewardedVideosMap[placementId];

            if (rewardedVideo == null || !rewardedVideo.IsLoaded())
            {
                Debug.LogWarning("Rewarded video ad not loaded");
                return;
            }

            Action onSkipCallback = onSkip;
            Action onEarnRewardCallback = onEarnReward;

            var adSetting = settings.RewardedVideoAds
                .Where(x => x.PlacementId == placementId)
                .FirstOrDefault();

            if (adSetting.LoadAutomatically)
            {
                onSkipCallback = () =>
                {
                    LoadRewardedVideoAd(placementId);
                    onSkip?.Invoke();
                };

                onEarnRewardCallback = () =>
                {
                    LoadRewardedVideoAd(placementId);
                    onEarnReward?.Invoke();
                };
            };

            rewardedVideo.Show(onEarnRewardCallback, onSkipCallback);
        }

        #endregion

        #region Private methods

        private void InitializeInterstitialAds()
        {
            foreach (Ad ad in settings.InterstitalAds)
            {
                if (ad.LoadAutomatically)
                {
                    LoadInterstitialAd(ad.PlacementId);
                }
            }
        }

        private void InitializeRewardedVideoAds()
        {
            foreach (Ad ad in settings.RewardedVideoAds)
            {
                if (ad.LoadAutomatically)
                {
                    LoadRewardedVideoAd(ad.PlacementId);
                }
            }
        }

        #endregion
    }
}
