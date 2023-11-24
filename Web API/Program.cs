using Microsoft.AspNetCore.Cors.Infrastructure;
using Sample04_Service.Models; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//註冊服務:IHttpClientFactory 
builder.Services.AddHttpClient();

//--------- 註冊服務: ServiceURL (自訂Model) --------- 
//建立ServiceURL物件
ServiceURL serviceURL = new ServiceURL();
//建立ConfigurationManager物件，管理appsetting.json
ConfigurationManager configurationManager = builder.Configuration;
//讀取自訂Section (from appsetting.json)
IConfigurationSection configurationSection = configurationManager.GetSection("Services");
//將自訂Section bind到serviceURL物件
configurationSection.Bind(serviceURL);
//註冊ServiceURL服務 (使用 AddSingleton: Dependency Injection)
builder.Services.AddSingleton(serviceURL);
//-----------------------------------

//--------- 配置CORS policy ---------
builder.Services.AddCors(
    (corsOptions) =>
    {
        corsOptions.AddPolicy("allweb", //全部開放
            (builder) =>
            {
                builder.AllowAnyOrigin(); 
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            }
    );
        corsOptions.AddPolicy("mvcweb", //特定開放
            (builder) =>
            {
                builder.WithOrigins("https://localhost:7259");
                builder.WithHeaders("https://localhost:7259");
                builder.WithMethods("https://localhost:7259");
            });
    }
    );
//-----------------------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//---------配置CORS policy----------
app.UseCors("allweb");
//-----------------------------------


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
