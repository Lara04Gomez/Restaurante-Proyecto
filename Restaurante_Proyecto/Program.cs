using Application.Interfaces;
using Application.Interfaces.ICategory;
using Application.Interfaces.ICategory.Repository;
using Application.Interfaces.IDeliveryType;
using Application.Interfaces.IDeliveryType.Repository;
using Application.Interfaces.IDish;
using Application.Interfaces.IDish.Repository;
using Application.Interfaces.IOrder;
using Application.Interfaces.IOrder.Repository;
using Application.Interfaces.IOrderItem.Repository;
using Application.Interfaces.IStatus;
using Application.Interfaces.IStatus.Repository;
using Application.Services;
using Application.Validators;
using Application.Services.CategoryService;
using Application.Services.DeliveryTypeService;
using Application.Services.DishServices;
using Application.Services.OrderService;
using Application.Services.StatusService;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Command;
using Infrastructure.Data;
using Infrastructure.Query;
using Infrastructure.Repositories;
using Restaurante_Proyecto.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);



// Configurar EF Core con SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//INJECTIONS
//builder Dish
builder.Services.AddScoped<IDishCommand, DishCommand>();
builder.Services.AddScoped<IDishQuery, DishQuery>();
builder.Services.AddScoped<IDishRepository, DishRepository>();
builder.Services.AddScoped<ISearchAsyncUseCase, SearchAsyncUseCase>();
builder.Services.AddScoped<IUpdateDishUseCase, UpdateDishUseCase>();
builder.Services.AddScoped<ICreateDishUseCase, CreateDishUseCase>();
builder.Services.AddScoped<IGetDishByIdUseCase, GetDishByIdUseCase>();
builder.Services.AddScoped<IDeleteDishUseCase, DeleteDishUseCase>();

//builder Category
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryQuery, CategoryQuery>();
builder.Services.AddScoped<ICategoryCommand, CategoryCommand>();
builder.Services.AddScoped<ICategoryExistsUseCase, CategoryExistsUseCase>();
builder.Services.AddScoped<IGetAllCategoriesAsyncUseCase, GetAllCategoriesAsyncUseCase>();

//builder DeliveryType
builder.Services.AddScoped<IDeliveryTypeCommand, DeliveryTypeCommand>();
builder.Services.AddScoped<IDeliveryTypeQuery, DeliveryTypeQuery>();
builder.Services.AddScoped<IGetAllDeliveryAsyncUseCase, GetAllDeliveryAsyncUseCase>();

//builder Order
builder.Services.AddScoped<IOrderCommand, OrderCommand>();
builder.Services.AddScoped<IOrderQuery, OrderQuery>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICreateOrderUseCase, CreateOrderUseCase>();
builder.Services.AddScoped<IGetOrderWithFilterUseCase, GetOrderWithFilterUseCase>();
builder.Services.AddScoped<IGetOrderByIdUseCase, GetOrderByIdUseCase>();
builder.Services.AddScoped<IUpdateItemFromOrderUseCase, UpdateItemFromOrderUseCase>();
builder.Services.AddScoped<IUpdateOrderItemStatusUseCase, UpdateOrderItemStatusUseCase>();

//builder Status
builder.Services.AddScoped<IStatusQuery, StatusQuery>();
builder.Services.AddScoped<IGetAllStatusAsyncUseCase,GetAllStatusAsyncUseCase>();

//builder OrderItem
builder.Services.AddScoped<IOrderItemCommand, OrderItemCommand>();
builder.Services.AddScoped<IOrderItemQuery, OrderItemQuery>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();

//Validation with FluentValidation
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<DishRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<DishUpdateRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<OrderRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ItemsValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<DeliveryTypeValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<OrderUpdateValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<StatusRequestValidator>();


builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Restaurant API",
        Version = "1.0",
        Description= "API para la gestión de platos en un restaurante"
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
       
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}


app.UseMiddleware<ErrorHandlingMiddleware>();


//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();