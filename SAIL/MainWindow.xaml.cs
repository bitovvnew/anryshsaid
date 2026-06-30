using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using SAIL.Helpers;
using SAIL.Views;

namespace SAIL;

public partial class MainWindow : Window
{
    private readonly HomeView _homeView;
    private readonly ContractView _contractView;
    private readonly GalleryView _galleryView;

    public MainWindow()
    {
        InitializeComponent();

        Title = $"{AppInfo.ProjectName} — {AppInfo.LauncherName}";
        VersionBadge.Text = AppInfo.Version;

        TrySetWindowIcon();

        WindowHelper.EnableBlur(this);

        _homeView = new HomeView();
        _contractView = new ContractView();
        _galleryView = new GalleryView();

        _homeView.NavigateToContract += (_, _) => NavigateTo(NavContract);
        _homeView.NavigateToGallery += (_, _) => NavigateTo(NavGallery);

        ContentHost.Content = _homeView;
    }

    private void TrySetWindowIcon()
    {
        try
        {
            var uri = new Uri("pack://application:,,,/Assets/app-icon.png", UriKind.Absolute);
            Icon = BitmapFrame.Create(uri);
        }
        catch
        {
            // Иконка окна необязательна — .exe-иконка остаётся из ApplicationIcon
        }
    }

    private void NavigateTo(RadioButton navButton)
    {
        navButton.IsChecked = true;
        Nav_Checked(navButton, new RoutedEventArgs());
    }

    private void Nav_Checked(object sender, RoutedEventArgs e)
    {
        ContentHost.Content = sender switch
        {
            RadioButton rb when rb == NavHome => _homeView,
            RadioButton rb when rb == NavContract => _contractView,
            RadioButton rb when rb == NavGallery => _galleryView,
            _ => _homeView
        };
    }

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2)
        {
            Maximize_Click(sender, e);
            return;
        }
        DragMove();
    }

    private void Minimize_Click(object sender, RoutedEventArgs e) =>
        WindowState = WindowState.Minimized;

    private void Maximize_Click(object sender, RoutedEventArgs e) =>
        WindowState = WindowState == WindowState.Maximized
            ? WindowState.Normal
            : WindowState.Maximized;

    private void Close_Click(object sender, RoutedEventArgs e) => Close();

    private void Launch_Click(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show(
            "Запуск SAIL PROJECT...\n\n" +
            "Версия: bito 0.0.0.1\n" +
            "Цифровая подпись: dolbaeb productions\n\n" +
            "🇷🇺 Русский интерфейс\n" +
            "⚠️ Напоминание: 15% с продажи — по договору!\n\n" +
            "Продолжить?",
            "SAIL PROJECT — Launcher",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            MessageBox.Show(
                "🚀 SAIL PROJECT запущен!\n\n" +
                "Версия: bito 0.0.0.1\n" +
                "Подпись: dolbaeb productions\n\n" +
                "(Это демо-лаунчер. Подключи свои инструменты продажи\n" +
                "и не забудь подписать договор!)",
                "SAIL PROJECT — Готово",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
