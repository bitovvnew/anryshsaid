using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using SAIL.Helpers;

namespace SAIL.Views;

public partial class RejectContractGameWindow : Window
{
    private const int TargetScore = 12;
    private const int TimeLimit = 20;

    private int _score;
    private int _timeLeft = TimeLimit;
    private DispatcherTimer? _spawnTimer;
    private DispatcherTimer? _clockTimer;
    private readonly Random _rng = new();
    public bool EscapeSuccessful { get; private set; }

    public RejectContractGameWindow()
    {
        InitializeComponent();
        Loaded += (_, _) => StartGame();
        Closed += (_, _) => StopTimers();
    }

    private void StartGame()
    {
        _score = 0;
        _timeLeft = TimeLimit;
        UpdateHud();
        GameCanvas.Children.Clear();

        _clockTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _clockTimer.Tick += (_, _) =>
        {
            _timeLeft--;
            UpdateHud();
            if (_timeLeft <= 0)
                EndGame(won: false);
        };
        _clockTimer.Start();

        _spawnTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(850) };
        _spawnTimer.Tick += (_, _) => SpawnTarget();
        _spawnTimer.Start();
        SpawnTarget();
    }

    private void SpawnTarget()
    {
        if (_timeLeft <= 0) return;

        var isGood = _rng.NextDouble() > 0.38;
        var btn = new Button
        {
            Width = 96,
            Height = 44,
            Content = isGood ? "✨ СВОБОДА" : "💸 15%",
            FontFamily = Application.Current.Resources["AppFont"] as System.Windows.Media.FontFamily,
            FontSize = 11,
            FontWeight = FontWeights.SemiBold,
            Cursor = System.Windows.Input.Cursors.Hand,
            BorderThickness = new Thickness(0),
            Background = isGood
                ? new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0x30, 0xD1, 0x58))
                : new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0xFF, 0x37, 0x5F)),
            Foreground = System.Windows.Media.Brushes.White,
            Tag = isGood
        };

        var maxX = Math.Max(0, GameCanvas.ActualWidth - btn.Width);
        var maxY = Math.Max(0, GameCanvas.ActualHeight - btn.Height);
        Canvas.SetLeft(btn, maxX > 0 ? _rng.Next(0, (int)maxX) : 0);
        Canvas.SetTop(btn, maxY > 0 ? _rng.Next(40, (int)maxY) : 40);

        btn.Click += (_, _) =>
        {
            if (btn.Tag is true)
            {
                _score++;
                UpdateHud();
                GameCanvas.Children.Remove(btn);
                if (_score >= TargetScore)
                    EndGame(won: true);
            }
            else
            {
                _score = Math.Max(0, _score - 2);
                UpdateHud();
                SoundHelper.PlayAction(SoundAction.GameWrongClick);
                GameCanvas.Children.Remove(btn);
                if (Owner is MainWindow mw)
                    mw.ShowMemeOverlay("ЭТО НЕ ВЫХОД", "Ты нажал 15%! Минус 2 очка. Договор смеётся. 🤡");
            }
        };

        GameCanvas.Children.Add(btn);

        var removeTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2.2) };
        removeTimer.Tick += (_, _) =>
        {
            removeTimer.Stop();
            GameCanvas.Children.Remove(btn);
        };
        removeTimer.Start();
    }

    private void UpdateHud()
    {
        ScoreText.Text = $"✨ {_score} / {TargetScore}";
        TimeText.Text = $"⏱ {_timeLeft}";
    }

    private void EndGame(bool won)
    {
        StopTimers();
        EscapeSuccessful = won;

        if (won)
        {
            SoundHelper.PlayAction(SoundAction.GameWin);
            MessageBox.Show(
                "Ты сбежал от 15%!\n\n(На 20 секунд. Договор всё равно ждёт.)",
                "SAIL PROJECT — Победа",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            DialogResult = true;
        }
        else
        {
            SoundHelper.PlayAction(SoundAction.GameLose);
            if (Owner is MainWindow mw)
                mw.ShowMemeOverlay("ЭТО НЕ ВЫХОД",
                    "Не набрал 12 «СВОБОДА».\n15% — навсегда. Договор не отклоняется. 🔒");
            else
                MessageBox.Show("ЭТО НЕ ВЫХОД\n\n15% — навсегда. 🔒", "SAIL PROJECT",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            DialogResult = false;
        }

        Close();
    }

    private void GiveUp_Click(object sender, RoutedEventArgs e)
    {
        StopTimers();
        EscapeSuccessful = false;
        SoundHelper.PlayAction(SoundAction.ContractReject);
        if (Owner is MainWindow mw)
            MemeOverlayHelper.Show(mw);
        else
            MessageBox.Show("ЭТО НЕ ВЫХОД", "SAIL PROJECT", MessageBoxButton.OK);
        DialogResult = false;
        Close();
    }

    private void StopTimers()
    {
        _spawnTimer?.Stop();
        _clockTimer?.Stop();
    }
}
