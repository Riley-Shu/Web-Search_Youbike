- 可以不另外將URL部署為共用服務
- 另外部署有利於日後編輯
- - 參考: [[Note 註冊自訂服務流程]]
- 注意錯誤: [[Err-System.InvalidOperationException Unable to resolve service 'System.Net.Http.IHttpClientFactory']]
# 1 建立類別
## ServiceEntity.cs (Web)
```CS
namespace Sample04_Web.Models
{
    public class ServiceEntity
    {
        public String youbikeService { set; get; }
    }
}
```
## appsetting.json (Web)
```json
    "ServiceURL": {
        "youbikeService": "https://localhost:7078/api/Youbike/qyr/{0}/rawdata",
    }
```
# 2 註冊服務
## Program.cs (Web)
- 初始
```cs
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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

```
- 註冊自訂服務 (同 ServiceURL 服務)
```cs
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
```

# 3 測試控制器
## YoubikeController.cs (Web)
- using Model資料夾
- 建構子注入
- 綁定ViewData  (參考: [[Vue物件-ViewData vs ViewBag]])
```cs
using Microsoft.AspNetCore.Mvc;
using Sample04_Web.Models;

namespace Sample04_Web.Controllers
{
    public class YoubikeController : Controller
    {
        //Data Field
        private ServiceEntity _serviceEntity;
        //Ctor injection
        public YoubikeController(ServiceEntity serviceEntity)
        {
            _serviceEntity = serviceEntity;
        }

        //頁面
        public IActionResult qryByArea()
        {
            String url = this._serviceEntity.youbikeService;
            ViewData["url"] = url;
            return View();
        }
    }
}
```
## qryByArea.cshtml
- 加入 ViewData "youbikeServiceUrl"]
- 加入`@Html.Raw()` (參考:  [[Razor語法-Html.Raw]])
	- `url`是一個字串變數，它包含了一個HTML原始碼。`@Html.Raw(url)`將這個HTML原始碼輸出到視圖中，並將其賦值給名為`youbikeService`的變數。
- 加入Data屬性:  `youbikeAPI: ""`
- 使用 mounted 方法 (參考: [[Vue方法-mounted]])
	- 將Javascript global variable (youbikeService) 指派給 Vue Data Model (youbikeAPI)
	- Vue實例就可以使用youbikeAPI數據屬性來訪問全局變數youbikeService。
```cs
@{
    ViewData["Title"] = "YouBike臺北市公共自行車即時資訊";
    String url = ViewData["youbikeServiceUrl"] as String;
}
<script>
    var youbikeService = '@Html.Raw(url)';
</script>
```

```cs
<script>
    var app = new Vue
    (
        {
            data: 
            {
                sarea:"",
                youbikeAPI: "",
            },
            methods:
            {
                qryHandler: function (e) 
                {
                    console.log('qryHandler');
                    console.log(this.sarea);
                    console.log(this.youbikeAPI);
                    let reURL = app.youbikeAPI.replace('{0}', this.sarea);
                    console.log(reURL);
                }
            },
            mounted: function () {
                this.youbikeAPI = youbikeService;
            }
        }
    );
    app.$mount('#app');
</script>

```

# Discussion
## 關於指派全域變數
全域變數 (youbikeService) 指派給 Vue Data Model (youbikeAPI)，我考慮了兩種寫法：
### 方式一
```cs
// 假設youbikeService是全域變數
var app = new Vue({
  el: '#app',
  data: {
    youbikeAPI: youbikeService
  }
})
```
- 在Vue實例創建時直接將全域變數youbikeService指派給Vue Data Model(youbikeAPI)。
- 優點：簡單明了，易於理解和維護。
- 缺點：當全域變數youbikeService發生變化時，Vue Data Model(youbikeAPI)不會自動更新。
### 方式二
```cs
var app = new Vue({
  el: '#app',
  data: {
    youbikeAPI: ""
  },
 //使用mounted方法
            mounted: function () {
                //將Javascript global變數 (youbikeService) 指派給 Vue Data Model (youbikeAPI)
                this.youbikeAPI = youbikeService;
            }
})
```

- 在Vue實例創建後使用mounted方法將全域變數youbikeService指派給Vue Data Model(youbikeAPI)。
- 優點：當全域變數youbikeService發生變化時，Vue Data Model(youbikeAPI)會自動更新。
- 缺點：程式碼稍微複雜一些。
