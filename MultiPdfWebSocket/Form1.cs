using System;
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
                DirectoryInfo di = new DirectoryInfo(Parameter.LogFilePath);
                if (!di.Exists)
                    di.Create();
                FileInfo[] fi = di.GetFiles("Demo_*.log");
                DateTime dateTime = DateTime.Now;
                foreach (FileInfo info in fi)
                {
                    TimeSpan ts = dateTime.Subtract(info.LastWriteTime);
                    if (ts.TotalDays > Parameter.LogFileExistDay)
                    {
                        info.Delete();
                        LogHelper.Debug(string.Format("已删除日志。{0}", info.Name));
                    }
                }
                LogHelper.Debug("日志清理完毕。");
            });
        }

        /// <summary>
        /// 签章结束消息处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void  AxPDFView1_AfterSignPDF(object sender, EventArgs e)
        {
            LogHelper.log.Info(userIpAddress + " => 执行了签章操作");
            string message = JsonConvert.SerializeObject(new Response("Event_AfterSignPDF", new Result(MessageConstant.USER_SELF_SIGN)));
            byte[] responseBytes = Encoding.UTF8.GetBytes(message);
            websocket.SendAsync(new ArraySegment<byte>(responseBytes), WebSocketMessageType.Text, true, CancellationToken.None);
            throw new NotImplementedException();
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
            int ret;                                // 定义操作状态
            // 处理接收到的消息
            this.TopMost = true;                    // 窗口置顶显示   
            string resultMessage = System.Text.Encoding.UTF8.GetString(buffer, 0, length);                      // 转换接受到的数据编码格式 

            var json_resultMessage = JsonConvert.DeserializeObject<JObject>(resultMessage);                     // json数据序列化
            /*if (data["method"].ToString().Equals("SNCAOpenPdf"))
            {
                Console.WriteLine("现在执行的方法是：" + data["method"].ToString());
                this.TopMost = false;
                ret = this.axPDFView1.SNCAOpenPdf();
                //this.WindowState = FormWindowState.Normal;
                
                responseMessage = ret.ToString();
            }
            else if (data["method"].ToString().Equals("GetPageCounts"))
            {
                Console.WriteLine("现在执行的方法是：" + data["method"].ToString());
                this.TopMost = false;
                ret = this.axPDFView1.GetPageCounts();  // 此时获取到的是pdf文件的总页数
                responseMessage = ret.ToString();
            }
            else if (data["method"].ToString().Equals("First"))
            {
                Console.WriteLine("现在执行的方法是：" + data["method"].ToString());
                this.TopMost = false;
                this.axPDFView1.First();
                responseMessage = "已跳转到文件首页!";
            }
            else if (data["method"].ToString().Equals("Prev"))
            {
                Console.WriteLine("现在执行的方法是：" + data["method"].ToString());
                this.TopMost = false;
                int tempPageNum = this.axPDFView1.GetCurPageNo();
                if (tempPageNum > 1)
                {
                    this.axPDFView1.Prev();
                    responseMessage = "跳转上一页成功！";
                }
                else
                {
                    responseMessage = "已是文档第一页！";
                }
            }
            else if (data["method"].ToString().Equals("Next"))
            {
                Console.WriteLine("现在执行的方法是：" + data["method"].ToString());
                this.TopMost = false;
                int tempPageNum = this.axPDFView1.GetCurPageNo();
                if (tempPageNum < this.axPDFView1.GetPageCounts())
                {
                    this.axPDFView1.Next();
                    responseMessage = "跳转下一页成功！";
                }
                else
                {
                    responseMessage = "已到文档最后一页！";
                }
            }
            else if (data["method"].ToString().Equals("Last"))
            {
                Console.WriteLine("现在执行的方法是：" + data["method"].ToString());
                this.TopMost = false;
                this.axPDFView1.Last();
                responseMessage = "已跳转到文件尾页!";
            }
            else if (data["method"].ToString().Equals("TZGoToPage"))
            {
                Console.WriteLine("现在执行的方法是：" + data["method"].ToString());
                int pageNummber = (int)data["pageNum"];
                this.TopMost = false;
                ret = this.axPDFView1.TZGoToPage(pageNummber);
                if (ret == 0)
                {
                    responseMessage = "跳转打开成功！";
                }
                else
                {
                    responseMessage = "跳转打开失败，请先打开文件！";
                }
            }
            else if (data["method"].ToString().Equals("Stamp"))
            {
                Console.WriteLine("现在执行的方法是：" + data["method"].ToString());
                int type = (int)data["type"];
                this.WindowState = FormWindowState.Normal;
                ret = this.axPDFView1.Stamp(type);
                if (ret == -1)
                {
                    this.TopMost = false;
                    responseMessage = "签章失败，请打开文件后重试！";
                }
                else if (ret == 0)
                {
                    this.TopMost = false;
                    responseMessage = "签章成功，请打开文件查看详情！";
                }
            }
            else if (data["method"].ToString().Equals("TZSignByPos3"))
            {
                Console.WriteLine("现在执行的方法是：" + data["method"].ToString());
                String pageNummber = data["pageNum"].ToString();
                int posX = (int)data["posX"];
                int posY = (int)data["posY"];
                this.TopMost = false;
                ret = this.axPDFView1.TZSignByPos3(pageNummber, posX, posY);
                if (ret == -1)
                {
                    responseMessage = "签章失败，请打开文件后重试！";
                }
                else if (ret == 0)
                {
                    responseMessage = "签章成功，请打开文件查看详情！";
                }
            }
            else if (data["method"].ToString().Equals("CloseFile"))
            {
                Console.WriteLine("现在执行的方法是：" + data["method"].ToString());
                this.axPDFView1.CloseFile();
                this.TopMost = false;
                responseMessage = "文件已关闭";
            }
            else if (data["method"].ToString().Equals("NormalPdfReader"))
            {
                Console.WriteLine("现在执行的方法是：" + data["method"].ToString());
                // 显示窗口
                this.WindowState = FormWindowState.Normal;
            }
            else if (data["method"].ToString().Equals("MinimizedPdfReader"))
            {
                Console.WriteLine("现在执行的方法是：" + data["method"].ToString());
                // 最小化窗口
                this.WindowState = FormWindowState.Minimized;
            }*/
            byte[] responseBytes = Encoding.UTF8.GetBytes(responseMessage);
            await websocket.SendAsync(new ArraySegment<byte>(responseBytes), WebSocketMessageType.Text, true, CancellationToken.None);

        }
    }
}
