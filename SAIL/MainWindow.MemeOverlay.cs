using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using SAIL.Helpers;

namespace SAIL;

public partial class MainWindow
{
    private DispatcherTimer? _memeAutoHideTimer;

    public void ShowMemeOverlay(string title, string text)
    {
        SoundHelper.PlayAction(SoundAction.MemeOverlay);

        MemeOverlayTitle.Text = title;
        MemeOverlayText.Text = text;
        MemeOverlay.Visibility = Visibility.Visible;
        MemeOverlay.Opacity = 0;

        var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(280))
        {
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };
        MemeOverlay.BeginAnimation(OpacityProperty, fadeIn);

        _memeAutoHideTimer?.Stop();
        _memeAutoHideTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
        _memeAutoHideTimer.Tick += (_, _) =>
        {
            _memeAutoHideTimer.Stop();
            HideMemeOverlay();
        };
        _memeAutoHideTimer.Start();
    }

    public void HideMemeOverlay()
    {
        _memeAutoHideTimer?.Stop();

        var fadeOut = new DoubleAnimation(MemeOverlay.Opacity, 0, TimeSpan.FromMilliseconds(220));
        fadeOut.Completed += (_, _) => MemeOverlay.Visibility = Visibility.Collapsed;
        MemeOverlay.BeginAnimation(OpacityProperty, fadeOut);
    }

    private void DismissMeme_Click(object sender, RoutedEventArgs e)
    {
        SoundHelper.PlayAction(SoundAction.MemeDismiss);
        HideMemeOverlay();
    }
}
