using System;
using UnityEngine;

namespace KansusGames.KansusAds.Manager
{
    /// <summary>
    /// Default implementation of a behavioral targeting opt-in dialog.
    /// </summary>
    public class BehavioralTargetingOptInDialog : MonoBehaviour, IBehavioralTargetingOptInDialog
    {
        #region Fields

        [Tooltip("The URL of the Privacy Policy of your game")]
        [SerializeField]
        private string privacyPolicyURL;

        private Action<bool> onResult;

        #endregion

        #region IBehavioralTargetingOptInDialog

        public virtual void Show(Action<bool> onResult)
        {
            this.onResult = onResult;

            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        #endregion

        #region Button callbacks

        public virtual void OnAgreeButtonPressed()
        {
            Hide();

            onResult.Invoke(true);
        }

        public virtual void OnDeclineButtonPressed()
        {
            Hide();

            onResult.Invoke(false);
        }

        public virtual void OnPrivacyPolicyButtonPressed()
        {
            Application.OpenURL(privacyPolicyURL);
        }

        #endregion
    }
}
