- 參考: 
	  [[CS-Interface-IHttpClientFactory]]
- 遇到的問題: 
	  [[Err-System.InvalidOperationException Unable to resolve service 'System.Net.Http.IHttpClientFactory']]
# 1 註冊IHttpClientFactory服務
## Program.cs (Service)
- 初始
```cs
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

```
- 註冊IHttpClientFactory服務
```CS
builder.Services.AddHttpClient();
```

# 2 測試IActionResult
## 2-1 加入Model類別: YoubikeData
- 從 [YouBike臺北市公共自行車即時資訊介接網址](https://tcgbusfs.blob.core.windows.net/dotapp/youbike/v2/youbike_immediate.json) 複製JSON格式
![image](https://github.com/Riley-Shu/WebForSearchingYoubike/blob/master/Note/image/02_2-1_1.png)
- 選擇性貼上 JSON作為類別
![image](https://github.com/Riley-Shu/WebForSearchingYoubike/blob/master/Note/image/02_2-1_2.png)
### YoubikeData.cs (Service)
```cs
namespace Sample04_Service.Models
{
    public class YoubikeData
    {
            public string sno { get; set; }
            public string sna { get; set; }
            public int tot { get; set; }
            public int sbi { get; set; }
            public string sarea { get; set; }
            public string mday { get; set; }
            public float lat { get; set; }
            public float lng { get; set; }
            public string ar { get; set; }
            public string sareaen { get; set; }
            public string snaen { get; set; }
            public string aren { get; set; }
            public int bemp { get; set; }
            public string act { get; set; }
            public string srcUpdateTime { get; set; }
            public string updateTime { get; set; }
            public string infoTime { get; set; }
            public string infoDate { get; set; }
    }
}
```

### YoubikeController.cs (Service)
- 設定為回傳介面
	這段程式碼是一個 ASP.NET Core Web API 控制器動作，它使用 `HttpClient` 類別來發送 HTTP GET 請求，以獲取 YouBike 的資料。以下是這段程式碼的詳細說明：
	1. 建立一個 `HttpClient` 實例。
	2. 設定 `HttpClient` 的基本位址，以便發送 HTTP GET 請求。
	3. 使用 `GetFromJsonAsync` 方法從 Web API 中獲取 JSON 資料，並將其反序列化為 `List<UbikeData>` 物件。
	4. 使用 `Ok` 方法將 `List<UbikeData>` 物件封裝到 `IActionResult` 物件中，並將其作為 HTTP 回應傳回。
- 
```cs
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Sample04_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UbikeController : ControllerBase
    {
        //Data Feild
        private readonly IHttpClientFactory _httpClientFactory;
        //cotr injection
        public UbikeController (IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGetAttribute] //HTTP GET請求。
        [RouteAttribute("qyr/{sarea}/rawdata")] //路徑
        public IActionResult ubikeQry([FromRouteAttribute(Name = "sarea"))
        {
            //建立 HttpClient 物件
            HttpClient httpClient = _httpClientFactory.CreateClient();
            //設定 HttpClient 的基本位址
            httpClient.BaseAddress = new Uri("https://tcgbusfs.blob.core.windows.net/dotapp/youbike/v2/youbike_immediate.json");
            //獲取JSON資料 (GetFromJsonAsync)並反序列化
            List<YoubikeData> YoubikeData = httpClient.GetFromJsonAsync <List< YoubikeData>> ("").GetAwaiter().GetResult();
            //回傳Http回應
            return Ok(YoubikeData);
        }
    }
}

```

![image](https://github.com/Riley-Shu/WebForSearchingYoubike/blob/master/Note/image/02_2-1_3.png)

