using System.Text.Json.Serialization;
using xDesign.Nutrition.Api.Infrastructure.Util;
using xDesign.Nutrition.Api.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(new UpperCaseJsonNamingPolicy()));
    });

var config = builder.Configuration.GetSection("NutritionSearch");
var format = config["Format"];
var fileName = config["FileName"];

var loader = NutritionDataLoaderFactory.Create(format ?? throw new ArgumentNullException("format"));
builder.Services.AddSingleton(loader);
builder.Services.AddSingleton(fileName ?? throw new ArgumentNullException("fileNAme"));
builder.Services.AddScoped(_ => new NutritionSearchService(loader, fileName ?? throw new ArgumentNullException("fileName")));
var app = builder.Build();

app.MapControllers();

app.Run();

public partial class Program { };