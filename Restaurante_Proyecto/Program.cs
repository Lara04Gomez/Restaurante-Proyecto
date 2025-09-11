using Application.Enums;
using Application.Interfaces;
using Application.Interfaces.ICategory;
using Application.Interfaces.IDish;
using Application.Services;
using Application.Services.CategoryService;
using Application.Services.DishServices;
using Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Command;
using Infrastructure.Data;
using Infrastructure.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Restaurante_Proyecto.Middlewares;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configurar EF Core con SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//INJECTIONS
//builder Dish
builder.Services.AddScoped<IDishCommand, DishCommand>();
builder.Services.AddScoped<IDishQuery, DishQuery>();
builder.Services.AddScoped<ISearchAsync, SearchAsyncDishesService>();
builder.Services.AddScoped<IUpdateDish, UpdateDishService>();
builder.Services.AddScoped<ICreateDish, CreateDishService>();
//builder Query
builder.Services.AddScoped<ICategoryQuery, CategoryQuery>();
builder.Services.AddScoped<ICategoryCommand, CategoryCommand>();
builder.Services.AddScoped<ICategoryExist, CategoryExists>();




builder.Services.AddControllers();
//Validation with FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<DishRequestValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Restaurante API",
        Version = "1.0",
        Description = "API para la gestión de platos en un restaurante",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Restaurant API Support",
            
        }
    });
   c.MapType<OrderPrice>(() => new Microsoft.OpenApi.Models.OpenApiSchema
    {
        Type = "string",
        Enum = new List <Microsoft.OpenApi.Any.IOpenApiAny>
        {
            new Microsoft.OpenApi.Any.OpenApiString("ASC"),
            new Microsoft.OpenApi.Any.OpenApiString("DESC")
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware custom for exception handling
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();