using Microsoft.AspNetCore.Cors.Infrastructure;
using Sample04_Service.Models; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//���U�A��:IHttpClientFactory 
builder.Services.AddHttpClient();

//--------- ���U�A��: ServiceURL (�ۭqModel) --------- 
//�إ�ServiceURL����
ServiceURL serviceURL = new ServiceURL();
//�إ�ConfigurationManager����A�޲zappsetting.json
ConfigurationManager configurationManager = builder.Configuration;
//Ū���ۭqSection (from appsetting.json)
IConfigurationSection configurationSection = configurationManager.GetSection("Services");
//�N�ۭqSection bind��serviceURL����
configurationSection.Bind(serviceURL);
//���UServiceURL�A�� (�ϥ� AddSingleton: Dependency Injection)
builder.Services.AddSingleton(serviceURL);
//-----------------------------------

//--------- �t�mCORS policy ---------
builder.Services.AddCors(
    (corsOptions) =>
    {
        corsOptions.AddPolicy("allweb", //�����}��
            (builder) =>
            {
                builder.AllowAnyOrigin(); 
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            }
    );
        corsOptions.AddPolicy("mvcweb", //�S�w�}��
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


//---------�t�mCORS policy----------
app.UseCors("allweb");
//-----------------------------------


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
