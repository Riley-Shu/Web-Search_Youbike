using Microsoft.AspNetCore.Cors.Infrastructure;
using Sample04_Web.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//--------- 註冊自訂服務: ServiceEntity ---------
//建立ServiceEntity物件
ServiceEntity serviceEntity = new ServiceEntity();
//建立管理物件
ConfigurationManager configurationManager = builder.Configuration;
//取得Section
IConfigurationSection section = configurationManager.GetSection("ServiceURL");
//綁定
section.Bind(serviceEntity);
//加入服務
builder.Services.AddSingleton(serviceEntity);
//-----------------------------------

//--------- 註冊自訂服務: Sarea ---------
Sarea sarea = new Sarea();
builder.Services.AddSingleton(sarea);
//------------------


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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
