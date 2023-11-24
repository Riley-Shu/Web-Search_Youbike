#我有問題 
在API controller中， ｢HttpGet ］測試時，類別為String可以正確回傳，類別為IActionResult卻回傳錯誤代碼500
# Condition
## YoubikeController.cs (Service)
```cs
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sample04_Service.Models;


namespace Sample04_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UbikeController : ControllerBase
    {
        //Data Feild
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ServiceURL _serviceURL;
        //cotr injection
        public UbikeController (IHttpClientFactory httpClientFactory,ServiceURL serviceURL)
        {
            _httpClientFactory = httpClientFactory;
            _serviceURL = serviceURL;
        }

        [HttpGetAttribute] //HTTP GET請求。
        [RouteAttribute("qyr/{sarea}/rawdata")] //路徑
        public IActionResult ubikeQry([FromRouteAttribute(Name = "sarea")] String sarea)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(_serviceURL.ubikeURL);
            List<UbikeData> ubikeData = httpClient.GetFromJsonAsync <List< UbikeData>> ("").GetAwaiter().GetResult();
            return Ok(ubikeData);
        }
    }
}
```
# Solution 
#我有答案 [[Step02 - (Service) 部署 IHttpClientFactory]]
#我有答案 [[Step03 - (Service) 部署共用服務]]]

----
# Error: Unable to resolve service for type 'System.Net.Http.IHttpClientFactory' 
## AI response

這個錯誤通常表示您的應用程式中缺少 `IHttpClientFactory` 服務的註冊。`IHttpClientFactory` 是一個介面，它提供了一個方法，可以創建和配置 `HttpClient` 實例。在 ASP.NET Core Web API 中，`HttpClient` 是一個常用的類別，用於發送 HTTP 請求。如果您的控制器需要使用 `HttpClient`，則必須將 `IHttpClientFactory` 服務註冊到 DI 容器中。您可以在 `Startup.cs` 檔案中的 `ConfigureServices` 方法中註冊 `IHttpClientFactory` 服務，如下所示：

```csharp
services.AddHttpClient();
```

如果您需要更多的控制，例如配置 `HttpClient` 的超時時間、設置預設的請求標頭等等，您可以使用 `HttpClientFactory` 來創建和配置 `HttpClient` 實例。以下是一個示例：

```csharp
services.AddHttpClient("myclient", client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("User-Agent", "My HttpClient");
});
```

## 解決方法
### Program.cs (Service)
```CS
//加入啟動Http Client Service
builder.Services.AddHttpClient();
```

----
# Error: Unable to resolve service for type 'Sample04_Service.Models.ServiceURL'
## AI response
這個錯誤通常表示您的應用程式中缺少 `ServiceURL` 服務的註冊。為了解決這個問題，您需要在 `Startup.cs` 檔案中的 `ConfigureServices` 方法中註冊 `ServiceURL` 服務。以下是一個示例：

```csharp
services.AddSingleton<ServiceURL>();
```

如果您需要更多的控制，例如配置 `ServiceURL` 的屬性、設置預設值等等，您可以使用其他生命週期方法來註冊 `ServiceURL` 服務。以下是一些相關的生命週期方法：

- `AddTransient`：每次請求都創建一個新的實例。
- `AddScoped`：每個請求創建一個新的實例。
- `AddSingleton`：應用程式生命週期中只創建一個實例。

希望這可以幫助您解決問題！
## 解決方法 (錯誤)
### Program.cs (Service)
```cs
builder.Services.AddSingleton<ServiceURL>();
```
# System.ArgumentNullException: Value cannot be null. (Parameter 'uriString')
## AI response
這個錯誤通常表示您的程式碼中傳遞了一個空的 `uriString` 參數。請檢查您的程式碼，確保您傳遞的參數不為空。如果您需要更多的幫助，請提供更多的程式碼細節，以便我們更好地理解問題。

希望這可以幫助您解決問題！

## 解決方法
### Program.cs (Service)
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



