using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TheStory.Data;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
    options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
        options.SlidingExpiration = true;
        options.Cookie.Name = "TheStory";
        options.LoginPath = new PathString("/");
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdministratorOnly", policy => policy.RequireRole("Administrator"));
});
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseSqlServer(connectionString: builder.Configuration.GetConnectionString("DefaultConnection"));
});


var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    // Production's config
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();