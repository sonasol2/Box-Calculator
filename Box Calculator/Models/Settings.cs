using System.Text.Json.Serialization;

namespace Box_Calculator.Models;

public class Settings
{
    public double SheetLength { get; set; }
    public double SheetWidth { get; set; }
    public double SheetPrice { get; set; }
    public double WastagePercent { get; set; }
    public double EngravingHourlyRate { get; set; }
    public double EngravingTimeInHours { get; set; }
    public double CoatingMaterialCostPerLiter { get; set; }
    public double CoatingCoveragePerLiter { get; set; }
    public double CoatingTimeInHours { get; set; }
    public double CoatingHourlyRate { get; set; }
    public double Markup { get; set; } = 30; // Значение по умолчанию
    [JsonIgnore]
    public bool HasUnsavedChanges { get; set; }
    
}

