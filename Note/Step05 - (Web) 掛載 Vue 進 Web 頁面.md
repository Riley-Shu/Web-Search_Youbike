# 1 掛載Vue.js
- 下載Vue-min.js，並放入js資料夾中 [Installation — Vue.js (vuejs.org)](https://v2.vuejs.org/v2/guide/installation)
	- `Vue.js`是一個JavaScript框架，它是一個以視圖層為基礎發展的漸進式框架。在Vue.js中，`Vue`是一個JavaScript對象，它是Vue.js應用程序的實例。您可以使用`Vue`對象來創建和管理Vue.js應用程序的各個方面，例如數據、方法、生命週期鉤子等。因此，您可以將`Vue`視為Vue.js應用程序的核心對象，它是Vue.js應用程序的入口點。[1](https://book.vue.tw/CH1/1-1-introduction.html)[2](https://book.vue.tw/CH1/1-2-instance.html)

- 套用進Layout.cshtml 
## layout.cshtml
- 公版套用Vue (放在head)
```html
<script src="~/js/vue.min.js"></script>
```
# 2 規劃前端頁面
## 2-1 掛載Vue
- 建立Vue
- 掛入fieldset
- 測試
- 參考: [[Vue-el屬性 vs $mount方法]]
### qryByArea.cshtml
```html
@{
    ViewData["Title"] = "YouBike臺北市公共自行車即時資訊";
}
<fieldset id="app">
    <h5>YouBike臺北市公共自行車即時資訊</h5>
    <div>
        <label>行政區域</label>
    </div>
	<div>{{100/5}}</div>
</fieldset>

<script>
    var app = new Vue();
    app.$mount('#app');
</script>
```
## 2-2 (Vue) 加入Data及渲染
### qryByArea.cshtml
- script 加入 Data (參考:[[Vue物件-Data]])
- Fieldset 加入 v-model (雙向綁定) 渲染 input
```html
@{
    ViewData["Title"] = "YouBike臺北市公共自行車即時資訊";
}
<fieldset id="app">
    <legend>YouBike臺北市公共自行車即時資訊</legend>
    <div>
        <lable>行政區域</lable>
        <input type="text" v-model:value="sarea"/>
        <button>查詢</button>
    </div>
    @* <div>{{100/5}}</div> *@
</fieldset>

<script>
    var app = new Vue
    (
        {
            data: 
            {
                sarea:"大安區",
            }
        }
    );
    app.$mount('#app');
</script>
```

## 2-3  (Vue) 加入mehods及渲染
### qryByArea.cshtml
- script 加入 methods (注意: 要s，不然會掛掉) (參考:[[Vue物件-methods]])
- Fieldset 加入 v-on:click 渲染 button
```html
<fieldset id="app">
(略)
        <button v-on:click = "qryHandler">查詢</button>
(略)
</fieldset>

<script>
    var app = new Vue
    (
        {
            data: 
            {
                sarea:"大安區",
            },
            methods:
            {
                qryHandler: function (e) 
                {
                    console.log(e)
                }
            }
        }
    );
    app.$mount('#app');
</script>
```
![image](https://github.com/Riley-Shu/WebForSearchingYoubike/blob/master/Note/image/05_2-3_1.png)

# 3 確認連接正確
## qryByArea.cshtml
- 直接給API網址，確認其能抓到
- 小心錯字
- 注意:  youbikeAPI 命名須與 var youbikeService 相同 ，一邊是API 一邊是service
```html
<script>
    var app = new Vue
    (
        {
            data: 
            {
                sarea:"大安區",
                youbikeAPI: "https://localhost:7078/api/Youbike/qyr/{0}/rawdata",
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
```

![image](https://github.com/Riley-Shu/WebForSearchingYoubike/blob/master/Note/image/05_3_1.png)
