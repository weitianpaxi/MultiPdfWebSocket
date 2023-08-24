# Multi_PDF_WebSocket

## 项目简介

这是一款利用`websocket`网络协议的`PDF`签章插件服务，客户端发送`json`格式的请求数据，插件会执行相应请求参数的方法。

## 项目结构

```shell
│  App.config
│  Form1.cs
│  Form1.Designer.cs
│  Form1.resx
│  MultiPdfWebSocket.csproj
│  packages.config
│  Program.cs
│  Project_Structure.txt
│  README.md
│  
├─Common
│      EventMessageConstant.cs
│      InterfaceMethodConstant.cs
│      MessageConstant.cs
│      OperationalStateConstant.cs
│      ProfileConstant.cs
│      Response.cs
│      Result.cs
│      
├─Config
│      Config.xml
│      
├─Properties
│      app.manifest
│      AssemblyInfo.cs
│      Resources.Designer.cs
│      Resources.resx
│      Settings.Designer.cs
│      Settings.settings
│      
└─Utility
        LogHelper.cs
        LogParameter.cs
        XMLHelper.cs
```

其中根目录下为程序主运行代码，以及窗体定义代码，其他目录所保存的为

- `Common`:该文件下存放有各种常量类的定义。
- `Config`:该文件夹下存放各种自定义配置文件。
- `Properties`:该文件夹下存放程序运行的必要配置信息。
- `Utility`:该文件夹下存放项目中使用的通用的工具类。