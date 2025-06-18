using xDesign.Nutrition.Api.Domain.Interface;
using xDesign.Nutrition.Api.Infrastructure.DataLoader;

namespace xDesign.Nutrition.Api.Infrastructure.Util
{
    public static class NutritionDataLoaderFactory
    {
        public static INutritionDataLoader Create(string format)
        {
            return format.ToLower() switch
            {
                "csv" => new CsvNutritionDataLoader(),
                "json" => new JsonNutritionDataLoader(),
                "xml" => new XmlNutritionDataLoader(),
                _ => throw new NotSupportedException($"Format '{format}' is not supported.")
            };
        }
    }
}
