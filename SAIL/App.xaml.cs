using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;

namespace SAIL;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        AppDomain.CurrentDomain.UnhandledException += (_, args) =>
        {
            if (args.ExceptionObject is Exception ex)
                LogError(ex);
        };

        DispatcherUnhandledException += (_, args) =>
        {
            LogError(args.Exception);
            MessageBox.Show(
                $"Произошла ошибка:\n\n{args.Exception.Message}\n\nПодробности: {GetLogPath()}",
                "SAIL PROJECT — Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            args.Handled = true;
        };

        try
        {
            var culture = new CultureInfo(AppInfo.Locale);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            var mainWindow = new MainWindow();
            MainWindow = mainWindow;
            mainWindow.Show();
        }
        catch (Exception ex)
        {
            LogError(ex);
            MessageBox.Show(
                $"SAIL PROJECT не смог запуститься.\n\n{ex.Message}\n\nПодробности: {GetLogPath()}",
                "SAIL PROJECT — Ошибка",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            Shutdown(1);
        }
    }

    private static string GetLogPath()
    {
        var dir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "SAIL PROJECT");
        Directory.CreateDirectory(dir);
        return Path.Combine(dir, "sail-error.log");
    }

    private static void LogError(Exception ex)
    {
        try
        {
            var log = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]\r\n{ex}\r\n\r\n";
            File.AppendAllText(GetLogPath(), log);
        }
        catch
        {
            // ignore logging failures
        }
    }
}
