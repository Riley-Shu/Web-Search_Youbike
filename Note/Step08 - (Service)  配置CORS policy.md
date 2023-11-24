- 由於Web和Service設置在不同專案 (網域)中，違反 相同來源原則。故需要啟用跨原始來源要求 (CORS)
- 參考: [在 ASP.NET Core 中啟用跨原始來源要求 (CORS) | Microsoft Learn](https://learn.microsoft.com/zh-tw/aspnet/core/security/cors?view=aspnetcore-7.0)

## Program.cs (Service)
- 加入所有允許 allweb
- 加入特定允許 mvcweb
```cs
//--------- 配置CORS policy ---------
builder.Services.AddCors(
    (corsOptions) =>
    {
        corsOptions.AddPolicy("allweb", //全部開放
            (builder) =>
            {
                builder.AllowAnyOrigin(); 
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            }
    );
        corsOptions.AddPolicy("mvcweb", //特定開放
            (builder) =>
            {
                builder.WithOrigins("https://localhost:7259");
                builder.WithHeaders("https://localhost:7259");
                builder.WithMethods("https://localhost:7259");
            });
    }
    );
//-----------------------------------
```

```cs
//---------配置CORS policy----------
app.UseCors("allweb");
//-----------------------------------
```

## YoubikeController.cs (Service)
```cs
[DisableCors] //關閉Cors
[EnableCors("mvcweb")] //開啟特定Cors
public IActionResult youbikeQry([FromRouteAttribute(Name = "sarea")] String sarea)
```