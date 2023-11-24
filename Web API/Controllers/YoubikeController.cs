using Microsoft.AspNetCore.Cors;
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
        public YoubikeController(IHttpClientFactory httpClientFactory, ServiceURL serviceURL)
        {
            _httpClientFactory = httpClientFactory;
            _serviceURL = serviceURL;
        }

        [HttpGetAttribute] //HTTP GET請求。
        [RouteAttribute("qyr/{sarea}/rawdata")] //路徑
        [ProducesAttribute("application/json")] //產生JSON格式的回應
        [ProducesResponseType(typeof(List<YoubikeData>),200)] //指定成功回應的類型和HTTP狀態碼
        [ProducesResponseType(typeof(Message),400)] //指定錯誤回應的類型和HTTP狀態碼
        [DisableCors] //關閉Cors
        [EnableCors("mvcweb")] //開啟特定Cors
        public IActionResult youbikeQry([FromRouteAttribute(Name = "sarea")] String sarea) //使用FromRouteAttribute指定該參數的值應該從路由中提取
        {
            //Part1: 獲取資料
            //建立 HttpClient 物件
            HttpClient httpClient = _httpClientFactory.CreateClient();
            //設定 HttpClient 的基本位址
            httpClient.BaseAddress = new Uri(_serviceURL.youbikeURL);
            Console.WriteLine($"_serviceURL.youbikeURL:{_serviceURL.youbikeURL}");
            //獲取JSON資料 (GetFromJsonAsync)並反序列化
            List<YoubikeData> YoubikeData = httpClient.GetFromJsonAsync <List< YoubikeData>> ("").GetAwaiter().GetResult();

            //Part2: 判斷並回傳Http回應
            //LINQ搜尋比對
            var result = (from r in YoubikeData where r.sarea == sarea select r).ToList();
            if (result.Count > 0)
            {
                return this.Ok(result);
            }
            else
            {
                Message msg = new Message()
                {
                    code = 400,
                    message = $"{sarea} 查無任何資料。"
                };
                return this.StatusCode(400, msg);
            }
        }
    }
}
