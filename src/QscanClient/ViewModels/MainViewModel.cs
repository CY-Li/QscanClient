using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows;
using System.Linq;
using System.Collections.ObjectModel;
using QscanClient.Models;

namespace QscanClient.ViewModels;

public enum Q30ConnectionStatus
{
    Disconnected,
    Searching,
    Connected,
    WaitingForUpload
}

public enum SettingsCategory
{
    General,
    ScanSettings
}

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private Q30ConnectionStatus _q30Status = Q30ConnectionStatus.Searching;

    [ObservableProperty]
    private string _q30StatusText = "Searching for connection...";

    [ObservableProperty]
    private object? _currentView;

    [ObservableProperty]
    private bool _isScanning;


    [ObservableProperty]
    private int _currentScanningPage = 0;

    [ObservableProperty]
    private int _totalScanningPages = 10;

    [ObservableProperty]
    private ScanBatch? _selectedBatch;

    public ObservableCollection<ScanBatch> Batches { get; } = new();

    [ObservableProperty]
    private bool _isDarkTheme = false;

    [ObservableProperty]
    private int _totalBatchesToday;

    [ObservableProperty]
    private bool _autoCrop = true;

    [ObservableProperty]
    private bool _autoDeskew = true;

    [ObservableProperty]
    private bool _removeBlankPage = false;

    [ObservableProperty]
    private string _appVersion = "1.0.0";

    [ObservableProperty]
    private string _clientName = System.Environment.MachineName;

    [ObservableProperty]
    private SettingsCategory _selectedSettingsCategory = SettingsCategory.General;

    [ObservableProperty]
    private string _quickScanFolder = @"C:\Users\Public\Documents\QscanClient";

    partial void OnIsDarkThemeChanged(bool value)
    {
        ApplyTheme();
    }

    private Views.HomeView? _homeView;
    private Views.SettingsView? _settingsView;
    private Views.DetailView? _detailView;
    private Views.WorkflowView? _workflowView;
    private Views.WorkflowEditorView? _workflowEditorView;


    [ObservableProperty]
    private WorkflowViewModel? _workflowVM;

    [ObservableProperty]
    private WorkflowEditorViewModel? _workflowEditorVM;



    public MainViewModel()
    {
        // Initial state
        UpdateStatus(Q30ConnectionStatus.Searching);
        
        WorkflowVM = new WorkflowViewModel(this);
        WorkflowEditorVM = new WorkflowEditorViewModel(this);

        // Get version from assembly
        var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        if (version != null)
        {
            AppVersion = $"{version.Major}.{version.Minor}.{version.Build}";
        }

        // Mock initial data
        LoadMockData();
    }

    public void Initialize()
    {
        try
        {
            // Link views to this ViewModel for bindings
            _homeView = new Views.HomeView { DataContext = this };
            _settingsView = new Views.SettingsView { DataContext = this };
            _detailView = new Views.DetailView { DataContext = this };
            _workflowView = new Views.WorkflowView { DataContext = this };
            _workflowEditorView = new Views.WorkflowEditorView { DataContext = this };



            // Initial view
            CurrentView = _homeView;
            
            // Start with disconnected status as requested
            UpdateStatus(Q30ConnectionStatus.Disconnected);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Qscan Startup Error (XAML): {ex.Message}\n\nStack: {ex.StackTrace}", "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }


    private void LoadMockData()
    {
        var now = DateTime.Now;
        var r = new Random();

        // Get actual images from specific folder
        string specificPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "..", "pics", "塗鴉智能（Tuya Smart）市場調查報告");
        
        var imageFiles = new System.Collections.Generic.List<string>();
        
        if (System.IO.Directory.Exists(specificPath))
        {
            imageFiles.AddRange(System.IO.Directory.GetFiles(specificPath, "*.png"));
            imageFiles.AddRange(System.IO.Directory.GetFiles(specificPath, "*.jpg"));
            // Ensure they are sorted so pages appear in order
            imageFiles.Sort();
        }

        for (int i = 0; i < 8; i++)
        {
            var date = now.AddDays(-i).AddHours(-r.Next(0, 12));
            
            int imageCount = imageFiles.Count > 0 ? imageFiles.Count : 3;
            
            var batch = new ScanBatch 
            { 
                Title = date.ToString("yyyyMMdd_HHmmss"), 
                Timestamp = date, 
                ImageCount = imageCount 
            };

            for (int j = 0; j < imageCount; j++)
            {
                if (imageFiles.Count > 0)
                {
                    batch.ImagePaths.Add(imageFiles[j % imageFiles.Count]);
                }
                else
                {
                    // Ensure the mock file actually exists on disk so MAPI can find it
                    string mockPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"mock_page_{j}.jpg");
                    if (!System.IO.File.Exists(mockPath))
                    {
                        System.IO.File.WriteAllBytes(mockPath, new byte[1024]);
                    }
                    batch.ImagePaths.Add(mockPath);
                }
            }

            Batches.Add(batch);
        }
    }

    [RelayCommand]
    public async System.Threading.Tasks.Task SimulateScan()
    {
        if (IsScanning) 
        {
            IsScanning = false;
            UpdateStatus(Q30ConnectionStatus.Connected);
            return;
        }

        IsScanning = true;
        UpdateStatus(Q30ConnectionStatus.Connected);
        CurrentScanningPage = 0;

        // Simulate passive reception of pages
        // In a real scenario, this would loop until a 'Batch Complete' packet is received
        int randomTotal = new Random().Next(5, 20); 

        for (int i = 1; i <= randomTotal; i++)
        {
            if (!IsScanning) break;
            
            CurrentScanningPage = i;
            CurrentScanningPage = i;
            
            // Simulate page transfer time
            await System.Threading.Tasks.Task.Delay(500); 
        }

        if (IsScanning)
        {
            // Auto addition of new batch on completion
            var now = DateTime.Now;
            var newBatch = new ScanBatch 
            { 
                Title = now.ToString("yyyyMMdd_HHmmss"), 
                Timestamp = now, 
                ImageCount = CurrentScanningPage,
                IsNew = true // Trigger highlight
            };

            // Populate ImagePaths so Mail functionality has files to attach
            string scanDir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SimulatedScans");
            if (!System.IO.Directory.Exists(scanDir))
                System.IO.Directory.CreateDirectory(scanDir);

            var random = new Random();
            for (int i = 1; i <= CurrentScanningPage; i++)
            {
                // Simulate a mix of PDFs and Images to demonstrate support for both
                string ext = random.Next(0, 2) == 0 ? ".pdf" : ".jpg";
                string filePath = System.IO.Path.Combine(scanDir, $"page_{i}{ext}");
                
                if (!System.IO.File.Exists(filePath))
                    System.IO.File.WriteAllBytes(filePath, new byte[2048]);
                
                newBatch.ImagePaths.Add(filePath);
            }
            
            Batches.Insert(0, newBatch); // Add to top of list
            SelectedBatch = newBatch; // Auto-select? Maybe just highlight.
            IsScanning = false;
            IsScanning = false;
            
            // Auto-disconnect after scan as requested
            UpdateStatus(Q30ConnectionStatus.Disconnected);

            // Auto-remove highlight after 5 seconds
            _ = System.Threading.Tasks.Task.Delay(5000).ContinueWith(_ => 
            {
                Application.Current.Dispatcher.Invoke(() => 
                {
                    newBatch.IsNew = false;
                });
            });
        }
        else
        {
            UpdateStatus(Q30ConnectionStatus.Searching);
        }
    }

    [RelayCommand]
    public void SelectBatch(ScanBatch batch)
    {
        SelectedBatch = batch;
        if (batch != null && batch.ImagePaths.Count > 0 && string.IsNullOrEmpty(batch.SelectedImagePath))
        {
            batch.SelectedImagePath = batch.ImagePaths[0];
        }
        Navigate("Detail");
    }

    [RelayCommand]
    public void DeleteBatch()
    {
        if (SelectedBatch != null)
        {
            Batches.Remove(SelectedBatch);
            SelectedBatch = null;
            Navigate("Home");
        }
    }

    [ObservableProperty]
    private bool _isProcessingEmail;

    [RelayCommand]
    public async Task MailBatch()
    {
        if (SelectedBatch == null) return;
        try
        {
            var filePaths = SelectedBatch.ImagePaths.Where(p => System.IO.File.Exists(p)).ToList();
            if (filePaths.Count == 0)
            {
                MessageBox.Show("目前找不到任何可附加的檔案。", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            IsProcessingEmail = true;

            // EmailService handles both Outlook (via COM) and other clients (via MAPI/EML).
            // It automatically supports any file type including PDFs and ZIPs them.
            await QscanClient.Services.EmailService.SendMail(SelectedBatch.Title, filePaths);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            // Fallback to mailto if generation fails completely
            try
            {
                var subject = Uri.EscapeDataString(SelectedBatch.Title);
                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = $"mailto:?subject={subject}",
                    UseShellExecute = true
                };
                System.Diagnostics.Process.Start(psi);
            }
            catch
            {
                MessageBox.Show("無法開啟郵件客戶端，請確認系統已設定預設郵件程式。",
                                "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        finally
        {
            IsProcessingEmail = false;
        }
    }

    [RelayCommand]
    public void PrintBatch()
    {
        if (SelectedBatch == null || SelectedBatch.ImagePaths.Count == 0) return;

        var dlg = new System.Windows.Controls.PrintDialog();
        if (dlg.ShowDialog() != true) return;

        var doc = new System.Windows.Documents.FixedDocument();
        doc.DocumentPaginator.PageSize = new System.Windows.Size(dlg.PrintableAreaWidth, dlg.PrintableAreaHeight);

        foreach (var path in SelectedBatch.ImagePaths)
        {
            var pageContent = new System.Windows.Documents.PageContent();
            var fixedPage = new System.Windows.Documents.FixedPage
            {
                Width = dlg.PrintableAreaWidth,
                Height = dlg.PrintableAreaHeight
            };

            var img = new System.Windows.Controls.Image
            {
                Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(path, UriKind.Absolute)),
                Width = dlg.PrintableAreaWidth,
                Height = dlg.PrintableAreaHeight,
                Stretch = System.Windows.Media.Stretch.Uniform
            };

            fixedPage.Children.Add(img);
            ((System.Windows.Markup.IAddChild)pageContent).AddChild(fixedPage);
            doc.Pages.Add(pageContent);
        }

        dlg.PrintDocument(doc.DocumentPaginator, SelectedBatch.Title);
    }

    [RelayCommand]
    public void Navigate(string destination)
    {
        // Reset editing state when leaving Detail view
        if (destination != "Detail" && SelectedBatch != null)
            SelectedBatch.IsEditingTitle = false;

        // Default to General tab when entering Settings
        if (destination == "Settings")
        {
            SelectedSettingsCategory = SettingsCategory.General;
        }

        CurrentView = destination switch
        {
            "Home" => _homeView,
            "Settings" => _settingsView,
            "Detail" => _detailView,
            "Workflow" => _workflowView,
            "WorkflowEditor" => _workflowEditorView,

            _ => (object?)_homeView
        };
    }

    [RelayCommand]
    public void ToggleTheme()
    {
        IsDarkTheme = !IsDarkTheme;
        ApplyTheme();
    }

    private void ApplyTheme()
    {
        var dict = Application.Current.Resources.MergedDictionaries;
        var themeDict = dict.FirstOrDefault(d => d.Source != null && (d.Source.OriginalString.Contains("Theme.xaml")));
        
        if (themeDict != null)
        {
            dict.Remove(themeDict);
        }

        var newSource = IsDarkTheme 
            ? new Uri("Themes/DarkTheme.xaml", UriKind.Relative) 
            : new Uri("Themes/LightTheme.xaml", UriKind.Relative);
            
        dict.Add(new ResourceDictionary { Source = newSource });
    }

    /// <summary>
    /// Placeholder for mDNS service that makes this PC discoverable by Q30.
    /// </summary>
    private void StartMdnsDiscovery()
    {
        // TODO: Implement DNS-SD advertisement (e.g., using Makaretu.Dns or similar)
    }

    /// <summary>
    /// Placeholder for WebSocket server to receive scans from Q30.
    /// </summary>
    private void SetupWebSocketServer()
    {
        // TODO: Implement WebSocket server to listen for Q30 upload requests
        // Once connected, call UpdateStatus(Q30ConnectionStatus.Connected);
    }

    public void UpdateStatus(Q30ConnectionStatus status)
    {
        Q30Status = status;
        Q30StatusText = status switch
        {
            Q30ConnectionStatus.Disconnected => "Searching for connection...",
            Q30ConnectionStatus.Searching => "Searching for connection...",
            Q30ConnectionStatus.Connected => "Plustek Q30 Connected",
            Q30ConnectionStatus.WaitingForUpload => "Plustek Q30 Connected",
            _ => "Unknown Status"
        };
    }

    [RelayCommand]
    private void BrowseQuickScanFolder()
    {
        var dialog = new Microsoft.Win32.OpenFolderDialog
        {
            Title = "Select Quick Scan Folder",
            InitialDirectory = QuickScanFolder
        };

        if (dialog.ShowDialog() == true)
        {
            QuickScanFolder = dialog.FolderName;
        }
    }
}
