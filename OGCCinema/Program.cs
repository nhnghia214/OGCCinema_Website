using Microsoft.EntityFrameworkCore;
using OGCCinema.Data;
using OGCCinema.Models;
using OGCCinema.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Thêm dịch vụ DI
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<PayOSService>(); // Đăng ký PayOSService
// Đảm bảo IConfiguration được thêm vào DI
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

// Program.cs (hoặc Startup.cs)
builder.Services.AddHttpClient();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Cấu hình authentication
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Home/DangNhap"; // Trỏ đến trang đăng nhập của bạn
        options.LogoutPath = "/Home/DangXuat"; // Trỏ đến trang đăng xuất (nếu có)
        options.AccessDeniedPath = "/Home/AccessDenied"; // Trang khi bị từ chối
    });

builder.Services.AddAuthorization();

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<OgccinemaContext>((provider, options) => {
    IConfiguration config = provider.GetRequiredService<IConfiguration>();
    string connectionString = config.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

// Trong ConfigureServices
builder.Services.AddHttpClient("PayOS", client =>
{
    client.BaseAddress = new Uri("https://api-merchant.payos.vn/");
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("User-Agent", "YourApp/1.0");
});

builder.Services.AddDistributedMemoryCache();



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

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=TrangChu}/{id?}");

app.Run();

