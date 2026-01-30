using System.Windows.Controls;

namespace QscanClient.Views;

public partial class HomeView : UserControl
{
    public HomeView()
    {
        InitializeComponent();
        
        // Listen to size changes to update card width
        this.SizeChanged += OnSizeChanged;
    }

    private void OnSizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
    {
        CalculateCardWidth();
    }

    private void CalculateCardWidth()
    {
        // Container width (ScrollViewer or Grid) where cards are hosted
        // Note: We need to reference the actual container of the WrapPanel or calculate based on View width
        // Let's assume the Grid inside ScrollViewer has some margins.
        
        // The Grid with margin "48,10,24,48" inside ScrollViewer which is in col 1.
        // Grid.Column="1" (Star width)
        
        // Approximate available width calculation:
        // Or better, bind the Grid's ActualWidth is simpler if we name it.
        // But for now, let's use the HomeView ActualWidth - LeftColumn(360) - Margins
        
        // Wait, HomeView Side-by-SideHero was reverted. 
        // Now row 0 is Title, row 1 is ScrollViewer.
        // Margin on Grid inside ScrollViewer is 48(L) + 24(R) = 72.
        
        // To be safe, we should add x:Name to the container in XAML, but I can't do that comfortably in .cs only.
        // I will use `this.ActualWidth - 72` as a rough estimate or try to find the container.
        // Actually, simpler approach: Just use this.ActualWidth. 
        // The container margin is inside.
        
        // Let's refine:
        // HomeView is getting full width.
        // Inside is Grid with Rows.
        // ScrollViewer is Row 1.
        // Inside ScrollViewer is Grid Margin="48,10,24,48".
        // So Available Width for Cards = ActualWidth - 48 - 24 = ActualWidth - 72.
        
        // Deduct ScrollBar width (approx 17px) + Safety (3px) = 20px
        double containerWidth = this.ActualWidth - 72 - 20;
        if (containerWidth < 200) return;

        // Card MinWidth = 164 (Border) + 12 (Border Margin 6*2) + 20 (DocCard Margin Right) = ~196
        // Let's strictly follow DocCard definition:
        // Root Margin: 0,0,20,20.
        // Button Padding: 0.
        // Border Margin: 6.
        // Border Width: Min 164.
        // Total Min Box Width = 164 + 6 + 6 = 176 (Visual Card) + 20 (Right Margin) = 196.
        
        double minItemWidth = 196; 
        
        int columns = (int)(containerWidth / minItemWidth);
        if (columns < 1) columns = 1;
        
        // Calculate exact width to fill
        // NewItemWidth * columns = containerWidth
        // (But usually minus a bit to avoid wrapping due to rounding errors)
        
        double newItemWidth = (containerWidth / columns) - 1; // -1 for safety
        
        CardWidth = newItemWidth;
    }

    // Dependency Property for Binding
    public static readonly System.Windows.DependencyProperty CardWidthProperty =
        System.Windows.DependencyProperty.Register("CardWidth", typeof(double), typeof(HomeView), new System.Windows.PropertyMetadata(196.0));

    public double CardWidth
    {
        get { return (double)GetValue(CardWidthProperty); }
        set { SetValue(CardWidthProperty, value); }
    }
}
