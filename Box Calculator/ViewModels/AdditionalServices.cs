using System.Windows.Input;

namespace Box_Calculator.ViewModels;

public class AdditionalServices
{
    public double EngravingHourlyRate { get; set; }
    public double EngravingTimeInHours { get; set; }
    public double CoatingMaterialCostPerLiter { get; set; }
    public double CoatingCoveragePerLiter { get; set; }
    public double CoatingTimeInHours { get; set; }
    public double CoatingHourlyRate { get; set; }

    public double CalculateEngravingCost()
    {
        return EngravingHourlyRate * EngravingTimeInHours;
    }

    public double CalculateCoatingCost(double totalAreaInSquareMeters)
    {
        double materialCost = (totalAreaInSquareMeters / CoatingCoveragePerLiter) * CoatingMaterialCostPerLiter;
        double laborCost = CoatingHourlyRate * CoatingTimeInHours;
        return materialCost + laborCost;
    }
}