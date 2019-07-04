using KansusGames.KansusAds.Core;
using KansusGames.KansusAds.Manager;
using UnityEngine;

namespace KansusGames.KansusAds.Demo
{
    public abstract class DemoBase<TAdPlatform> : MonoBehaviour where TAdPlatform : IAdPlatform, new()
    {
        [SerializeField]
        private AdManagerSettings adManagerSettings;

        [SerializeField]
        private string bannerPlacementId;

        [SerializeField]
        private string interstitialPlacementId;

        [SerializeField]
        private string rewardedVideoPlacementId;

        protected IAdManager adManager;

        private bool isShowingBanner = false;

        protected void Initialize()
        {
            IAdPlatform adPlatform = new TAdPlatform();

            adManager = new AdManager(adPlatform, adManagerSettings);

            adManager.Initialize();
        }

        public void ShowBannerAd()
        {
            if (isShowingBanner)
            {
                adManager.HideBannerAd(bannerPlacementId);
                isShowingBanner = false;
            }
            else
            {
                adManager.ShowBannerAd(bannerPlacementId, () => isShowingBanner = true);
            }
        }

        public void ShowInterstitialAd()
        {
            adManager.ShowInterstitialAd(interstitialPlacementId);
        }

        public void ShowRewardedVideoAd()
        {
            adManager.ShowRewardedVideoAd(rewardedVideoPlacementId);
        }

        private void Start()
        {
            Initialize();
        }
    }
}