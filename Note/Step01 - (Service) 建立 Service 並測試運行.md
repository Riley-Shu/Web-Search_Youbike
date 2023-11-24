# 1 建立專案: ASP.NET Core Web API
![[01_1_1.png]]
![image](Note/image/01_1_1.png)
# 2 加入API控制器
1. 加入API控制器
2. 設定路徑
3. Swagger、Postman測試
## 2-1 加入API控制器
- 加入 > 控制器 > API控制器
![[01_2-1_1.png]]
## 2-2 設定路徑
### YoubikeController.cs (Service)
- 初始
```cs
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Sample04_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YoubikeController : ControllerBase
    {
    }
}
```
- 設定路徑
- FromRouteAttribute (參考: [[CS-Property-FromRouteAttribute]])
- 使用`FromRouteAttribute`屬性來指定該參數的值應該從路由中提取。應用程序收到一個帶有`sarea`值的請求時，該值將自動傳遞到`youbikeQry`方法的`sarea`參數中。

```cs
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Sample04_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YoubikeController : ControllerBase
    {
        [HttpGetAttribute] //HTTP GET請求。
        [RouteAttribute("qyr/{sarea}/rawdata")] //路徑
        public String ubikeQry([FromRouteAttribute(Name = "sarea")] String sarea)
        {
            return sarea;
        }
    }
}
```
## 2-3 Swagger、Postman測試
![[01_2-3_1.png]]
![[01_2-3_2.png]]
