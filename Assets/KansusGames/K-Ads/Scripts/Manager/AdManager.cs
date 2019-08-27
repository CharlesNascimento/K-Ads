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

        private readonly IAdNetwork adPlatform;
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
        public AdManager(IAdNetwork adPlatform, AdManagerSettings settings)
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

        public void Initialize(bool servePersonalizedAds = true, AdNetworkExtras extras = null)
        {
            adPlatform.Initialize(settings.AppId, servePersonalizedAds, extras);

            InitializeInterstitialAds();
            InitializeRewardedVideoAds();
        }

        public void ShowBannerAd(string placementId, Action onShow = null, Action<string> onFail = null)
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

            bannerAd.Show(onShow, onFail);

            bannersMap[placementId] = bannerAd;
        }

        public void HideBannerAd(string placementId)
        {
            placementId = GetPlacementIdOrDefault(placementId, settings.BannerAds);

            var bannerAd = bannersMap[placementId];

            bannerAd.Hide();
        }

        public bool IsInterstitialAdLoaded(string placementId = null)
        {
            placementId = GetPlacementIdOrDefault(placementId, settings.InterstitalAds);

            var interstitialAd = interstitialsMap[placementId];

            return interstitialAd != null && interstitialAd.IsLoaded();
        }

        public void LoadInterstitialAd(Action onLoad = null, Action<string> onFail = null, string placementId = null)
        {
            placementId = GetPlacementIdOrDefault(placementId, settings.InterstitalAds);

            var interstitial = adPlatform.CreateInterstitial(placementId);

            interstitial.Load(onLoad, onFail);

            interstitialsMap[placementId] = interstitial;
        }

        public void ShowInterstitialAd(Action onClose = null, Action<string> onFail = null, string placementId = null)
        {
            placementId = GetPlacementIdOrDefault(placementId, settings.InterstitalAds);

            var interstitialAd = interstitialsMap[placementId];
            var adSettings = settings.InterstitalAds.FirstOrDefault(x => x.PlacementId == placementId);

            if (interstitialAd == null || !interstitialAd.IsLoaded())
            {
                Debug.LogWarning("Interstitial ad not loaded");

                if (adSettings.LoadAutomatically)
                {
                    LoadInterstitialAd(null, null, placementId);
                }

                return;
            }

            var onCloseCallback = onClose;
            var onFailCallback = onFail;
            var lastTimePlayed = lastTimePlayedMap.ContainsKey(placementId) ? lastTimePlayedMap[placementId] : 0;

            if (CurrentTimeInSeconds - lastTimePlayed < adSettings.TimeCap)
            {
                Debug.LogWarning("Interstitial ad will not play due to its time cap");
                return;
            }

            if (adSettings.LoadAutomatically)
            {
                onFailCallback = (reason) =>
                {
                    LoadInterstitialAd(null, null, placementId);
                    onFail?.Invoke(reason);
                };

                onCloseCallback = () =>
                {
                    LoadInterstitialAd(null, null, placementId);
                    onClose?.Invoke();
                };
            };

            interstitialAd.Show(onCloseCallback, onFailCallback);
            lastTimePlayedMap[placementId] = CurrentTimeInSeconds;
        }

        public bool IsRewardedVideoAdLoaded(string placementId = null)
        {
            placementId = GetPlacementIdOrDefault(placementId, settings.RewardedVideoAds);

            var rewardedVideoAd = rewardedVideosMap[placementId];

            return rewardedVideoAd != null && rewardedVideoAd.IsLoaded();
        }

        public void LoadRewardedVideoAd(Action onLoad = null, Action<string> onFail = null, string placementId = null)
        {
            placementId = GetPlacementIdOrDefault(placementId, settings.RewardedVideoAds);

            var rewardedVideo = adPlatform.CreateRewardedVideoAd(placementId);

            rewardedVideo.Load(onLoad, onFail);

            rewardedVideosMap[placementId] = rewardedVideo;
        }

        public void ShowRewardedVideoAd(Action<bool> onResult = null, Action<string> onFail = null,
            string placementId = null)
        {
            placementId = GetPlacementIdOrDefault(placementId, settings.RewardedVideoAds);

            var rewardedVideo = rewardedVideosMap[placementId];
            var adSettings = settings.RewardedVideoAds.FirstOrDefault(x => x.PlacementId == placementId);

            if (rewardedVideo == null || !rewardedVideo.IsLoaded())
            {
                Debug.LogWarning("Rewarded video ad not loaded");

                if (adSettings.LoadAutomatically)
                {
                    LoadRewardedVideoAd(null, null, placementId);
                }

                return;
            }

            var onResultCallback = onResult;
            var onFailCallback = onFail;

            if (adSettings.LoadAutomatically)
            {
                onFailCallback = (reason) =>
                {
                    LoadRewardedVideoAd(null, null, placementId);
                    onFail?.Invoke(reason);
                };

                onResultCallback = (result) =>
                {
                    LoadRewardedVideoAd(null, null, placementId);
                    onResult?.Invoke(result);
                };
            };

            rewardedVideo.Show(onResultCallback, onFailCallback);
        }

        #endregion

        #region Private methods

        private void InitializeInterstitialAds()
        {
            foreach (Ad ad in settings.InterstitalAds)
            {
                if (ad.LoadAutomatically)
                {
                    LoadInterstitialAd(null, null, ad.PlacementId);
                }
            }
        }

        private void InitializeRewardedVideoAds()
        {
            foreach (Ad ad in settings.RewardedVideoAds)
            {
                if (ad.LoadAutomatically)
                {
                    LoadRewardedVideoAd(null, null, ad.PlacementId);
                }
            }
        }

        private string GetPlacementIdOrDefault<TAd>(string placementId, List<TAd> ads) where TAd : Ad
        {
            if (string.IsNullOrEmpty(placementId))
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
