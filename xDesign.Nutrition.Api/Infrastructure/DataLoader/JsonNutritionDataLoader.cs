using xDesign.Nutrition.Api.Domain.Entity;
using xDesign.Nutrition.Api.Domain.Interface;

namespace xDesign.Nutrition.Api.Infrastructure.DataLoader
{
    public class JsonNutritionDataLoader : INutritionDataLoader
    {
        public IEnumerable<Food> Load(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
