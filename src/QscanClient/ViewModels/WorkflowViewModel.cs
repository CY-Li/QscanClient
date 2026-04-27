using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QscanClient.Models;
using System.Linq;

namespace QscanClient.ViewModels;

public partial class WorkflowViewModel : ObservableObject
{
    private readonly MainViewModel _mainViewModel;

    public ObservableCollection<Workflow> Workflows { get; } = new();

    [ObservableProperty]
    private bool _isEditing;

    [ObservableProperty]
    private Workflow? _editingWorkflow;

    [ObservableProperty]
    private bool _isSelectingDestination;

    public bool IsEmpty => Workflows.Count == 0;


    public WorkflowViewModel(MainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
        Workflows.CollectionChanged += (s, e) => OnPropertyChanged(nameof(IsEmpty));
        LoadMockWorkflows();
    }

    private void LoadMockWorkflows()
    {
        // Start with an empty list for production, or add different mocks if needed
    }

    [RelayCommand]
    private void AddWorkflow()
    {
        IsSelectingDestination = true;
    }

    [RelayCommand]
    private void SelectDestination(string typeStr)
    {
        if (Enum.TryParse<WorkflowDestinationType>(typeStr, out var type))
        {
            IsSelectingDestination = false;
            _mainViewModel.WorkflowEditorVM?.SetupForNew(type);
            _mainViewModel.Navigate("WorkflowEditor");
        }
    }

    [RelayCommand]
    private void CancelSelection()
    {
        IsSelectingDestination = false;
    }


    [RelayCommand]
    private void EditWorkflow(Workflow workflow)
    {
        _mainViewModel.WorkflowEditorVM?.SetupForEdit(workflow);
        _mainViewModel.Navigate("WorkflowEditor");
    }


    [RelayCommand]
    private void SaveWorkflow()
    {
        if (EditingWorkflow == null) return;
        var existing = Workflows.FirstOrDefault(w => w.Id == EditingWorkflow.Id);
        if (existing != null)
        {
            existing.Name = EditingWorkflow.Name;
            existing.EnableOCR = EditingWorkflow.EnableOCR;
            existing.FileFormat = EditingWorkflow.FileFormat;
            existing.EnableAISmartNaming = EditingWorkflow.EnableAISmartNaming;
            existing.NamingFormat = EditingWorkflow.NamingFormat;
            existing.TargetPath = EditingWorkflow.TargetPath;
            existing.IsNASDestination = EditingWorkflow.IsNASDestination;
        }
        else
        {
            Workflows.Add(EditingWorkflow);
        }
        IsEditing = false;
        EditingWorkflow = null;
    }

    [RelayCommand]
    private void CancelEdit()
    {
        IsEditing = false;
        EditingWorkflow = null;
    }

    [RelayCommand]
    private void DeleteWorkflow(Workflow workflow)
    {
        Workflows.Remove(workflow);
    }

    [RelayCommand]
    private void RunWorkflow(Workflow workflow)
    {
        // Placeholder for the actual execution logic
        System.Windows.MessageBox.Show($"Executing workflow: {workflow.Name}\nTarget: {workflow.TargetPath}", "Workflow Execution", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
    }
}


