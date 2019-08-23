using KansusGames.KansusAds.Core;
using KansusGames.KansusAds.Manager;
using UnityEngine;

namespace KansusGames.KansusAds.Demo
{
    public abstract class DemoBase<TAdPlatform> : MonoBehaviour where TAdPlatform : IAdNetwork, new()
    {
        [SerializeField]
        protected AdManagerSettings adManagerSettings;

        [SerializeField]
        protected bool behavioralTargeting = true;

        [SerializeField]
        private string bannerPlacementId;

        [SerializeField]
        private string interstitialPlacementId;

        [SerializeField]
        private string rewardedVideoPlacementId;

        protected IAdManager adManager;

        private bool isShowingBanner = false;

        protected virtual void Initialize()
        {
            IAdNetwork adPlatform = new TAdPlatform();

            adManager = new AdManager(adPlatform, adManagerSettings);

            adManager.Initialize(behavioralTargeting, CreateExtras());
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
            //adManager.ShowInterstitialAd(interstitialPlacementId);
            // It also works without providing the placementId parameter
            // In this case, the first ad in the list will be used (From the manager settings)
            adManager.ShowInterstitialAd();
        }

        public void ShowRewardedVideoAd()
        {
            adManager.ShowRewardedVideoAd(null, null, rewardedVideoPlacementId);
        }

        protected virtual AdNetworkExtras CreateExtras()
        {
            return null;
        }

        private void Start()
        {
            Initialize();
        }
    }
}