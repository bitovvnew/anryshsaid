using System.Windows;
using System.Windows.Threading;
using SAIL.Models;

namespace SAIL.Helpers;

public static class MemeOverlayHelper
{
    public static void Show(Window owner, string? title = null, string? text = null)
    {
        var meme = MemeCatalog.FailMemes[Random.Shared.Next(MemeCatalog.FailMemes.Count)];
        ShowInternal(owner, title ?? meme.Title, text ?? meme.Text);
    }

    public static void ShowInternal(Window owner, string title, string text)
    {
        if (owner is not MainWindow mw)
        {
            MessageBox.Show($"{title}\n\n{text}", "SAIL PROJECT", MessageBoxButton.OK);
            return;
        }

        mw.ShowMemeOverlay(title, text);
    }
}
