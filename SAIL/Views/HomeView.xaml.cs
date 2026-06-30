using System.Windows;
using System.Windows.Input;

namespace SAIL.Views;

public partial class HomeView : System.Windows.Controls.UserControl
{
    public event EventHandler? NavigateToContract;
    public event EventHandler? NavigateToGallery;

    public HomeView()
    {
        InitializeComponent();
    }

    private void OpenContract_Click(object sender, MouseButtonEventArgs e)
    {
        NavigateToContract?.Invoke(this, EventArgs.Empty);
    }

    private void OpenGallery_Click(object sender, MouseButtonEventArgs e)
    {
        NavigateToGallery?.Invoke(this, EventArgs.Empty);
    }
}
