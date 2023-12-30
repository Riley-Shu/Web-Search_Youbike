# 1 呈現即時資料
## qryByArea.cshtml
- 修改方法
- Http get請求成功，回傳資料；失敗，則回傳訊息。
```cs
data: 
{
    sarea:"大安區",
    youbikeAPI: "",
    result: [],
    message:'',
},
methods:
{
    qryHandler: function (e) 
    {
        console.log('qryHandler');
        // console.log(this.sarea);
        // console.log(this.youbikeAPI);
        let reURL = app.youbikeAPI.replace('{0}', this.sarea);
        // console.log(reURL);
        axios.get(reURL)
            .then(
                (r) => {
                    console.log(r)
                    app.result = r.data;
                }
            )
            .catch
            (
                (r) => {
                    console.log(r);
                    app.message = r.response.data.message
                }
            );
    }
},
```
- 修改表格
- v-for渲染列表 (參考:[[Vue指令-v-for]])
```html
    <fieldset>
        <div>記錄數:{{result.length}}</div>
        <div>
        <table>
            <thead>
                <tr>
                    <td>區域</td>
                    <td>地點</td>
                    <td>空車數</td>
                    <td>空車位數</td>
                    <td>更新時間</td>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in result">
                    <td>{{item.sarea}}</td>
                    <td>{{item.ar}}</td>
                    <td>{{item.sbi}}</td>
                    <td>{{item.bemp}}</td>
                    <td>{{item.srcUpdateTime}}</td>
                </tr>
            </tbody>
        </table>
        </div>
        <div>{{message}}</div>

    </fieldset>

```
- 美化
```html
<table class="table table-primary table-borderless">
```

# 2 處理問題: 刷新
## qryByArea.cshtml
- 注意: `:` 和 `=`
- 表格增加 `v-show` (參考: [[Vue指令-v-show]])
```html
    <fieldset v-show="isShowT">
```
- data 增加 v-show開關，每次觸發方法時重刷
```cs
<script>
    var app = new Vue
    (
        {
            data: 
            {
                sarea:"",
                youbikeAPI: "",
                result: [],
                message:'',
                isShowT:false,
            },
            methods:
            {
                qryHandler: function (e) 
                {
                    app.result = [];
                    app.message = "";
                    app.isShowT = false;
                    console.log('qryHandler');
                    // console.log(this.sarea);
                    // console.log(this.youbikeAPI);
                    let reURL = app.youbikeAPI.replace('{0}', this.sarea);
                    // console.log(reURL);
                    axios.get(reURL)
                        .then(
                            (r) => {
                                console.log(r)
                                app.result = r.data;
                                app.isShowT= true;
                            }
                        )
                        .catch
                        (
                            (r) => {
                                console.log(r);
                                if (r.response.data == "") 
                                {
                                    app.message = "提醒: 請選擇欲查詢之行政區域";
                                }
                                else 
                                {
                                    app.message = r.response.data.message;
                                }
                                        app.isShowN = true;
                            }
                        );
                }
            },
            mounted: function () {
                //將Javascript global變數指派給 Vue Data Model
                this.youbikeAPI = youbikeService;
            }
        }
    );
    app.$mount('#app');
</script>
```

# 3 處理問題: 輸入方格改為下拉式選單
## 3-1 加入類別並註冊服務
### Sarea.cs (Web)
- 參考: [[Q String［］vs List＜String＞]]
```cs
namespace Sample04_Web.Models
{
    public class Sarea
    {
        public String[] Taipei = new String[]
        //public List<String> Taipei = new List<String>
        {
            "大安區",
            "大同區",
            "士林區",
            "文山區",
            "中正區",
            "中山區",
            "內湖區",
            "北投區",
            "松山區",
            "南港區",
            "信義區",
            "萬華區",
            "臺大公館校區"
        };
    }
}
```

### Program.cs (Web)
```cs
//--------- 註冊自訂服務: Sarea ---------
Sarea sarea = new Sarea();
builder.Services.AddSingleton(sarea);
//------------------
```
## 3-2 加入控制器及頁面
### YoubikeController.cs (Web)
- 建構子注入
- 加入ViewData (參考: [[Vue物件-ViewData vs ViewBag]])
```cs
using Microsoft.AspNetCore.Mvc;
using Sample04_Web.Models;

namespace Sample04_Web.Controllers
{
    public class YoubikeController : Controller
    {
        //Data Field
        private ServiceEntity _serviceEntity;
        private Sarea _sarea;
        //Ctor injection
        public YoubikeController(ServiceEntity serviceEntity, Sarea sarea)
        {
            _serviceEntity = serviceEntity;
            _sarea = sarea;
        }

        //頁面
        public IActionResult qryByArea()
        {
            String[] TaipeiList = this._sarea.Taipei;
			//List<String> TaipeiList = this._sarea.Taipei;
            ViewData["TaipeiList"] = TaipeiList;
            //ViewBag.sareaList = TaipeiList;
            //foreach (String i in TaipeiList)
            //{
            //    Console.WriteLine(i);
            //}

            String url = this._serviceEntity.youbikeService;
            //Console.WriteLine($"url:{url}");
            ViewData["youbikeServiceUrl"] = url;
            return View();
        }
    }
}
```
### qryByArea.cshtml (Web)
- 加入ViewData
```cs
 String[] TaipeiList = ViewData["TaipeiList"] as String[];
```
- 將 `input` 改為 `select`
```html
<div>
	<lable>行政區域</lable>
	@*input type="text" v-model:value="sarea"/> *@
	<select v-model:value="sarea">
		@foreach (var i in TaipeiList)
		{
			<option value="@i" selected>@i</option>
		}
	</select>
	<button v-on:click = "qryHandler">查詢</button>
</div>
```

# 4 處理問題: 修改錯誤訊息
## qryByArea.cshtml (Web)
- 整理 div
```html
    <fieldset v-show="isShowN">
        <div style="background-color:lightpink;">
            <h3>{{message}}</h3>
        </div>
    </fieldset>
```
- 整理Data
```cs
data: 
{
	sarea:"",
	youbikeAPI: "",
	result: [],
	message:'',
	isShowT:false,
	isShowN: false
},
```
- 整理方法
```cs
methods:
{
	qryHandler: function (e) 
	{
		app.result = [];
		app.message = "";
		app.isShowT = false;
		app.isShowN = false;

		console.log('qryHandler');
		// console.log(this.sarea);
		// console.log(this.youbikeAPI);
		let reURL = app.youbikeAPI.replace('{0}', this.sarea);
		// console.log(reURL);
		axios.get(reURL)
			.then(
				(r) => {
					console.log(r)
					app.result = r.data;
					app.isShowT= true;
				}
			)
			.catch
			(
				(r) => {
					console.log(r);
					if (r.response.data == "") 
					{
						app.message = "提醒: 請選擇欲查詢之行政區域";
					}
					else 
					{
						app.message = r.response.data.message;
					}
							app.isShowN = true;
				}
			);
	}
},
```

![image](https://github.com/Riley-Shu/WebForSearchingYoubike/blob/master/Note/image/10_3_1.png)
