# 註冊服務
1. 建立自訂物件 (要先建立Model類別)
2. 建立管理物件
3. 註冊服務
```cs
builder.Services.AddSingleton(___Model物件___);
```
## AddSingleton()
`AddSingleton()` 是 .NET Core 中的一個方法，它可以將服務註冊為單例實例。以下是一個範例：

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<IMyService, MyService>();
}
```

-----
# 註冊服務: 有關URL
1. 建立自訂物件 (要先建立Model類別)
2. 建立管理物件
   ```cs
ConfigurationManager manager = builder.Configuration;
```
3. 讀取Section
   ```cs
IConfigurationSection section = configurationManager.GetSection("___appsetting.json自訂物件名稱___");
```
4. 綁定Section
   ```cs
section.Bind(___Model物件___);
```
5. 註冊服務
   ```cs
builder.Services.AddSingleton(___Model物件___);
```

## ConfigurationManager 
[`ConfigurationManager` 是一個 C# 中的類別，它提供了對用戶端應用程式組態檔的存取](https://zhuanlan.zhihu.com/p/413964365)[1](https://zhuanlan.zhihu.com/p/413964365)[。如果您想要使用 `ConfigurationManager` 類別，您需要參考 `System.Configuration` 組件](https://zhuanlan.zhihu.com/p/413964365)[1](https://zhuanlan.zhihu.com/p/413964365)。

## IConfigurationSection 
`IConfigurationSection` 是 .NET Core 中的一個介面，它表示應用程式設定的一個節點。您可以使用 `IConfigurationSection` 來讀取和寫入應用程式設定。以下是一個範例：

```csharp
public void ConfigureServices(IServiceCollection services)
{
    IConfigurationSection mySection = Configuration.GetSection("MySection");
    services.AddSingleton(mySection);
}
```
