using xDesign.Nutrition.Api.Domain.Entity;

namespace xDesign.Nutrition.Api.Domain.Interface
{
    public interface INutritionDataLoader
    {
        IEnumerable<Food> Load(string filePath);
    }

}
