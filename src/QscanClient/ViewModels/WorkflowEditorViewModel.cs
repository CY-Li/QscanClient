using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QscanClient.Models;
using System;

namespace QscanClient.ViewModels;

public enum WorkflowDestinationType
{
    LocalPC,
    SMB,
    FTP
}

public partial class WorkflowEditorViewModel : ObservableObject
{
    private readonly MainViewModel _mainVM;
    
    [ObservableProperty]
    private Workflow? _editingWorkflow;

    [ObservableProperty]
    private WorkflowDestinationType _destinationType;

    [ObservableProperty]
    private bool _isNewWorkflow;

    public WorkflowEditorViewModel(MainViewModel mainVM)
    {
        _mainVM = mainVM;
    }

    public void SetupForNew(WorkflowDestinationType type)
    {
        IsNewWorkflow = true;
        DestinationType = type;
        EditingWorkflow = new Workflow 
        { 
            Name = "New Workflow",
            IsNASDestination = type != WorkflowDestinationType.LocalPC,
            NamingFormat = "{YYYY}{MM}{DD}_Scan"
        };
    }

    public void SetupForEdit(Workflow workflow)
    {
        IsNewWorkflow = false;
        EditingWorkflow = workflow; // In a real app, we'd clone this
        
        // Determine type based on properties (simplified for now)
        if (workflow.IsNASDestination)
            DestinationType = WorkflowDestinationType.SMB;
        else
            DestinationType = WorkflowDestinationType.LocalPC;
    }

    [RelayCommand]
    private void Save()
    {
        if (EditingWorkflow == null) return;

        if (IsNewWorkflow)
        {
            _mainVM.WorkflowVM?.Workflows.Add(EditingWorkflow);
        }
        
        _mainVM.Navigate("Workflow");
    }

    [RelayCommand]
    private void Cancel()
    {
        _mainVM.Navigate("Workflow");
    }
}
