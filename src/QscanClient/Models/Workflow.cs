using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace QscanClient.Models;

public partial class Workflow : ObservableObject
{
    [ObservableProperty]
    private string _id = Guid.NewGuid().ToString();

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _icon = "Icon_Workflow";

    [ObservableProperty]
    private bool _enableOCR = true;

    [ObservableProperty]
    private string _fileFormat = "PDF";

    [ObservableProperty]
    private bool _enableAISmartNaming = false;

    [ObservableProperty]
    private string _namingFormat = "{YYYYMMDD}_{Subject}";

    [ObservableProperty]
    private string _targetPath = string.Empty;

    [ObservableProperty]
    private bool _isNASDestination = false;

    // New Scan Settings
    [ObservableProperty]
    private string _scanMode = "Color"; // Color, Black & White, Grey

    [ObservableProperty]
    private string _paperSize = "Auto"; // Auto, A3, A4, A5, Letter, Long Paper

    [ObservableProperty]
    private string _resolution = "300dpi"; // 600dpi, 300dpi, 200dpi

    [ObservableProperty]
    private string _sides = "One sided"; // One sided, Two sided

    public string DisplaySummary => $"{FileFormat} {(EnableOCR ? "+ OCR" : "")} {(EnableAISmartNaming ? "+ AI" : "")}";
}
