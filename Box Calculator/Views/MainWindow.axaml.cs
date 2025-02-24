using System;
using System.ComponentModel;
using System.Windows.Input;
using Avalonia.Controls;
using Box_Calculator.ViewModels;

namespace Box_Calculator.Views;

public partial class MainWindow : Window
{

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this.Resources["MainViewModel"];

        this.Closing += OnWindowClosing;

    }

    private async void OnWindowClosing(object sender, CancelEventArgs e)
    {
        var vm = DataContext as MainWindowViewModel;
        if (vm != null && vm.SettingsViewModel.Settings.HasUnsavedChanges)
        {
            // Передаём текущее окно как родителя
            var result = await vm.ShowUnsavedChangesWarningAsync(this);
            if (!result)
            {
                e.Cancel = true;
            }
        }
    }

}
