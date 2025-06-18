using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using xDesign.Nutrition.Api.Domain.Entity;
using xDesign.Nutrition.Api.Infrastructure.Util;
using xDesign.Nutrition.Api.Domain.Interface;

public class CsvNutritionDataLoader : INutritionDataLoader
{
    private string? _fileName;
    private const string ExpectedServingSize = "100 g";
    private const string FatUnitSuffix = "g";
    public IEnumerable<Food> Load(string fileName)
    {
        // Existing CSV parsing logic here
        _fileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        using var readStream = File.OpenRead(_fileName);
        using var reader = new StreamReader(readStream);
        using var csvReader = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));

        var foods = new List<Food>();

        csvReader.Read();
        csvReader.ReadHeader();
        while (csvReader.Read())
        {
            var name = csvReader.GetField(CsvColumnHeadings.NameField);
            var caloriesStr = csvReader.GetField(CsvColumnHeadings.CaloriesField);
            var fatStr = csvReader.GetField(CsvColumnHeadings.TotalFatField);
            var caffeine = csvReader.GetField(CsvColumnHeadings.CaffeineField);
            var servingSize = csvReader.GetField(CsvColumnHeadings.ServingSizeField);

            if (!IsValidFood(name, caloriesStr, fatStr, caffeine, servingSize))
                continue;

            if (!int.TryParse(caloriesStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var calories))
                continue;

            if (string.IsNullOrWhiteSpace(fatStr) || !fatStr.Contains(FatUnitSuffix))
                continue;

            var fatValue = fatStr[..fatStr.IndexOf(FatUnitSuffix)];
            if (!double.TryParse(fatValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var totalFat))
                continue;

            foods.Add(new Food
            {
                Name = name!,
                Calories = calories,
                TotalFat = totalFat,
                Caffeine = caffeine!
            });
        }

        return foods;
    }
    private static bool IsValidFood(string? name, string? calories, string? totalFat, string? caffeine, string? servingSize)
    {
        var requiredFields = new[] { name, calories, totalFat, caffeine, servingSize };
        if (requiredFields.Any(string.IsNullOrWhiteSpace))
            return false;

        return servingSize == ExpectedServingSize;
    }
}
