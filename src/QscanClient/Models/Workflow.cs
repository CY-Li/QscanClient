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
    [NotifyPropertyChangedFor(nameof(DisplayTargetPath))]
    private string _targetPath = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DisplayTargetPath))]
    private string _destinationType = "LocalPC"; // LocalPC, SMB, FTP

    [ObservableProperty]
    private string _smbUser = string.Empty;

    [ObservableProperty]
    private string _smbPassword = string.Empty;

    // FTP Settings
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DisplayTargetPath))]
    private string _ftpServer = string.Empty;

    [ObservableProperty]
    private string _ftpPort = "21";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DisplayTargetPath))]
    private string _ftpRemoteFolder = string.Empty;

    [ObservableProperty]
    private string _ftpProtocol = "FTP"; // FTP, SFTP

    [ObservableProperty]
    private string _ftpEncryption = "None"; // None, SSL/TLS, Implicit

    [ObservableProperty]
    private string _ftpAuthType = "Normal"; // Normal, Anonymous

    [ObservableProperty]
    private string _ftpLogin = string.Empty;

    [ObservableProperty]
    private string _ftpPassword = string.Empty;

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
    private string _sides = "1 Sided"; // 1 Sided, 2 Sided

    public string DisplaySummary => $"{FileFormat} {(EnableOCR ? "+ OCR" : "")} {(EnableAISmartNaming ? "+ AI" : "")}";

    public string DisplayTargetPath
    {
        get
        {
            if (DestinationType == "FTP")
            {
                var server = FtpServer?.TrimEnd('/');
                var folder = FtpRemoteFolder?.TrimStart('/');
                if (string.IsNullOrEmpty(folder)) return server ?? string.Empty;
                return $"{server}/{folder}";
            }
            return TargetPath;
        }
    }
}
