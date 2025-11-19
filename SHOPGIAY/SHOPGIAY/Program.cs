using Microsoft.EntityFrameworkCore;
using SHOPGIAY.Data;

var builder = WebApplication.CreateBuilder(args);

// ĐĂNG KÝ DbContext vào DI container
builder.Services.AddDbContext<ShoeShopContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ShoeShopConnection")
    ));

// Đăng ký MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
