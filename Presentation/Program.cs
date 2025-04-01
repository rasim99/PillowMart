using Business.Services.Abstract;
using Business.Services.Concrete;
using Core.Entities;
using Core.Utilities.EmailHandler.Abstract;
using Core.Utilities.EmailHandler.Models;
using Core.Utilities.File;
using Core.Utilities.Stripe;
using Data;
using Data.Contexts;
using Data.Repositories.Abstract;
using Data.Repositories.Base;
using Data.Repositories.Concrete;
using Data.UnitOfWork;
using Fruitkh.Utilities.EmailHandler.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(opt=>opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddIdentity<User, IdentityRole>(option =>
{
    option.SignIn.RequireConfirmedEmail = true;
    option.User.RequireUniqueEmail = true;
    option.Password.RequiredLength = 8;
    option.Password.RequireDigit = true;
    option.Password.RequireLowercase = true;

}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

var rootPath = builder.Environment.WebRootPath;
builder.Services.AddSingleton(rootPath);

var emailConfiguration = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfiguration);
builder.Services.AddSingleton<IEmailService, EmailService>();

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));


#region Data

builder.Services.AddScoped<ICategoryRepistory, CategoryRepistory>();
builder.Services.AddScoped<IProductRepistory, ProductRepistory>();
builder.Services.AddScoped<IBasketRepistory, BasketRepistory>();
builder.Services.AddScoped<IBasketProductRepository, BasketProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderProductRepository, OrderProductRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


#endregion

#region Services
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, Business.Services.Concrete.ProductService>();
builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IAccountService, Business.Services.Concrete.AccountService>();
builder.Services.AddScoped<IAdminAccountService, AdminAccountService>();
builder.Services.AddScoped<IUserService, UserService>();
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

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
    DbInitalizer.SeedData(roleManager, userManager);
}

app.MapControllerRoute(
  name: "areas",
  pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe")["SecretKey"];

app.Run();
