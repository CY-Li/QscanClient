using System.Windows.Controls;

namespace QscanClient.Views;

public partial class DetailView : UserControl
{
    public DetailView()
    {
        InitializeComponent();
        this.Loaded += (s, e) => this.Focus();
    }

    private void ZoomIn_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        TheZoomBorder.ZoomIn();
    }

    private void ZoomOut_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        TheZoomBorder.ZoomOut();
    }

    private void Reset_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        TheZoomBorder.Reset();
    }
}
