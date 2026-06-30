using System.Globalization;
using System.Windows;
using System.Threading;

namespace SAIL;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        var culture = new CultureInfo(AppInfo.Locale);
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;

        base.OnStartup(e);
    }
}
