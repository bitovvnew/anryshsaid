using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using SAIL.Helpers;
using SAIL.Views;

namespace SAIL;

public partial class MainWindow : Window
{
    private HomeView _homeView = null!;
    private ContractView _contractView = null!;
    private GalleryView _galleryView = null!;
    private FunZoneView _funZoneView = null!;
    private MinesweeperView _minesweeperView = null!;

    public MainWindow()
    {
        InitializeComponent();

        _homeView = new HomeView();
        _contractView = new ContractView();
        _galleryView = new GalleryView();
        _funZoneView = new FunZoneView();
        _minesweeperView = new MinesweeperView();

        _homeView.NavigateToContract += (_, _) => NavigateTo(NavContract);
        _homeView.NavigateToGallery += (_, _) => NavigateTo(NavGallery);
        _homeView.NavigateToFun += (_, _) => NavigateTo(NavFun);
        _homeView.NavigateToMines += (_, _) => NavigateTo(NavMines);

        NavHome.Checked += Nav_Checked;
        NavContract.Checked += Nav_Checked;
        NavGallery.Checked += Nav_Checked;
        NavFun.Checked += Nav_Checked;
        NavMines.Checked += Nav_Checked;

        Title = $"{AppInfo.ProjectName} — {AppInfo.LauncherName}";
        VersionBadge.Text = AppInfo.Version;

        TrySetWindowIcon();
        WindowHelper.EnableBlur(this);

        ContentHost.Content = _homeView;
        NavHome.IsChecked = true;

        if (SoundToggle is not null)
            SoundToggle.IsChecked = SoundHelper.Enabled;
        if (RandomSoundToggle is not null)
            RandomSoundToggle.IsChecked = SoundHelper.RandomMode;

        SoundHelper.StartSurpriseTimer(Dispatcher);
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
            // Иконка окна необязательна
        }
    }

    private void NavigateTo(RadioButton navButton) => navButton.IsChecked = true;

    private void Nav_Checked(object sender, RoutedEventArgs e)
    {
        if (ContentHost is null || sender is not RadioButton rb) return;

        ContentHost.Content = rb switch
        {
            _ when rb == NavHome => _homeView,
            _ when rb == NavContract => _contractView,
            _ when rb == NavGallery => _galleryView,
            _ when rb == NavFun => _funZoneView,
            _ when rb == NavMines => _minesweeperView,
            _ => _homeView
        };

        SoundHelper.PlayAction(SoundAction.Navigate);
    }

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        SoundHelper.PlayAction(SoundAction.TitleBarClick);
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

    private void Agreement_Click(object sender, RoutedEventArgs e)
    {
        new UserAgreementWindow(reviewOnly: true) { Owner = this }.ShowDialog();
    }

    private void Launch_Click(object sender, RoutedEventArgs e)
    {
        SoundHelper.PlayAction(SoundAction.LaunchClick);
        var result = MessageBox.Show(
            "Запуск SAIL PROJECT...\n\n" +
            "Версия: bito 0.0.0.1\n" +
            "Цифровая подпись: dolbaeb productions\n\n" +
            "🇷🇺 Русский · iOS 26 vibes\n" +
            "⚠️ 15% с продажи — по договору!\n\n" +
            "Продолжить?",
            "SAIL PROJECT — Launcher",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            SoundHelper.PlayAction(SoundAction.LaunchConfirm);
            MessageBox.Show(
                "🚀 SAIL PROJECT запущен!\n\n" +
                "Подпись: dolbaeb productions",
                "SAIL PROJECT — Готово",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }

    private void SoundToggle_Changed(object sender, RoutedEventArgs e)
    {
        if (SoundToggle is null) return;
        SoundHelper.Enabled = SoundToggle.IsChecked == true;
        if (SoundHelper.Enabled)
        {
            SoundHelper.PlayAction(SoundAction.MemeDismiss);
            SoundHelper.StartSurpriseTimer(Dispatcher);
        }
        else
            SoundHelper.StopSurpriseTimer();
    }

    private void RandomSoundToggle_Changed(object sender, RoutedEventArgs e)
    {
        if (RandomSoundToggle is null) return;
        SoundHelper.RandomMode = RandomSoundToggle.IsChecked == true;
        if (SoundHelper.RandomMode && SoundHelper.Enabled)
            SoundHelper.MaybePlayRandom(SoundCategory.Friendly, 1.0);
    }
}
