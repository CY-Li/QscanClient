using System.Windows.Controls;

namespace QscanClient.Views;

public partial class WorkflowView : UserControl
{
    public WorkflowView()
    {
        InitializeComponent();
    }

    private void OverlayBorder_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (e.OriginalSource == sender && DataContext is ViewModels.MainViewModel vm && vm.WorkflowVM != null)
        {
            vm.WorkflowVM.CancelSelectionCommand.Execute(null);
        }
    }
}
