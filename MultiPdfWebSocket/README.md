# _PDF_

## 项目简介

本项目是利用`websocket`网络通信协议，接受`json`格式的消息，并从中解析具体命令，执行相应操作的服务。

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

其中，根目录下为程序运行的与功能实现的具体代码。各个不同文件夹的功能为：

- `Common`:存放项目中的常量类的定义。
- `Config`:存放用户自定义的配置文件。
- `Properties`:程序运行的核心配置文件。
- `Utility`:存放项目所需的公用的工具类。