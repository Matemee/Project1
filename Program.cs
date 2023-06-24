using Microsoft.EntityFrameworkCore;
using OnlineBookStoreAPI.Data;
using OnlineBookStoreAPI.Models;
using OnlineBookStoreAPI.Repositories;
using OnlineBookStoreAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));

builder.Services.AddScoped<IGenericRepository<Book, int>, BooksRepository>();
builder.Services.AddScoped<IGenericRepository<Author, int>, AuthorsRepository>();
builder.Services.AddScoped<IGenericRepository<Category, string>, CategoriesRepository>();
builder.Services.AddScoped<IBooksRepository, BooksRepository>();

var app = builder.Build();

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
