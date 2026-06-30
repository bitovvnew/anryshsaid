using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace SAIL.Views;

public partial class GalleryView : UserControl
{
    private static readonly Dictionary<string, string> LaughMessages = new()
    {
        ["meme1"] = "АХАХАХА! 750₽ улетают другу, а ты стоишь с 4250₽ и думаешь «ну норм» 😂",
        ["meme2"] = "Это лицо, когда понял что 15% — это НАВСЕГДА 💀",
        ["money"] = "750₽ + 1500₽ + 3000₽ = друг на пенсии, ты на ramen 🍜",
        ["deal"] = "Рукопожатие заключено! Юридически бессмысленно, морально — больно 🤝"
    };

    public GalleryView()
    {
        InitializeComponent();
    }

    private void Laugh_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button btn) return;
        var tag = btn.Tag?.ToString() ?? "meme1";
        var msg = LaughMessages.GetValueOrDefault(tag, "ОБДРИСАЛСЯ ОТ СМЕХА! 🤣🤣🤣");
        MessageBox.Show(msg, "SAIL — Галерея", MessageBoxButton.OK, MessageBoxImage.None);
    }

    private void Calculate_Click(object sender, RoutedEventArgs e)
    {
        if (!decimal.TryParse(PriceInput.Text.Replace(",", "."),
                NumberStyles.Any, CultureInfo.InvariantCulture, out var price) || price < 0)
        {
            ResultText.Text = "??? ₽";
            MessageBox.Show("Введи нормальную цену, не «бесплатно»!", "SAIL", MessageBoxButton.OK);
            return;
        }

        var cut = price * 0.15m;
        ResultText.Text = $"{cut:N0} ₽";

        var remaining = price - cut;
        MessageBox.Show(
            $"Продажа: {price:N0} ₽\n" +
            $"Друг забирает: {cut:N0} ₽ (15%)\n" +
            $"Тебе остаётся: {remaining:N0} ₽\n\n" +
            $"Не забудь — это по договору! 📜",
            "SAIL — Калькулятор боли",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }
}
