using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SAIL.Helpers;

namespace SAIL.Views;

public partial class ContractView : UserControl
{
    public event EventHandler? ContractSigned;

    public ContractView()
    {
        InitializeComponent();
    }

    private void Sign_Click(object sender, RoutedEventArgs e)
    {
        if (!AgreePercent.IsChecked == true || !AgreeForever.IsChecked == true ||
            !AgreeLaugh.IsChecked == true || !AgreeMemes.IsChecked == true)
        {
            MessageBox.Show(
                "Сначала поставь все галочки!\n\nБез них договор юридически недействителен\n(шутка, он и так недействителен, но галочки нужны).",
                "SAIL PROJECT — Договор",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            return;
        }

        var nameA = PartyAName.Text.Trim();
        var nameB = PartyBName.Text.Trim();

        if (nameA.StartsWith("Введите") || nameB.StartsWith("Введите") ||
            string.IsNullOrWhiteSpace(nameA) || string.IsNullOrWhiteSpace(nameB))
        {
            MessageBox.Show(
                "Введи имена обеих сторон!\n\nБез имён это просто красивая бумажка.",
                "SAIL PROJECT — Договор",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            return;
        }

        PartyASignature.Text = nameA;
        PartyASignature.Foreground = new SolidColorBrush(Color.FromRgb(0x30, 0xD1, 0x58));
        PartyBSignature.Text = nameB;
        PartyBSignature.Foreground = new SolidColorBrush(Color.FromRgb(0x00, 0x78, 0xD4));

        SignButton.IsEnabled = false;
        SignButton.Content = "✅ Подписано! 15% — навсегда";

        SoundHelper.PlayAction(SoundAction.ContractSign);

        MessageBox.Show(
            $"Поздравляем, {nameA}!\n\n" +
            $"Ты только что подписал договор SAIL PROJECT с {nameB}.\n" +
            $"15% с каждой продажи Roblox-аккаунта — теперь официально (нет).\n\n" +
            $"🔏 Цифровая подпись: dolbaeb productions\n" +
            $"Версия: bito 0.0.0.1\n\n" +
            $"💸 Сохрани скриншот. Он тебе понадобится.",
            "SAIL PROJECT — Договор подписан! 🎉",
            MessageBoxButton.OK,
            MessageBoxImage.Information);

        ContractSigned?.Invoke(this, EventArgs.Empty);
    }

    private void Reject_Click(object sender, RoutedEventArgs e)
    {
        SoundHelper.PlayAction(SoundAction.ContractReject);
        var window = Window.GetWindow(this);
        var game = new RejectContractGameWindow { Owner = window };
        game.ShowDialog();

        if (game.EscapeSuccessful)
        {
            MessageBox.Show(
                "Ты победил в мини-игре!\n\nНо договор 15% всё равно рекомендуется подписать 😈",
                "SAIL PROJECT — Побег",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }

    private void Print_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show(
            "Печать договора...\n\n(На самом деле принтер не подключён, но представь, что распечатал.\nПовесь на стену — пусть все видят твои 15%.)",
            "SAIL PROJECT — Печать",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }
}
