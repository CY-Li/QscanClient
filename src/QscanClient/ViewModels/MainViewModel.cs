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
    private bool _isDarkTheme = true;

    [ObservableProperty]
    private int _totalPagesToday;

    [ObservableProperty]
    private int _totalBatchesToday;

    partial void OnIsDarkThemeChanged(bool value)
    {
        ApplyTheme();
    }

    private Views.HomeView? _homeView;
    private Views.SettingsView? _settingsView;
    private Views.DetailView? _detailView;

    public MainViewModel()
    {
        // Initial state
        UpdateStatus(Q30ConnectionStatus.Searching);
        
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
            
            // If we have specific images, use their count to simulate a real document batch
            int imageCount = imageFiles.Count > 0 ? imageFiles.Count : r.Next(3, 10);
            
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
                    // Use the sorted images in order
                    batch.ImagePaths.Add(imageFiles[j % imageFiles.Count]);
                }
                else
                {
                    batch.ImagePaths.Add($"mock_image_{j}.jpg");
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

    [RelayCommand]
    public void Navigate(string destination)
    {
        CurrentView = destination switch
        {
            "Home" => _homeView,
            "Settings" => _settingsView,
            "Detail" => _detailView,
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
}
