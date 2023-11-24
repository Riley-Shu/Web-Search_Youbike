# Message.cs (Service)

```cs
namespace Sample04_Service.Models
{
    public class Message
    {
        public Int32 code { get; set; }
        public String message { get; set; }
    }
}
```
# YoubikeController.cs (Service)
- LINQ 搜尋語法 (參考: [[LINQ 查詢語法]])
```cs
var result = (from r in YoubikeData where r.sarea == sarea select r).ToList();
```

```cs
[HttpGetAttribute] //HTTP GET請求。
[RouteAttribute("qyr/{sarea}/rawdata")] //路徑
[ProducesAttribute("application/json")] //產生JSON格式的回應
[ProducesResponseType(typeof(List<YoubikeData>),200)] //指定成功回應的類型和HTTP狀態碼
[ProducesResponseType(typeof(Message),400)] //指定錯誤回應的類型和HTTP狀態碼
[DisableCors] //關閉Cors
[EnableCors("mvcweb")] //開啟特定Cors

public IActionResult youbikeQry([FromRouteAttribute(Name = "sarea")] String sarea)
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
			message = $"{sarea}區域查無任何資料"
		};
		return this.StatusCode(400, msg);
	}
}
```

- 比較: 
	- 原本: 沒有LINQ，就算搜尋其他區域也會出現所有資料
![image](https://github.com/Riley-Shu/WebForSearchingYoubike/blob/master/Note/image/09_1_1.png)
	- 修改: 加入LINQ和Message
![image](https://github.com/Riley-Shu/WebForSearchingYoubike/blob/master/Note/image/09_1_2.png)
![image](https://github.com/Riley-Shu/WebForSearchingYoubike/blob/master/Note/image/09_1_3.png)
