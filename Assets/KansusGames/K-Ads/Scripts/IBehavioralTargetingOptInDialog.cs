using System;

namespace KansusGames.KansusAds.Manager
{
    /// <summary>
    /// Specifies a dialog which asks the player whether he consents with
    /// behavioral targeting ads.
    /// </summary>
    public interface IBehavioralTargetingOptInDialog
    {
        /// <summary>
        /// Presents the opt-in dialog.
        /// </summary>
        /// <param name="onResult">A callback invoked when the player agrees
        /// or declines the opt-in.</param>
        void Show(Action<bool> onResult);

        /// <summary>
        /// Hides the opt-in dialog.
        /// </summary>
        void Hide();
    }
}
