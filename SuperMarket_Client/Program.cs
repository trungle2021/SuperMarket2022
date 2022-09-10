using Microsoft.EntityFrameworkCore;
using SuperMarket_DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using SuperMarket_DataAccess.Repository.IRepository;
using SuperMarket_DataAccess.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using SuperMarket_Utility;
using Stripe;
using SuperMarket_Client.Areas.Customer.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("sqlconnect")));
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddSession();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = "242580396308-ti1if1cm1h9pecc0puvdi4og3knuclcf.apps.googleusercontent.com";
    options.ClientSecret = "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";
    options.CallbackPath = "/signin-google";
    options.AuthorizationEndpoint = String.Concat(options.AuthorizationEndpoint, "?prompt=select_account");
});

//add newtonsoftJson
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});
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
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
app.UseAuthentication();;

app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",


    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

    
app.Run();
