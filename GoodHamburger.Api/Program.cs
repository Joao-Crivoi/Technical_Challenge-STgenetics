using Microsoft.EntityFrameworkCore;
using GoodHamburger.Api.Web.Middlewares;
using GoodHamburger.Api.Infrastructure.Data;
using GoodHamburger.Api.Application.Services;
using GoodHamburger.Api.Application.Interfaces;
using GoodHamburger.Api.Infrastructure.Data.Helpers;
using GoodHamburger.Api.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);

#region Services Configuration

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var strategyType = typeof(IDiscountStrategy);
var strategyImplementations = strategyType.Assembly.GetTypes()
    .Where(t => strategyType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

foreach (var type in strategyImplementations)
{
    builder.Services.AddScoped(typeof(IDiscountStrategy), type);
}


builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();


builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductService, ProductService>();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
#endregion

var app = builder.Build();

#region Pipeline Configuration

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    dbContext.Database.EnsureCreated(); 
    
    DbInitializer.Seed(dbContext);
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers(); 

app.Run();

#endregion