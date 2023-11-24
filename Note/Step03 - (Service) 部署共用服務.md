- 可以不另外將URL部署為共用服務
- 另外部署有利於日後編輯
- 參考: [[Note 註冊自訂服務流程]]

# 1 建立Model類別: ServiceURL
## ServiceURL.cs (Service)
```cs
namespace Sample04_Service.Models
{
    public class ServiceURL
    {
        public String youbikeURL { set; get; }
    }
}
```
## appsetting.json
```js
    "Services": {
        "youbikeURL": "https://tcgbusfs.blob.core.windows.net/dotapp/youbike/v2/youbike_immediate.json"
    }
```


# 2 註冊服務
## Program.cs (Service)
這段程式碼的功能是將自訂的 `ServiceURL` 物件註冊到 DI（Dependency Injection）容器中，以便在應用程式的其他部分中使用。以下是這段程式碼的詳細說明：

1. 建立一個 `ServiceURL` 物件。
2. 使用 `ConfigurationManager` 管理員物件來管理 appsettings.json 檔案。
3. 讀取自訂 Section（作為 JSON 屬性）。
4. 使用 `Bind` 方法將 JSON 屬性的值直接綁定到 `ServiceURL` 物件的屬性上。
5. 將 `ServiceURL` 物件註冊到 Dependency Injection (DI) 容器中，以便在應用程式的其他部分中使用。

```cs
//= = = 註冊服務: ServiceURL (自訂Model)= = = 
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
//= = = = =
```


# 3 測試控制器
## YoubikeController
- using Model資料夾
```cs
using Sample04_Service.Models;
```
- 建構子注入ServiceURL
```cs
//Data Feild
private readonly IHttpClientFactory _httpClientFactory;
private readonly ServiceURL _serviceURL;
//Constructor Injection
public YoubikeController (IHttpClientFactory httpClientFactory,ServiceURL serviceURL)
{
	_httpClientFactory = httpClientFactory;
	_serviceURL = serviceURL;
}
```
- 將uri位置改為 serviceURL
```CS
httpClient.BaseAddress = new Uri(_serviceURL.youbikeURL);
```

```CS
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sample04_Service.Models;


namespace Sample04_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YoubikeController : ControllerBase
    {
        //Data Feild
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ServiceURL _serviceURL;
        //Constructor Injection
        public YoubikeController (IHttpClientFactory httpClientFactory,ServiceURL serviceURL)
        {
            _httpClientFactory = httpClientFactory;
            _serviceURL = serviceURL;
        }

        //
        [HttpGetAttribute] //HTTP GET請求。
        [RouteAttribute("qyr/{sarea}/rawdata")] //路徑
        public IActionResult youbikeQry([FromRouteAttribute(Name = "sarea")] String sarea)
        {
            //建立 HttpClient 物件
            HttpClient httpClient = _httpClientFactory.CreateClient();
            //設定 HttpClient 的基本位址
            httpClient.BaseAddress = new Uri(_serviceURL.youbikeURL);
            //獲取JSON資料 (GetFromJsonAsync)並反序列化
            List<YoubikeData> YoubikeData = httpClient.GetFromJsonAsync <List< YoubikeData>> ("").GetAwaiter().GetResult();
            //回傳Http回應
            return Ok(YoubikeData);
        }
    }
}

```
![image](https://github.com/Riley-Shu/WebForSearchingYoubike/blob/master/Note/image/03_3_1.png)
