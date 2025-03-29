using Business.Services.Abstract;
using Business.Services.Concrete;
using Core.Utilities.File;
using Data.Contexts;
using Data.Repositories.Abstract;
using Data.Repositories.Base;
using Data.Repositories.Concrete;
using Data.UnitOfWork;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(opt=>opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

var rootPath = builder.Environment.WebRootPath;
builder.Services.AddSingleton(rootPath);

#region Data

builder.Services.AddScoped<ICategoryRepistory, CategoryRepistory>();
builder.Services.AddScoped<IProductRepistory, ProductRepistory>();

#endregion

#region Services
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddSingleton<IFileService, Core.Utilities.File.FileService>();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
  name: "areas",
  pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
