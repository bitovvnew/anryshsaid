using System.Windows;
using System.Windows.Input;

namespace SAIL.Views;

public partial class HomeView : System.Windows.Controls.UserControl
{
    public event EventHandler? NavigateToContract;
    public event EventHandler? NavigateToGallery;
    public event EventHandler? NavigateToFun;
    public event EventHandler? NavigateToMines;

    public HomeView()
    {
        InitializeComponent();
    }

    private void OpenContract_Click(object sender, MouseButtonEventArgs e) =>
        NavigateToContract?.Invoke(this, EventArgs.Empty);

    private void OpenGallery_Click(object sender, MouseButtonEventArgs e) =>
        NavigateToGallery?.Invoke(this, EventArgs.Empty);

    private void OpenFun_Click(object sender, MouseButtonEventArgs e) =>
        NavigateToFun?.Invoke(this, EventArgs.Empty);

    private void OpenMines_Click(object sender, MouseButtonEventArgs e) =>
        NavigateToMines?.Invoke(this, EventArgs.Empty);
}
