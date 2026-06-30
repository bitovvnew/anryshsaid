using System.Windows;
using System.Windows.Controls;
using SAIL.Helpers;
using SAIL.Models;

namespace SAIL.Views;

public partial class FunZoneView : UserControl
{
    public FunZoneView()
    {
        InitializeComponent();
        Loaded += (_, _) => QuotesList.ItemsSource = MemeCatalog.RandomQuotes;
    }

    private void SpinQuote_Click(object sender, RoutedEventArgs e)
    {
        SoundHelper.PlayAction(SoundAction.FunSpin);
        var quotes = MemeCatalog.RandomQuotes;
        QuoteText.Text = quotes[Random.Shared.Next(quotes.Count)];
    }

    private void GeneratePain_Click(object sender, RoutedEventArgs e)
    {
        SoundHelper.PlayAction(SoundAction.FunPain);
        var price = Random.Shared.Next(500, 50000);
        var cut = (int)(price * 0.15m);
        var phrases = new[]
        {
            $"Продал за {price:N0}₽ → друг забрал {cut:N0}₽",
            $"Твои {cut:N0}₽ уже в мечтах друга",
            $"{cut:N0}₽ = 15% твоей души",
            $"Robux gone, {cut:N0}₽ gone, happiness gone"
        };
        PainText.Text = phrases[Random.Shared.Next(phrases.Length)];
    }

    private void PlayEscape_Click(object sender, RoutedEventArgs e)
    {
        var window = Window.GetWindow(this);
        var game = new RejectContractGameWindow { Owner = window };
        game.ShowDialog();
    }
}
