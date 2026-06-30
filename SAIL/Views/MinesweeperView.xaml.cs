using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using SAIL.Helpers;

namespace SAIL.Views;

public partial class MinesweeperView : UserControl
{
    private const int Size = 10;
    private int _mineCount = 15;
    private int _revealedSafe;
    private int _totalSafe;
    private bool _gameOver;
    private bool _firstClick = true;
    private DateTime _startTime;
    private DispatcherTimer? _timer;
    private readonly bool[,] _mines = new bool[Size, Size];
    private readonly bool[,] _revealed = new bool[Size, Size];
    private readonly bool[,] _flagged = new bool[Size, Size];
    private readonly Button[,] _cells = new Button[Size, Size];

    public MinesweeperView()
    {
        InitializeComponent();
        Loaded += (_, _) => NewGame();
    }

    private void NewGame()
    {
        _gameOver = false;
        _firstClick = true;
        _revealedSafe = 0;
        _totalSafe = Size * Size - _mineCount;
        Array.Clear(_mines);
        Array.Clear(_revealed);
        Array.Clear(_flagged);
        BoardGrid.Children.Clear();
        StopTimer();
        TimerText.Text = "⏱ 0:00";
        MineCounter.Text = $"💣 {_mineCount}";
        StatusText.Text = "ЛКМ — открыть · ПКМ — флаг · Не наступи на 15%!";

        for (var r = 0; r < Size; r++)
        for (var c = 0; c < Size; c++)
        {
            var btn = CreateCell(r, c);
            _cells[r, c] = btn;
            BoardGrid.Children.Add(btn);
        }
    }

    private Button CreateCell(int row, int col)
    {
        var btn = new Button
        {
            Tag = (row, col),
            Margin = new Thickness(1),
            FontSize = 13,
            FontWeight = FontWeights.Bold,
            Cursor = Cursors.Hand,
            Background = new SolidColorBrush(Color.FromRgb(0x2A, 0x2A, 0x38)),
            Foreground = Brushes.White,
            BorderThickness = new Thickness(0),
            Content = ""
        };
        btn.Click += Cell_Click;
        btn.MouseRightButtonDown += Cell_Flag;
        return btn;
    }

    private void PlaceMines(int safeRow, int safeCol)
    {
        var placed = 0;
        while (placed < _mineCount)
        {
            var r = Random.Shared.Next(Size);
            var c = Random.Shared.Next(Size);
            if (_mines[r, c] || (r == safeRow && c == safeCol)) continue;
            _mines[r, c] = true;
            placed++;
        }
    }

    private void Cell_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.Tag is not (int row, int col)) return;
        if (_gameOver || _flagged[row, col]) return;

        if (_firstClick)
        {
            _firstClick = false;
            PlaceMines(row, col);
            StartTimer();
        }

        Reveal(row, col);
    }

    private void Cell_Flag(object sender, MouseButtonEventArgs e)
    {
        e.Handled = true;
        if (sender is not Button btn || btn.Tag is not (int row, int col)) return;
        if (_gameOver || _revealed[row, col]) return;

        _flagged[row, col] = !_flagged[row, col];
        btn.Content = _flagged[row, col] ? "🚩" : "";
    }

    private void Reveal(int row, int col)
    {
        if (row < 0 || col < 0 || row >= Size || col >= Size) return;
        if (_revealed[row, col] || _flagged[row, col]) return;

        _revealed[row, col] = true;

        if (_mines[row, col])
        {
            Explode(row, col);
            return;
        }

        _revealedSafe++;
        var count = CountAdjacentMines(row, col);
        UpdateCellVisual(row, col, count);

        if (count == 0)
        {
            for (var dr = -1; dr <= 1; dr++)
            for (var dc = -1; dc <= 1; dc++)
                if (dr != 0 || dc != 0)
                    Reveal(row + dr, col + dc);
        }

        if (_revealedSafe >= _totalSafe)
            WinGame();
    }

    private void Explode(int row, int col)
    {
        _gameOver = true;
        StopTimer();

        for (var r = 0; r < Size; r++)
        for (var c = 0; c < Size; c++)
            if (_mines[r, c])
            {
                _cells[r, c].Content = r == row && c == col ? "💥" : "💣";
                _cells[r, c].Background = new SolidColorBrush(Color.FromRgb(0xFF, 0x37, 0x5F));
            }

        StatusText.Text = "БУМ! Это не выход...";

        var window = Window.GetWindow(this);
        if (window is not null)
        {
            SoundHelper.PlayAction(SoundAction.MineExplode);
            MemeOverlayHelper.Show(window, "ЭТО НЕ ВЫХОД",
                "Ты наступил на 15% в форме мины. 💣\nСапёр не прощает. Договор тоже.");
        }
    }

    private void WinGame()
    {
        _gameOver = true;
        StopTimer();
        StatusText.Text = "🏆 Победа! Ты избежал 15%... пока что.";
        SoundHelper.PlayAction(SoundAction.MineWin);
        MessageBox.Show(
            "Ты прошёл сапёра!\n\nНо 15% с продажи Roblox всё ещё по договору 😈",
            "SAIL PROJECT — Победа",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    private int CountAdjacentMines(int row, int col)
    {
        var count = 0;
        for (var dr = -1; dr <= 1; dr++)
        for (var dc = -1; dc <= 1; dc++)
        {
            if (dr == 0 && dc == 0) continue;
            var nr = row + dr;
            var nc = col + dc;
            if (nr >= 0 && nc >= 0 && nr < Size && nc < Size && _mines[nr, nc])
                count++;
        }
        return count;
    }

    private void UpdateCellVisual(int row, int col, int count)
    {
        var btn = _cells[row, col];
        btn.IsEnabled = false;
        btn.Background = new SolidColorBrush(Color.FromRgb(0x1E, 0x1E, 0x28));
        btn.Content = count == 0 ? "" : count.ToString();
        btn.Foreground = count switch
        {
            1 => new SolidColorBrush(Color.FromRgb(0x00, 0x78, 0xD4)),
            2 => new SolidColorBrush(Color.FromRgb(0x30, 0xD1, 0x58)),
            3 => new SolidColorBrush(Color.FromRgb(0xFF, 0x37, 0x5F)),
            _ => Brushes.White
        };
    }

    private void StartTimer()
    {
        _startTime = DateTime.Now;
        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _timer.Tick += (_, _) =>
            TimerText.Text = $"⏱ {(DateTime.Now - _startTime):m\\:ss}";
        _timer.Start();
    }

    private void StopTimer()
    {
        _timer?.Stop();
        _timer = null;
    }

    private void Restart_Click(object sender, RoutedEventArgs e) => NewGame();

    private void EasyMode_Click(object sender, RoutedEventArgs e)
    {
        _mineCount = 10;
        NewGame();
    }
}
