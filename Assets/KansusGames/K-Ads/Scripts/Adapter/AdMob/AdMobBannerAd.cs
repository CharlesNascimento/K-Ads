using GoogleMobileAds.Api;
using KansusGames.KansusAds.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KansusGames.KansusAds.Adapter.AdMob
{
    /// <summary>
    /// Represents a banner advertisement by the AdMob network.
    /// </summary>
    public class AdMobBannerAd : IBannerAd
    {
        #region Fields

        private readonly string placement;
        private BannerView bannerView;
        private readonly BannerPosition adPosition;
        private readonly Func<AdRequest.Builder> adRequestBuilderFactory;

        private static readonly Dictionary<BannerPosition, AdPosition> adPositionMap
            = new Dictionary<BannerPosition, AdPosition>()
        {
            { BannerPosition.Bottom, AdPosition.Bottom},
            { BannerPosition.BottomLeft, AdPosition.BottomLeft},
            { BannerPosition.BottomRight, AdPosition.BottomRight},
            { BannerPosition.Center, AdPosition.Center},
            { BannerPosition.Top, AdPosition.Top},
            { BannerPosition.TopLeft, AdPosition.TopLeft},
            { BannerPosition.TopRight, AdPosition.TopRight},
        };

        #endregion

        #region Initialization

        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        /// <param name="placement">The placement for this interstitial ad.</param>
        /// <param name="adRequestBuilderFactory">A factory to create ad request builders.</param>
        public AdMobBannerAd(string placement, BannerPosition adPosition,
            Func<AdRequest.Builder> adRequestBuilderFactory)
        {
            this.placement = placement;
            this.adPosition = adPosition;
            this.adRequestBuilderFactory = adRequestBuilderFactory;
        }

        #endregion

        #region IInterstitialAd

        public void Show(Action onShow = null, Action<string> onFail = null)
        {
            bannerView = new BannerView(placement, AdSize.SmartBanner, adPositionMap[adPosition]);

            bannerView.OnAdFailedToLoad += (sender, args) =>
            {
                Debug.LogWarning("Failed to load AdMob banner ad: " + args.Message);
                onFail?.Invoke(args.Message);
            };

            EventHandler<EventArgs> loadCallback = null;

            loadCallback = (sender, args) =>
            {
                bannerView.OnAdLoaded -= loadCallback;
                Debug.Log("AdMob banner ad presented successfully");
                onShow?.Invoke();
            };

            bannerView.OnAdLoaded += loadCallback;

            AdRequest request = adRequestBuilderFactory().Build();

            bannerView.LoadAd(request);
        }

        public void Hide()
        {
            if (bannerView == null)
            {
                Debug.LogWarning("Banner view not loaded");
            }

            Debug.Log("AdMob banner ad hidden successfully");

            bannerView.Hide();
            bannerView.Destroy();
        }

        #endregion
    }
}