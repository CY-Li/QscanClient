using CommunityToolkit.Mvvm.ComponentModel;

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

    public MainViewModel()
    {
        // Initial state: start searching for Q30
        UpdateStatus(Q30ConnectionStatus.Searching);
        
        // Placeholder for future implementation:
        // StartMdnsDiscovery(); // This would broadcast the PC name for Q30 to find
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
