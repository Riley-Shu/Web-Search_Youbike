using Microsoft.AspNetCore.Cors.Infrastructure;
using Sample04_Web.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//--------- ���U�ۭq�A��: ServiceEntity ---------
//�إ�ServiceEntity����
ServiceEntity serviceEntity = new ServiceEntity();
//�إߺ޲z����
ConfigurationManager configurationManager = builder.Configuration;
//���oSection
IConfigurationSection section = configurationManager.GetSection("ServiceURL");
//�j�w
section.Bind(serviceEntity);
//�[�J�A��
builder.Services.AddSingleton(serviceEntity);
//-----------------------------------

//--------- ���U�ۭq�A��: Sarea ---------
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
