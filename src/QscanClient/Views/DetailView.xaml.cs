using System.Windows;
using System.Windows.Controls;

namespace QscanClient.Views;

public partial class DetailView : UserControl
{
    public DetailView()
    {
        InitializeComponent();
        this.Loaded += (s, e) => this.Focus();
    }

    private void ZoomIn_Click(object sender, RoutedEventArgs e)
    {
        TheZoomBorder.ZoomIn();
    }

    private void ZoomOut_Click(object sender, RoutedEventArgs e)
    {
        TheZoomBorder.ZoomOut();
    }

    private void Reset_Click(object sender, RoutedEventArgs e)
    {
        TheZoomBorder.Reset();
    }

    private void TitleEditTextBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is TextBox tb && tb.IsVisible)
        {
            tb.Focus();
            tb.SelectAll();
        }
    }
}
