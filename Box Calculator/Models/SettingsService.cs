using System.IO;
using System.Text.Json;

namespace Box_Calculator.Models;

public class SettingsService
{
    
    private const string SettingsFile = "settings.json";

    public Settings LoadSettings()
    {
        if (File.Exists(SettingsFile))
        {
            string json = File.ReadAllText(SettingsFile);
            return JsonSerializer.Deserialize<Settings>(json);
        }

        return new Settings
        {
            SheetLength = 152,
            SheetWidth = 152,
            SheetPrice = 900,
            WastagePercent = 10,
            EngravingHourlyRate = 500,
            EngravingTimeInHours = 0.5,
            CoatingMaterialCostPerLiter = 600,
            CoatingCoveragePerLiter = 10,
            CoatingTimeInHours = 1,
            CoatingHourlyRate = 500
        };
    }
    
    public void SaveSettings(Settings settings)
    {
        string json = JsonSerializer.Serialize(settings);
        File.WriteAllText(SettingsFile, json);
    }
}