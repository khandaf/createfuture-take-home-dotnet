using xDesign.Nutrition.Api.Domain.Enums;

namespace xDesign.Nutrition.Api.Domain.Entity;

public class Food
{
    public required string Name { get; init; }
    public required int Calories { get; init; }
    public required double TotalFat { get; init; }

    public FatRating FatRating => TotalFat switch
    {
        >= 17.5 => FatRating.High,
        <= 3 => FatRating.Low,
        _ => FatRating.Medium
    };
    
    public required string Caffeine { get; init; }
}