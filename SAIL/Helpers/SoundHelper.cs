using System.IO;
using System.Text.Json;
using System.Windows.Media;
using System.Windows.Threading;

namespace SAIL.Helpers;

public enum SailSound
{
    FriendlyWelcome,
    XpStartup,
    FriendlySuccess,
    FriendlyClick,
    HorrorMeme,
    HorrorFail,
    ContractSign,
    GameWin
}

public enum SoundCategory
{
    Friendly,
    Horror,
    Click,
    Success,
    Startup
}

/// <summary>Действие в приложении — звук фиксированный или случайный из категории.</summary>
public enum SoundAction
{
    AppStartup,
    Navigate,
    LaunchClick,
    LaunchConfirm,
    ContractSign,
    ContractReject,
    AgreementAccept,
    MemeOverlay,
    MemeDismiss,
    MineExplode,
    MineWin,
    GameWin,
    GameLose,
    GameWrongClick,
    GalleryMeme,
    FunSpin,
    FunPain,
    TitleBarClick,
    RandomSurprise
}

public static class SoundHelper
{
    private static readonly Dictionary<SailSound, string> ResourceMap = new()
    {
        [SailSound.FriendlyWelcome] = "Assets/Sounds/friendly-startup.mp3",
        [SailSound.XpStartup] = "Assets/Sounds/xp-startup.wav",
        [SailSound.FriendlySuccess] = "Assets/Sounds/friendly-success.mp3",
        [SailSound.FriendlyClick] = "Assets/Sounds/click-soft.mp3",
        [SailSound.HorrorMeme] = "Assets/Sounds/horror-sting.mp3",
        [SailSound.HorrorFail] = "Assets/Sounds/horror-creepy.mp3",
        [SailSound.ContractSign] = "Assets/Sounds/friendly-success.mp3",
        [SailSound.GameWin] = "Assets/Sounds/friendly-success.mp3",
    };

    private static readonly IReadOnlyList<SailSound> FriendlyPool =
        [SailSound.FriendlyWelcome, SailSound.FriendlySuccess, SailSound.FriendlyClick, SailSound.XpStartup];

    private static readonly IReadOnlyList<SailSound> HorrorPool =
        [SailSound.HorrorMeme, SailSound.HorrorFail];

    private static readonly string SettingsPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "SAIL PROJECT", "sound-settings.json");

    private static MediaPlayer? _player;
    private static bool _enabled = true;
    private static bool _randomMode = true;
    private static double _volume = 0.55;
    private static DispatcherTimer? _surpriseTimer;

    public static bool Enabled
    {
        get => _enabled;
        set { _enabled = value; SaveSettings(); if (!value) StopSurpriseTimer(); else StartSurpriseTimer(); }
    }

    public static bool RandomMode
    {
        get => _randomMode;
        set { _randomMode = value; SaveSettings(); }
    }

    public static double Volume
    {
        get => _volume;
        set { _volume = Math.Clamp(value, 0, 1); SaveSettings(); }
    }

    static SoundHelper() => LoadSettings();

    /// <summary>Звук для конкретного действия + опционально рандом-бонус.</summary>
    public static void PlayAction(SoundAction action)
    {
        if (!_enabled) return;

        switch (action)
        {
            case SoundAction.AppStartup:
                PlayStartupSequence();
                return;

            case SoundAction.Navigate:
                PlayPick(SailSound.FriendlyClick, SoundCategory.Click);
                MaybeRandomBonus(friendlyChance: 0.18, horrorChance: 0.06);
                break;

            case SoundAction.LaunchClick:
                PlayPick(SailSound.FriendlyClick, SoundCategory.Click);
                MaybeRandomBonus(0.12, 0.04);
                break;

            case SoundAction.LaunchConfirm:
                PlayPick(SailSound.FriendlySuccess, SoundCategory.Success);
                MaybePlayRandom(SoundCategory.Friendly, 0.25);
                break;

            case SoundAction.ContractSign:
            case SoundAction.AgreementAccept:
                PlayPick(SailSound.ContractSign, SoundCategory.Success);
                MaybePlayRandom(SoundCategory.Friendly, 0.3);
                break;

            case SoundAction.ContractReject:
            case SoundAction.GameLose:
                PlayPick(SailSound.HorrorFail, SoundCategory.Horror);
                MaybePlayRandom(SoundCategory.Horror, 0.35);
                break;

            case SoundAction.MemeOverlay:
                PlayHorrorThenFriendly();
                return;

            case SoundAction.MemeDismiss:
                PlayPick(SailSound.FriendlyClick, SoundCategory.Click);
                break;

            case SoundAction.MineExplode:
                PlayPick(SailSound.HorrorMeme, SoundCategory.Horror);
                MaybePlayRandom(SoundCategory.Horror, 0.2);
                break;

            case SoundAction.MineWin:
            case SoundAction.GameWin:
                PlayPick(SailSound.GameWin, SoundCategory.Success);
                MaybePlayRandom(SoundCategory.Friendly, 0.4);
                break;

            case SoundAction.GameWrongClick:
                PlayPick(SailSound.HorrorFail, SoundCategory.Horror);
                MaybePlayRandom(SoundCategory.Horror, 0.15);
                break;

            case SoundAction.GalleryMeme:
            case SoundAction.FunSpin:
                PlayPick(SailSound.FriendlyClick, SoundCategory.Click);
                MaybeRandomBonus(0.35, 0.12);
                break;

            case SoundAction.FunPain:
                PlayPick(SailSound.HorrorFail, SoundCategory.Horror);
                MaybePlayRandom(SoundCategory.Friendly, 0.2);
                break;

            case SoundAction.TitleBarClick:
                MaybePlayRandom(SoundCategory.Click, 0.08);
                break;

            case SoundAction.RandomSurprise:
                if (Random.Shared.NextDouble() < 0.75)
                    PlayRandom(SoundCategory.Friendly);
                else
                    PlayRandom(SoundCategory.Horror);
                break;
        }
    }

    public static void Play(SailSound sound)
    {
        if (!_enabled) return;
        try
        {
            var path = ResolvePath(sound);
            if (path is null) return;
            _player?.Close();
            _player = new MediaPlayer { Volume = _volume };
            _player.Open(path);
            _player.Play();
        }
        catch { /* ignore */ }
    }

    public static void PlayRandom(SoundCategory category)
    {
        if (!_enabled) return;
        var pool = category switch
        {
            SoundCategory.Horror => HorrorPool,
            SoundCategory.Friendly or SoundCategory.Startup => FriendlyPool,
            SoundCategory.Click => new[] { SailSound.FriendlyClick },
            SoundCategory.Success => new[] { SailSound.FriendlySuccess, SailSound.GameWin },
            _ => FriendlyPool
        };
        Play(pool[Random.Shared.Next(pool.Count)]);
    }

    public static void MaybePlayRandom(SoundCategory category, double chance)
    {
        if (!_enabled || !_randomMode) return;
        if (Random.Shared.NextDouble() <= chance)
            PlayRandom(category);
    }

    public static void PlayStartupSequence()
    {
        if (!_enabled) return;
        Play(SailSound.FriendlyWelcome);
        _ = Task.Run(async () =>
        {
            await Task.Delay(450);
            Play(SailSound.XpStartup);
        });
    }

    public static void PlayHorrorThenFriendly(int horrorDelayMs = 1200)
    {
        if (!_enabled) return;
        PlayRandom(SoundCategory.Horror);
        _ = Task.Run(async () =>
        {
            await Task.Delay(horrorDelayMs);
            PlayRandom(SoundCategory.Click);
        });
    }

    public static void StartSurpriseTimer(Dispatcher dispatcher)
    {
        StopSurpriseTimer();
        if (!_enabled || !_randomMode) return;

        _surpriseTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(Random.Shared.Next(50, 95)) };
        _surpriseTimer.Tick += (_, _) =>
        {
            _surpriseTimer.Interval = TimeSpan.FromSeconds(Random.Shared.Next(50, 95));
            if (Random.Shared.NextDouble() < 0.22)
                PlayAction(SoundAction.RandomSurprise);
        };
        _surpriseTimer.Start();
    }

    public static void StopSurpriseTimer()
    {
        _surpriseTimer?.Stop();
        _surpriseTimer = null;
    }

    private static void StartSurpriseTimer()
    {
        if (System.Windows.Application.Current?.Dispatcher is { } d)
            StartSurpriseTimer(d);
    }

    private static void PlayPick(SailSound fixedSound, SoundCategory randomCategory)
    {
        if (_randomMode && Random.Shared.NextDouble() < 0.28)
            PlayRandom(randomCategory);
        else
            Play(fixedSound);
    }

    private static void MaybeRandomBonus(double friendlyChance, double horrorChance)
    {
        if (!_randomMode) return;
        MaybePlayRandom(SoundCategory.Friendly, friendlyChance);
        MaybePlayRandom(SoundCategory.Horror, horrorChance);
    }

    private static Uri? ResolvePath(SailSound sound)
    {
        if (!ResourceMap.TryGetValue(sound, out var resource))
            return null;

        var packUri = new Uri($"pack://application:,,,/{resource}", UriKind.Absolute);
        if (System.Windows.Application.GetResourceStream(packUri) is not null)
            return packUri;

        if (sound == SailSound.XpStartup)
        {
            var alt = new Uri("pack://application:,,,/Assets/Sounds/xp-startup-system.wav", UriKind.Absolute);
            if (System.Windows.Application.GetResourceStream(alt) is not null)
                return alt;

            foreach (var file in new[]
            {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Media", "Windows Startup.wav"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Media", "Windows XP Startup.wav"),
            })
                if (File.Exists(file))
                    return new Uri(file, UriKind.Absolute);
        }

        return null;
    }

    private static void LoadSettings()
    {
        try
        {
            if (!File.Exists(SettingsPath)) return;
            var data = JsonSerializer.Deserialize<SoundSettings>(File.ReadAllText(SettingsPath));
            if (data is null) return;
            _enabled = data.Enabled;
            _randomMode = data.RandomMode;
            _volume = Math.Clamp(data.Volume, 0, 1);
        }
        catch { /* ignore */ }
    }

    private static void SaveSettings()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath)!);
            File.WriteAllText(SettingsPath, JsonSerializer.Serialize(
                new SoundSettings(_enabled, _randomMode, _volume)));
        }
        catch { /* ignore */ }
    }

    private record SoundSettings(bool Enabled, bool RandomMode, double Volume);
}
