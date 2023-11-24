- `axios.js` 是一個基於 Promise 的 HTTP 客戶端庫，用於在瀏覽器和 Node.js 中發送和處理 HTTP 請求和回應。它支持多種請求類型，例如 GET、POST、PUT 等，還支持設置請求頭、攔截請求和回應等操作，是前端發起異步請求的主要工具之一。
# 1 加入axios
- [Axios (axios-http.com)](https://axios-http.com/)
- [GitHub - axios/axios: Promise based HTTP client for the browser and node.js](https://github.com/axios/axios)
- [axios/dist at v1.x · axios/axios · GitHub](https://github.com/axios/axios/tree/v1.x/dist)
## Layout.cshtml
方法一: 加入語法
```html
<script src="https://cdn.jsdelivr.net/npm/axios/dist/axios.min.js"></script>
```
方法二: download axios.js + 加入語法
![[07_1_1.png]]
```cs
<script src="~/js/axios.js"></script>
<script src="~/js/axios.min.js"></script>
```

# 2 利用axios進行HTTP GET請求
## qryByArea.cshtml
- 參考: [[axios方法-axios.get()]]
- 注意不要忘記; 
```cs
methods:
{
	qryHandler: function (e) 
	{
		console.log('qryHandler');
		console.log(this.sarea);
		console.log(this.youbikeAPI);
		let reURL = app.youbikeAPI.replace('{0}', this.sarea);
		console.log(reURL);
		console.log('qryHandler');
		axios.get(reURL)
			.then(
				(r) => {
					consol.log("ok");
					console.log(r);
				}
			)
			.catch
			(
				(r) => {
					console.log("error");
					console.log(r);
				}
			); //不要忘記這個;
	}
},
```

