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
        #region Constants

        private const string ConsentStatusKey = "BehavioralTargetingConsentStatus";

        #endregion

        #region Fields

        private readonly IAdPlatform adPlatform;
        private readonly AdManagerSettings settings;

        private readonly Dictionary<string, IBannerAd> bannersMap;
        private readonly Dictionary<string, IInterstitialAd> interstitialsMap;
        private readonly Dictionary<string, IRewardedVideoAd> rewardedVideosMap;

        private readonly Dictionary<string, long> lastTimePlayedMap;

        #endregion

        #region Properties

        private long CurrentTimeInSeconds
        {
            get => DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
        }

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

            lastTimePlayedMap = new Dictionary<string, long>();
        }

        #endregion

        #region IAdManager

        public void Initialize()
        {
            adPlatform.Initialize(settings.AppId, settings.TestMode, settings.TestDevices);

            InitializeInterstitialAds();
            InitializeRewardedVideoAds();
        }

        public void SetBehavioralTargetingEnabled(bool enable)
        {
            adPlatform.SetBehavioralTargetingEnabled(enable);

            var status = enable ?
                (int)BehavioralTargetingConsentStatus.Agreed :
                (int)BehavioralTargetingConsentStatus.Declined;

            PlayerPrefs.SetInt(ConsentStatusKey, status);
        }

        public BehavioralTargetingConsentStatus GetBehavioralTargetingConsentStatus()
        {
            var status = PlayerPrefs.GetInt(ConsentStatusKey, 0);

            return (BehavioralTargetingConsentStatus)status;
        }

        public void ShowBannerAd(string placementId, Action onShow = null, Action<string> onFailedToLoad = null)
        {
            placementId = GetPlacementIdOrDefault(placementId, settings.BannerAds);

            var adSettings = settings.BannerAds.FirstOrDefault(x => x.PlacementId == placementId);

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
            placementId = GetPlacementIdOrDefault(placementId, settings.BannerAds);

            var bannerAd = bannersMap[placementId];

            bannerAd.Hide();
        }

        public void LoadInterstitialAd(string placementId = null, Action onLoad = null, Action<string> onFailedToLoad = null)
        {
            placementId = GetPlacementIdOrDefault(placementId, settings.InterstitalAds);

            var interstitial = adPlatform.CreateInterstitial(placementId);

            interstitial.Load(onLoad, onFailedToLoad);

            interstitialsMap[placementId] = interstitial;
        }

        public void ShowInterstitialAd(string placementId = null, Action onOpening = null, Action onClose = null)
        {
            placementId = GetPlacementIdOrDefault(placementId, settings.InterstitalAds);

            var interstitialAd = interstitialsMap[placementId];

            if (interstitialAd == null || !interstitialAd.IsLoaded())
            {
                Debug.LogWarning("Interstitial ad not loaded");
                return;
            }

            var onCloseCallback = onClose;
            var adSettings = settings.InterstitalAds.FirstOrDefault(x => x.PlacementId == placementId);
            var lastTimePlayed = lastTimePlayedMap.ContainsKey(placementId) ? lastTimePlayedMap[placementId] : 0;

            if (CurrentTimeInSeconds - lastTimePlayed < adSettings.TimeCap)
            {
                Debug.LogWarning("Interstitial ad will not play due to its time cap");
                return;
            }

            if (adSettings.LoadAutomatically)
            {
                onCloseCallback = () =>
                {
                    LoadInterstitialAd(placementId);
                    onClose?.Invoke();
                };
            };

            interstitialAd.Show(onOpening, onCloseCallback);
            lastTimePlayedMap[placementId] = CurrentTimeInSeconds;
        }

        public void LoadRewardedVideoAd(string placementId, Action onLoad = null, Action<string> onFailedToLoad = null)
        {
            placementId = GetPlacementIdOrDefault(placementId, settings.RewardedVideoAds);

            var rewardedVideo = adPlatform.CreateRewardedVideoAd(placementId);

            rewardedVideo.Load(onLoad, onFailedToLoad);

            rewardedVideosMap[placementId] = rewardedVideo;
        }

        public void ShowRewardedVideoAd(string placementId, Action onEarnReward = null, Action onSkip = null)
        {
            placementId = GetPlacementIdOrDefault(placementId, settings.RewardedVideoAds);

            var rewardedVideo = rewardedVideosMap[placementId];

            if (rewardedVideo == null || !rewardedVideo.IsLoaded())
            {
                Debug.LogWarning("Rewarded video ad not loaded");
                return;
            }

            Action onSkipCallback = onSkip;
            Action onEarnRewardCallback = onEarnReward;

            var adSettings = settings.RewardedVideoAds.FirstOrDefault(x => x.PlacementId == placementId);

            if (adSettings.LoadAutomatically)
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

        private string GetPlacementIdOrDefault<TAd>(string placementId, List<TAd> ads) where TAd : Ad
        {
            if (placementId == null)
            {
                if (ads.Count == 0)
                {
                    throw new InvalidOperationException("No placement ID provided, but there is no" +
                        " first ad in the list, since it is empty");
                }

                placementId = ads[0].PlacementId;
            }

            return placementId;
        }

        #endregion
    }
}
