using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MidTermProject.Models;

var builder = WebApplication.CreateBuilder(args);

// Kết nối database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MidTermDBContext>(options =>
    options.UseSqlServer(connectionString));

// Đăng ký dịch vụ Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<MidTermDBContext>();

// Đăng ký MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Pipeline xử lý HTTP request
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // hiển thị lỗi chi tiết khi dev
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Bắt buộc nếu dùng Identity
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Dùng Razor Pages nếu có Identity UI
app.MapRazorPages();

app.Run();
