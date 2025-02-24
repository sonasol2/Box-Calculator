using Box_Calculator.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Box_Calculator.ViewModels;

// ViewModels/SettingsViewModel.cs
public partial class SettingsViewModel : ViewModelBase
{
    private readonly SettingsService _settingsService;
    
    [ObservableProperty]
    private Settings settings;
    
    public SettingsViewModel()
    {
        _settingsService = new SettingsService();
        Settings = _settingsService.LoadSettings();
    }
    
    [RelayCommand]
    private void SaveSettings()
    {
        _settingsService.SaveSettings(Settings);
    }
    
    partial void OnSettingsChanged(Settings oldValue, Settings newValue)
    {
        if (newValue != null)
        {
            newValue.HasUnsavedChanges = true;
        }
    }
}