using CrudStudentProject.Hubs;
using CrudStudentProject.Hubs;
using CrudStudentProject.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDistributedMemoryCache();

// Add session services with desired options
builder.Services.AddSession(options =>
{
    // Set session timeout (e.g., 30 minutes)
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true; // Makes session cookie accessible only via HTTP
    options.Cookie.IsEssential = true; // Ensures session cookie is created even if tracking consent is not given
});

// Add HttpContextAccessor for accessing the session in controllers/services
builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();
var app = builder.Build();

// Add SignalR services

// Map SignalR hub
app.MapHub<StudentHub>("/studentHub");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=LOgin}/{id?}");

app.Run();
