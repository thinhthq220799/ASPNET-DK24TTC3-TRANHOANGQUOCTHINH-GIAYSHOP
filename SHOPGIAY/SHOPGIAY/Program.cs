using Microsoft.EntityFrameworkCore;
using SHOPGIAY.Data;

var builder = WebApplication.CreateBuilder(args);

// ================== CẤU HÌNH DỊCH VỤ (DI) ==================

// DbContext kết nối SQL Server
builder.Services.AddDbContext<ShoeShopContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ShoeShopConnection")
    ));

// MVC: Controllers + Views
builder.Services.AddControllersWithViews();

// Session: dùng cho giỏ hàng, login, v.v.
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // thời gian sống của session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ================== BUILD APP ==================
var app = builder.Build();

// ================== PIPELINE HTTP ==================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// cho phép load /css, /js, /images, ...
app.UseStaticFiles();

app.UseRouting();

// bật session (phải nằm sau UseRouting, trước MVC endpoints)
app.UseSession();

app.UseAuthorization();

// route mặc định: Home/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
