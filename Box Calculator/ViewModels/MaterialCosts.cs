namespace Box_Calculator.ViewModels;

public class MaterialCosts
{
    public double SheetLength { get; set; } // см
    public double SheetWidth { get; set; } // см
    public double SheetPrice { get; set; } // рубли
    public double WastagePercent { get; set; } // процент отходов

    public double CalculateCostPerSquareMeter()
    {
        double sheetArea = (SheetLength * SheetWidth) / 10000; // перевод в м²
        return (SheetPrice / sheetArea) * (1 + WastagePercent / 100);
    }
}