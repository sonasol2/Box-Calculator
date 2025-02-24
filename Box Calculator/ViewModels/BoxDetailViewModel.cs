using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Box_Calculator.ViewModels;

public partial class BoxDetailViewModel : ViewModelBase
{
    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private double length;

    [ObservableProperty]
    private double width;

    // [ObservableProperty]
    // private double thickness;

    [ObservableProperty]
    private int quantity = 1;
    
    public ICommand? RemoveCommand { get; set; }


}