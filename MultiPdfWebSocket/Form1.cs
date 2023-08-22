﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.WebSockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MultiPdfWebSocket.Utility;
using MultiPdfWebSocket.Common;



namespace MultiPdfWebSocket
{
    public partial class Form1 : Form
    {
        private const int BufferSize = ProfileConstant.REQUEST_SIZE;
        private WebSocket websocket;
        private string userIpAddress;
        public Form1()
        {
            InitializeComponent();
            logInit();
            ocxInit();
            _ = this.Start();
        }

        /// <summary>
        /// 初始化OCX控件
        /// </summary>
        private void ocxInit()
        {
            this.axPDFView1.SetRCPath(ProfileConstant.RC_PATH);
            this.axPDFView1.SetCaType(0);
            this.axPDFView1.AfterSignPDF += AxPDFView1_AfterSignPDF;
        }

        /// <summary>
        /// 初始化日志记录信息
        /// </summary>
        private void logInit()
        {
            //读取XML配置信息
            XMLHelper.ReadXml();
            //日志清除
            Task.Factory.StartNew(() =>
            {
                DirectoryInfo di = new DirectoryInfo(LogParameter.LogFilePath);
                if (!di.Exists)
                    di.Create();
                FileInfo[] fi = di.GetFiles("Demo_*.log");
                DateTime dateTime = DateTime.Now;
                foreach (FileInfo info in fi)
                {
                    TimeSpan ts = dateTime.Subtract(info.LastWriteTime);
                    if (ts.TotalDays > LogParameter.LogFileExistDay)
                    {
                        info.Delete();
                        LogHelper.Debug(string.Format("Deleted log. {0}", info.Name));
                    }
                }
                LogHelper.Debug("Log cleaning completed.");
            });
        }

        /// <summary>
        /// 启动websocket服务器
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(ProfileConstant.LISTENER_URL);
            listener.Start();

            LogHelper.log.Info("WebSocket server started.");

            while (true)
            {
                HttpListenerContext context = await listener.GetContextAsync();
                if (context.Request.IsWebSocketRequest)
                {
                    ProcessWebSocketRequest(context);
                }
                else
                {
                    context.Response.Close();
                }
            }
        }

        /// <summary>
        /// 处理响应消息
        /// </summary>
        /// <param name="context"></param>
        private async void ProcessWebSocketRequest(HttpListenerContext context)
        {
            WebSocketContext websocketContext = null;
            try
            {
                websocketContext = await context.AcceptWebSocketAsync(subProtocol: null);
                websocket = websocketContext.WebSocket;

                userIpAddress = context.Request.UserHostAddress;    // 记录客户端请求IP
                LogHelper.log.Info("WebSocket client " + userIpAddress + " connected.");
                
                await ReceiveLoop();
            }
            catch (Exception ex)
            {
                LogHelper.log.Error($"WebSocket error: {ex.Message}");
            }
            finally
            {
                if (websocketContext != null && websocketContext.WebSocket != null)
                    await websocketContext.WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close", CancellationToken.None);

                LogHelper.log.Info("WebSocket client disconnected.");
            }
        }

        /// <summary>
        /// 循环接受请求
        /// </summary>
        /// <returns></returns>
        private async Task ReceiveLoop()
        {
            byte[] buffer = new byte[BufferSize];

            while (websocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await websocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await websocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close", CancellationToken.None);
                }
                else
                {
                    // 处理接收到的消息
                    await ProcessMessage(buffer, result.Count);
                }
            }
        }

        /// <summary>
        /// 处理具体请求
        /// </summary>
        /// <param name="buffer">请求信息</param>
        /// <param name="length">请求信息长度</param>
        /// <returns></returns>
        private async Task ProcessMessage(byte[] buffer, int length)
        {
            string responseMessage = "";            // 定义返回消息
            //int ret;                                // 定义操作状态
            // 处理接收到的消息
            //this.TopMost = true;                    // 窗口置顶显示   
            string resultMessage = Encoding.UTF8.GetString(buffer, 0, length);                      // 转换接受到的数据编码格式 

            var json_resultMessage = JsonConvert.DeserializeObject<JObject>(resultMessage);         // json数据序列化
            LogHelper.log.Info("json数据序列化为，{}" + json_resultMessage.ToString());
            if (json_resultMessage[ProfileConstant.METHOD].ToString().Equals(InterfaceMethodConstant.SHOW_PDF_SERVICE))
            {
                responseMessage = JsonConvert.SerializeObject(new Response(InterfaceMethodConstant.SHOW_PDF_SERVICE, 
                    Result.Success(ShowPdfService(json_resultMessage))));
            }
            if (json_resultMessage[ProfileConstant.METHOD].ToString().Equals(InterfaceMethodConstant.CLOSE_PDF_SERVICE))
            {
                responseMessage = JsonConvert.SerializeObject(new Response(InterfaceMethodConstant.SHOW_PDF_SERVICE, 
                    Result.Success(ClosePdfService())));
            }
            if (json_resultMessage[ProfileConstant.METHOD].ToString().Equals(InterfaceMethodConstant.ENABLE_TOOLBAR_BUTTON))
            {
                responseMessage = JsonConvert.SerializeObject(new Response(InterfaceMethodConstant.ENABLE_TOOLBAR_BUTTON, 
                    Result.Success(EnableToolBarButton(json_resultMessage))));
            }
            if (json_resultMessage[ProfileConstant.METHOD].ToString().Equals(InterfaceMethodConstant.SNCA_OPEN_PDF))
            {
                
            }

            // 发送回执消息
            byte[] responseBytes = Encoding.UTF8.GetBytes(responseMessage);
            await websocket.SendAsync(new ArraySegment<byte>(responseBytes), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        /// <summary>
        /// 显示Pdf插件
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        public string ShowPdfService(JObject Parameter)
        {
            if ((bool)Parameter[ProfileConstant.PARAMETER]["TopMost"])
            {
                this.TopMost = true;
            }
            else
            {
                this.TopMost = false;
            }
            if (Parameter[ProfileConstant.PARAMETER].ToList().Count > 1)
            {
                int posX = ProfileConstant.DEFAULT_POS_X, posY = ProfileConstant.DEFAULT_POS_Y;
                int width = ProfileConstant.DEFAULT_WIDTH, height = ProfileConstant.DEFAULT_HEIGHT;
                if (Parameter[ProfileConstant.PARAMETER]["posX"] != null)
                    int.TryParse(Parameter[ProfileConstant.PARAMETER]["posX"].ToString(), out posX);
                if (Parameter[ProfileConstant.PARAMETER]["posY"] != null)
                    int.TryParse(Parameter[ProfileConstant.PARAMETER]["posY"].ToString(), out posY);
                if (Parameter[ProfileConstant.PARAMETER]["width"] != null)
                    int.TryParse(Parameter[ProfileConstant.PARAMETER]["width"].ToString(), out width);
                if (Parameter[ProfileConstant.PARAMETER]["height"] != null)
                    int.TryParse(Parameter[ProfileConstant.PARAMETER]["height"].ToString(), out height);
                this.WindowState = FormWindowState.Normal;
                this.Location = new Point(posX, posY);                      // 设置窗口的位置
                this.Size = new Size(width, height);                        // 设置窗口的大小
                this.axPDFView1.Location = new Point(0, 0);                 // 位置更新后设置控件相对位置
                this.axPDFView1.Size = new Size(width, height);             // 保证控件随外框大小变化
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.StartPosition = FormStartPosition.CenterScreen;        // 窗口显示在屏幕中央
            }
            return MessageConstant.SHOWPDFSERVICE_SUCCESSFUL;
        }

        /// <summary>
        /// 隐藏Pdf插件
        /// </summary>
        /// <returns></returns>
        public string ClosePdfService()
        {
            this.WindowState = FormWindowState.Minimized;
            return MessageConstant.CLOSEPDFSERVICE_SUCCESSFUL;
        }

        /// <summary>
        /// 插件工具栏按钮显示或隐藏
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        public string EnableToolBarButton(JObject Parameter)
        {
            if ((bool)Parameter[ProfileConstant.PARAMETER]["IsEnable"])
            {
                DealEnableToolBarButton(Parameter);
                return MessageConstant.TOOLBAR_BUTTON_DISPLAY_SUCCESSFUL;
            }
            else
            {
                DealEnableToolBarButton(Parameter);
                return MessageConstant.TOOLBAR_BUTTON_HIDDEN_SUCCESSFUL;
            }
        }

        /// <summary>
        /// 隐藏ToolBarButton具体实现
        /// </summary>
        /// <param name="Parameter"></param>
        public void DealEnableToolBarButton(JObject Parameter)
        {
            if (Parameter[ProfileConstant.PARAMETER]["ButtonId"].ToString().Length > 1)
            {
                string[] strButtonIdArray = Parameter[ProfileConstant.PARAMETER]["ButtonId"].ToString().Split("-".ToCharArray());
                foreach (string strButtonId in strButtonIdArray)
                {
                    int.TryParse(strButtonId, out int tempButtonId);
                    this.axPDFView1.EnableToolBarButton(tempButtonId, (bool)Parameter[ProfileConstant.PARAMETER]["IsEnable"]);
                }
            }
            else
            {
                int.TryParse(Parameter[ProfileConstant.PARAMETER]["ButtonId"].ToString(), out int buttonId);
                this.axPDFView1.EnableToolBarButton(buttonId, (bool)Parameter[ProfileConstant.PARAMETER]["IsEnable"]);
            }
        }

        /// <summary>
        /// 打开本地文件（对话框）
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        public string OpenPdf()
        {
            int ret = this.axPDFView1.SNCAOpenPdf();
            if (ret == 0)
            {
                return DealGetFilePath();
            }
            else
            {
                return MessageConstant.OPEN_FILE_FAILED;
            }
        }

        /// <summary>
        /// 获取当前打开文档的全路径
        /// </summary>
        /// <returns></returns>
        public string DealGetFilePath()
        {
            return this.axPDFView1.GetFilePath();
        }

        /// <summary>
        /// 根据路径打开文件
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        public string OpenPdfByPath(JObject Parameter)
        {
            int.TryParse(Parameter[ProfileConstant.PARAMETER]["Type"].ToString(), out int type);
            string path = Parameter[ProfileConstant.PARAMETER]["Path"].ToString();
            int ret = this.axPDFView1.SNCAOpenPdfByPath(path, type);
            if (ret == 0)
            {
                return DealGetFilePath();
            }
            else
            {
                return MessageConstant.OPEN_FILE_FAILED;
            }
        }

        /// <summary>
        /// 获取打开文件路径
        /// </summary>
        /// <returns></returns>
        public string GetFilePath()
        {
            return DealGetFilePath();
        }

        /// <summary>
        /// 签章结束消息处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AxPDFView1_AfterSignPDF(object sender, EventArgs e)
        {
            LogHelper.log.Info(userIpAddress + " => Executed signature operation");
            string message = JsonConvert.SerializeObject(new Response(EventMessageConstant.AFTER_SIGN_PDF, Result.Success(MessageConstant.USER_SELF_SIGN)));
            byte[] responseBytes = Encoding.UTF8.GetBytes(message);
            websocket.SendAsync(new ArraySegment<byte>(responseBytes), WebSocketMessageType.Text, true, CancellationToken.None);
            throw new NotImplementedException();
        }
    }
}
