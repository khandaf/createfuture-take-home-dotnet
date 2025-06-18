using xDesign.Nutrition.Api.Applications.Dtos;
using xDesign.Nutrition.Api.Domain.Entity;
using xDesign.Nutrition.Api.Domain.Enums;
using xDesign.Nutrition.Api.Domain.Interface;


namespace xDesign.Nutrition.Api.Services;

public class NutritionSearchService
{

    private const int DefaultLimit = 10;
    private readonly IEnumerable<Food>? _items;
    public NutritionSearchService(INutritionDataLoader loader, string fileName)
    {
        _items = loader.Load(fileName).ToList();
    }

    public IEnumerable<Food> SearchNutrition(NutritionSearchRequest request)
    {
        var filtered = _items?
            .Where(food =>
                (!request.MinCalories.HasValue || food.Calories >= request.MinCalories.Value) &&
                (!request.MaxCalories.HasValue || food.Calories <= request.MaxCalories.Value) &&
                (!request.FatRating.HasValue || food.FatRating == request.FatRating.Value)
            );

        // Ensure filtered is not null before passing to SortFoods
        var sortedList = filtered != null
            ? SortFoods(filtered, request.SortCriteria).ToList()
            : new List<Food>();

        int limit = request.Limit > 0 ? request.Limit : DefaultLimit;
        return sortedList.Take(limit);
    }

    private static IEnumerable<Food> SortFoods(IEnumerable<Food> unsorted, IList<Sort> requestSortCriteria)
    {
        if (!requestSortCriteria.Any())
        {
            return unsorted;
        }

        var firstSort = requestSortCriteria.First();
        var sorted = firstSort.SortOrder == SortOrder.Asc
            ? unsorted.OrderBy(KeySelector(firstSort.SortField))
            : unsorted.OrderByDescending(KeySelector(firstSort.SortField));

        foreach (var sort in requestSortCriteria.Skip(1))
        {
            sorted = sort.SortOrder == SortOrder.Asc
                ? sorted.ThenBy(KeySelector(sort.SortField))
                : sorted.ThenByDescending(KeySelector(sort.SortField));
        }

        return sorted.ToList();
    }

    private static Func<Food, object> KeySelector(SortField field)
    {
        return field switch
        {
            SortField.Name => f => f.Name ?? string.Empty,
            SortField.Calories => f => f.Calories,
            _ => throw new ArgumentOutOfRangeException(nameof(field), field, null)
        };
    }



}
