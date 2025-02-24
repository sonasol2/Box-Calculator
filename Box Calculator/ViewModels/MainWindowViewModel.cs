using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Controls;
using Box_Calculator.Models;
using ReactiveUI;

namespace Box_Calculator.ViewModels
{
    
    public partial class MainWindowViewModel : ViewModelBase
{
    
    private readonly MaterialCosts _materialCosts;
    private readonly AdditionalServices _additionalServices;
    
    public ICommand RemoveDetailCommand => new RelayCommand<BoxDetailViewModel>(RemoveDetail);


    [ObservableProperty]
    private SettingsViewModel settingsViewModel;
    
    [ObservableProperty]
    private double totalCost;

    [ObservableProperty]
    private double markup = 30;

    [ObservableProperty]
    private bool includeEngraving;

    [ObservableProperty]
    private bool includeCoating;

    [ObservableProperty]
    private ObservableCollection<BoxDetailViewModel> details = new();
    
    partial void OnMarkupChanged(double oldValue, double newValue)
    {
        var settings = SettingsViewModel.Settings;
        settings.Markup = newValue;
        SettingsViewModel.SaveSettingsCommand.Execute(null);
    }

    public MainWindowViewModel()
    {
        var settingsService = new SettingsService();
        var settings = settingsService.LoadSettings();
        SettingsViewModel = new SettingsViewModel();

        Markup = settings.Markup; // Загрузка последней наценки

        
        Details = new ObservableCollection<BoxDetailViewModel>();
        
        // Инициализация параметров материала
        _materialCosts = new MaterialCosts
        {
            SheetLength = settings.SheetLength,
            SheetWidth = settings.SheetWidth,
            SheetPrice = settings.SheetPrice,
            WastagePercent = settings.WastagePercent
        };

        // Инициализация параметров дополнительных услуг
        _additionalServices = new AdditionalServices
        {
            EngravingHourlyRate = settings.EngravingHourlyRate,
            EngravingTimeInHours = settings.EngravingTimeInHours,
            CoatingMaterialCostPerLiter = settings.CoatingMaterialCostPerLiter,
            CoatingCoveragePerLiter = settings.CoatingCoveragePerLiter,
            CoatingTimeInHours = settings.CoatingTimeInHours,
            CoatingHourlyRate = settings.CoatingHourlyRate
        };
    }

    
    public async Task<bool> ShowUnsavedChangesWarningAsync(Window parentWindow)
    {
        // Создаём окно без содержимого
        var dialog = new Window
        {
            Width = 400,
            Height = 150
        };

        // Добавляем содержимое после объявления диалога
        dialog.Content = new StackPanel
        {
            Spacing = 10,
            Children =
            {
                new TextBlock 
                { 
                    Text = "Настройки не сохранены. Выйти без сохранения?", 
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center 
                },
                new StackPanel
                {
                    Orientation = Avalonia.Layout.Orientation.Horizontal,
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    Spacing = 10,
                    Children =
                    {
                        new Button 
                        { 
                            Content = "Да", 
                            Command = ReactiveCommand.Create(() => dialog.Close(true)) 
                        },
                        new Button 
                        { 
                            Content = "Нет", 
                            Command = ReactiveCommand.Create(() => dialog.Close(false)) 
                        }
                    }
                }
            }
        };

        // Показываем диалоговое окно и возвращаем результат
        return await dialog.ShowDialog<bool>(parentWindow);
    }

    
    [RelayCommand]
    private void Calculate()
    {
        // Получаем стоимость материала за квадратный метр
        double costPerSquareMeter = _materialCosts.CalculateCostPerSquareMeter();
        double totalArea = 0;
        double materialCost = 0;

        // Расчет стоимости материалов (толщина больше не учитывается)
        foreach (var detail in Details)
        {
            double detailArea = (detail.Length * detail.Width * detail.Quantity) / 10000; // перевод в м²
            totalArea += detailArea;
            materialCost += detailArea * costPerSquareMeter; // стоимость теперь зависит только от площади
        }

        // Расчет стоимости дополнительных услуг
        double additionalCost = 0;
        if (IncludeEngraving)
        {
            additionalCost += _additionalServices.CalculateEngravingCost();
        }
        if (IncludeCoating)
        {
            additionalCost += _additionalServices.CalculateCoatingCost(totalArea);
        }

        // Расчет общей стоимости
        double baseCost = materialCost + additionalCost;
        TotalCost = baseCost * (1 + Markup / 100); // Наценка
    }

    
    [RelayCommand]
    private void AddDetail()
    {
        Details.Add(new BoxDetailViewModel());
        foreach (var detail in Details)
        {
            detail.RemoveCommand = RemoveDetailCommand;
        }
    }
    
    private void RemoveDetail(BoxDetailViewModel detail)
    {
        if (Details.Contains(detail))
        {
            Details.Remove(detail);
        }
    }
}
}