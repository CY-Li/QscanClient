using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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

    [ObservableProperty]
    private bool _isEditingTitle;

    // Staging buffer – only committed to Title on confirm
    [ObservableProperty]
    private string _editingTitle = string.Empty;

    public ObservableCollection<string> ImagePaths { get; } = new();

    public string DateFormatted => Timestamp.ToString("MM/dd/yyyy");
    
    public string FullDateFormatted => Timestamp.ToString("yyyy/MM/dd HH:mm");

    public string ImageCountFormatted => $"{ImageCount} pages";

    [ObservableProperty]
    private string _selectedImagePath = string.Empty;

    [RelayCommand]
    public void SelectImage(string path)
    {
        SelectedImagePath = path;
    }

    [RelayCommand]
    private void ToggleEditTitle()
    {
        if (!IsEditingTitle)
        {
            // Enter edit mode: copy current title into staging buffer
            EditingTitle = Title;
            IsEditingTitle = true;
        }
        else
        {
            // Confirm: commit staging buffer → Title
            Title = EditingTitle;
            IsEditingTitle = false;
        }
    }

    [RelayCommand]
    private void CancelEditTitle()
    {
        // Discard changes
        IsEditingTitle = false;
    }

    [RelayCommand]
    public void DeleteImage(string path)
    {
        if (ImagePaths.Contains(path))
        {
            int index = ImagePaths.IndexOf(path);
            bool wasSelected = SelectedImagePath == path;
            
            ImagePaths.Remove(path);
            ImageCount = ImagePaths.Count;
            
            if (wasSelected)
            {
                if (ImagePaths.Count > 0)
                {
                    // Select next image (which now has the same index)
                    // or the previous one if we deleted the last item
                    int newIndex = Math.Min(index, ImagePaths.Count - 1);
                    SelectedImagePath = ImagePaths[newIndex];
                }
                else
                {
                    SelectedImagePath = string.Empty;
                }
            }
        }
    }

    [RelayCommand]
    public void NextImage()
    {
        if (ImagePaths.Count == 0) return;
        
        int index = ImagePaths.IndexOf(SelectedImagePath);
        if (index < ImagePaths.Count - 1)
        {
            SelectedImagePath = ImagePaths[index + 1];
        }
    }

    [RelayCommand]
    public void PreviousImage()
    {
        if (ImagePaths.Count == 0) return;

        int index = ImagePaths.IndexOf(SelectedImagePath);
        if (index > 0)
        {
            SelectedImagePath = ImagePaths[index - 1];
        }
    }

    public string SizeFormatted 
    {
        get
        {
            if (TotalSize > 1024 * 1024) return $"{(double)TotalSize / (1024 * 1024):F1} MB";
            return $"{TotalSize / 1024} KB";
        }
    }
}
