using System.Windows;
using System.Windows.Controls;
using SAIL.Helpers;
using SAIL.Models;

namespace SAIL.Views;

public partial class GalleryView : UserControl
{
    public GalleryView()
    {
        InitializeComponent();
        Loaded += (_, _) => MemeItems.ItemsSource = MemeCatalog.All;
    }

    private void Laugh_Click(object sender, RoutedEventArgs e)
    {
        SoundHelper.PlayAction(SoundAction.GalleryMeme);
        if (sender is not Button { Tag: MemeCard card }) return;
        MessageBox.Show(card.LaughMessage, "SAIL — Галерея", MessageBoxButton.OK, MessageBoxImage.None);
    }

    private void Calculate_Click(object sender, RoutedEventArgs e)
    {
        if (!decimal.TryParse(PriceInput.Text.Replace(",", "."),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out var price) || price < 0)
        {
            ResultText.Text = "??? ₽";
            return;
        }

        var cut = price * 0.15m;
        ResultText.Text = $"{cut:N0} ₽";
        MessageBox.Show(
            $"Продажа: {price:N0} ₽\nДруг: {cut:N0} ₽ (15%)\nТебе: {price - cut:N0} ₽",
            "SAIL — Калькулятор боли",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }
}
