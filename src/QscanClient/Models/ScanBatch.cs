using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace QscanClient.Models;

public partial class ScanBatch : ObservableObject
{
    [ObservableProperty]
    private string _id = string.Empty;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private DateTime _timestamp;

    [ObservableProperty]
    private long _totalSize;

    [ObservableProperty]
    private bool _isScanning;

    [ObservableProperty]
    private bool _isNew;

    [ObservableProperty]
    private int _imageCount;

    public ObservableCollection<string> ImagePaths { get; } = new();

    public string DateFormatted => Timestamp.ToString("MM/dd/yyyy");
    
    public string FullDateFormatted => Timestamp.ToString("yyyy/MM/dd HH:mm");

    public string ImageCountFormatted => $"{ImageCount} pages";

    public string SizeFormatted 
    {
        get
        {
            if (TotalSize > 1024 * 1024) return $"{(double)TotalSize / (1024 * 1024):F1} MB";
            return $"{TotalSize / 1024} KB";
        }
    }
}
