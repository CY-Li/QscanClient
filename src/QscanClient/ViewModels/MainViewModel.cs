using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Windows;
using System.Linq;

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
    private string _q30StatusText = "Searching for Q30 Scanner...";

    [ObservableProperty]
    private object _currentView;

    [ObservableProperty]
    private bool _isDarkTheme = true;

    partial void OnIsDarkThemeChanged(bool value)
    {
        ApplyTheme();
    }

    private readonly Views.HomeView _homeView = new();
    private readonly Views.SettingsView _settingsView = new();

    public MainViewModel()
    {
        // Initial state
        UpdateStatus(Q30ConnectionStatus.Searching);
        
        // Link views to this ViewModel for bindings
        _homeView.DataContext = this;
        _settingsView.DataContext = this;

        // Initial view
        CurrentView = _homeView;
    }

    [RelayCommand]
    public void Navigate(string destination)
    {
        CurrentView = destination switch
        {
            "Home" => _homeView,
            "Settings" => _settingsView,
            _ => _homeView
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
            Q30ConnectionStatus.Disconnected => "Q30 Scanner: Disconnected",
            Q30ConnectionStatus.Searching => "Searching for Q30 connection...",
            Q30ConnectionStatus.Connected => "Q30 Scanner: Connected",
            Q30ConnectionStatus.WaitingForUpload => "Q30 Scanner: Connected - Waiting for scan...",
            _ => "Unknown Status"
        };
    }
}
