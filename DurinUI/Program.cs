using DurinUI.Services;
using Microsoft.AspNetCore.Server.IISIntegration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(IISDefaults.AuthenticationScheme)
    .AddCookie("Admin", config => { config.LoginPath = "/Login/Index"; config.AccessDeniedPath = "/Login/AccessDenied"; })
	.AddCookie("User", config => { config.LoginPath = "/Login/Index"; config.AccessDeniedPath = "/Login/AccessDenied"; });
builder.Services.AddSession(option => { option.IdleTimeout = TimeSpan.FromDays(1); });
builder.Services.AddControllersWithViews().AddNewtonsoftJson(); 
builder.Services.AddHostedService<HostedService>();

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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);
app.MapControllerRoute(
	name: "areas",
	pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);
app.Run();
