using System.Windows.Controls;

namespace QscanClient.Views;

public partial class WorkflowEditorView : UserControl
{
    public WorkflowEditorView()
    {
        InitializeComponent();
    }

    private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        MainScrollViewer.ScrollToHome();
    }
}
