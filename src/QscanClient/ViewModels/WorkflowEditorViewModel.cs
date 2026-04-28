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
    [NotifyPropertyChangedFor(nameof(IsSmbDestination))]
    [NotifyPropertyChangedFor(nameof(IsFtpDestination))]
    [NotifyPropertyChangedFor(nameof(IsNotFtpDestination))]
    [NotifyPropertyChangedFor(nameof(BrowseButtonText))]
    private WorkflowDestinationType _destinationType;

    public bool IsSmbDestination => DestinationType == WorkflowDestinationType.SMB;
    public bool IsFtpDestination => DestinationType == WorkflowDestinationType.FTP;
    public bool IsNotFtpDestination => DestinationType != WorkflowDestinationType.FTP;
    public string BrowseButtonText => IsSmbDestination ? "Search" : "Browse...";

    [ObservableProperty]
    private bool _isNewWorkflow;

    [ObservableProperty]
    private bool _isPasswordVisible;

    [RelayCommand]
    private void TogglePasswordVisibility()
    {
        IsPasswordVisible = !IsPasswordVisible;
    }

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
            DestinationType = type.ToString(),
            IsNASDestination = type != WorkflowDestinationType.LocalPC,
            NamingFormat = "{YYYY}{MM}{DD}_Scan",
            Sides = "1 Sided"
        };
    }

    public void SetupForEdit(Workflow workflow)
    {
        IsNewWorkflow = false;
        EditingWorkflow = workflow; // In a real app, we'd clone this
        
        // Determine type based on model property
        if (Enum.TryParse<WorkflowDestinationType>(workflow.DestinationType, out var result))
        {
            DestinationType = result;
        }
        else if (workflow.IsNASDestination)
        {
            DestinationType = WorkflowDestinationType.SMB;
        }
        else
        {
            DestinationType = WorkflowDestinationType.LocalPC;
        }
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
