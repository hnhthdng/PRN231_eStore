using BusinessObject.Data;
using BusinessObject.Models;
using DataAccess.DAO;
using DataAccess.Data;
using DataAccess.Mapper;
using DataAccess.Repositories;
using DataAccess.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<StoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<CategoryDAO>();
builder.Services.AddScoped<ProductDAO>();
builder.Services.AddScoped<OrdersDAO>();
builder.Services.AddScoped<OrdersDetailDAO>();
builder.Services.AddScoped<MemberDAO>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();

builder.Services.AddAutoMapper(typeof(MapperProfile));
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<StoreDbContext>();
    SeedData.Initialize(services, context); 
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
