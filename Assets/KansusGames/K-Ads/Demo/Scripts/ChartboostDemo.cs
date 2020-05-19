using KansusGames.KansusAds.Adapter.Chartboost;

namespace KansusGames.KansusAds.Demo
{
    /// <summary>
    /// Due to a conflict of dependencies between Chartboost and Admob, the
    /// Chartboost SDK will crash on Android the second time you run the app.
    /// To avoid that problem, clear the app storage.
    /// </summary>
    public class ChartboostDemo : DemoBase<ChartboostAdNetwork>
    {

    }
}
