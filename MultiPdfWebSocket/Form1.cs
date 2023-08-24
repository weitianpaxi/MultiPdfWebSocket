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
        private const int BufferSize = ProfileConstant.REQUEST_SIZE;    // 消息体默认大小
        private WebSocket websocket;                    // 服务器对象
        private string userIpAddress;                   // 连接客户端地址
        private Boolean selfSignState = true;           // 用户自主签章状态
        private Boolean messageSignState = false;       // 消息体签章状态
        public Form1()
        {
            InitializeComponent();
            LogInit();
            OcxInit();
            _ = this.Start();
        }

        /// <summary>
        /// 初始化OCX控件
        /// </summary>
        private void OcxInit()
        {
            this.axPDFView1.SetRCPath(ProfileConstant.RC_PATH);
            this.axPDFView1.SetCaType(0);
            this.axPDFView1.AfterSignPDF += AxPDFView1_AfterSignPDF;
        }

        /// <summary>
        /// 初始化日志记录信息
        /// </summary>
        private void LogInit()
        {
            //读取XML配置信息
            XMLHelper.ReadXml();
            //日志清除
            Task.Factory.StartNew(() =>
            {
                DirectoryInfo di = new DirectoryInfo(LogParameter.LogFilePath);
                if (!di.Exists)
                    di.Create();
                FileInfo[] fi = di.GetFiles("MultiPdfWebSocket_*.log");
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

            LogHelper.Info("WebSocket server started.");

            while (true)
            {
                HttpListenerContext context = await listener.GetContextAsync();
                if (context.Request.IsWebSocketRequest)
                    ProcessWebSocketRequest(context);
                else
                    context.Response.Close();
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
                LogHelper.Info("WebSocket client " + userIpAddress + " connected.");
                
                await ReceiveLoop();
            }
            catch (Exception ex)
            {
                LogHelper.Error($"WebSocket error: {ex.Message}");
            }
            finally
            {
                if (websocketContext != null && websocketContext.WebSocket != null)
                    await websocketContext.WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close", CancellationToken.None);

                LogHelper.Info("WebSocket client disconnected.");
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
                    await websocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close", CancellationToken.None);
                else
                    // 处理接收到的消息
                    await ProcessMessage(buffer, result.Count);
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
            //this.TopMost = true;
            // 处理接收到的消息
            string resultMessage = Encoding.UTF8.GetString(buffer, 0, length);                          // 转换接受到的数据编码格式 
            if (resultMessage == null || resultMessage == "")
                responseMessage = MessageConstant.PARAMETER_ERROR;
            else
            {
                var json_resultMessage = JsonConvert.DeserializeObject<JObject>(resultMessage);         // json数据序列化
                LogHelper.Info("json数据序列化为：" + json_resultMessage.ToString());
                if (json_resultMessage[ProfileConstant.METHOD].ToString().Equals(InterfaceMethodConstant.SHOW_PDF_SERVICE))
                {
                    responseMessage = JsonConvert.SerializeObject(new Response(InterfaceMethodConstant.SHOW_PDF_SERVICE,
                        ShowPdfService(json_resultMessage)));
                }
                if (json_resultMessage[ProfileConstant.METHOD].ToString().Equals(InterfaceMethodConstant.CLOSE_PDF_SERVICE))
                {
                    responseMessage = JsonConvert.SerializeObject(new Response(InterfaceMethodConstant.SHOW_PDF_SERVICE,
                        ClosePdfService()));
                }
                if (json_resultMessage[ProfileConstant.METHOD].ToString().Equals(InterfaceMethodConstant.ENABLE_TOOLBAR_BUTTON))
                {
                    responseMessage = JsonConvert.SerializeObject(new Response(InterfaceMethodConstant.ENABLE_TOOLBAR_BUTTON,
                        EnableToolBarButton(json_resultMessage)));
                }
                if (json_resultMessage[ProfileConstant.METHOD].ToString().Equals(InterfaceMethodConstant.SNCA_OPEN_PDF))
                {
                    responseMessage = JsonConvert.SerializeObject(new Response(InterfaceMethodConstant.SNCA_OPEN_PDF, OpenPdf()));
                }
                if (json_resultMessage[ProfileConstant.METHOD].ToString().Equals(InterfaceMethodConstant.SNCA_OPEN_PDF_BY_PATH))
                {
                    responseMessage = JsonConvert.SerializeObject(new Response(InterfaceMethodConstant.SNCA_OPEN_PDF_BY_PATH,
                        OpenPdfByPath(json_resultMessage)));
                }
                if (json_resultMessage[ProfileConstant.METHOD].ToString().Equals(InterfaceMethodConstant.GET_FILE_PATH))
                {
                    responseMessage = JsonConvert.SerializeObject(new Response(InterfaceMethodConstant.GET_FILE_PATH, GetFilePath()));
                }
                if (json_resultMessage[ProfileConstant.METHOD].ToString().Equals(InterfaceMethodConstant.PRINT_FILE))
                {
                    responseMessage = JsonConvert.SerializeObject(new Response(InterfaceMethodConstant.PRINT_FILE, PrintFile()));
                }
                if (json_resultMessage[ProfileConstant.METHOD].ToString().Equals(InterfaceMethodConstant.CLOSE_FILE))
                {
                    responseMessage = JsonConvert.SerializeObject(new Response(InterfaceMethodConstant.CLOSE_FILE, CloseFile()));
                }
                if (json_resultMessage[ProfileConstant.METHOD].ToString().Equals(InterfaceMethodConstant.SAVS_AS))
                {
                    responseMessage = JsonConvert.SerializeObject(new Response(InterfaceMethodConstant.SAVS_AS, 
                        SaveAs(json_resultMessage)));
                }
                if (json_resultMessage[ProfileConstant.METHOD].ToString().Equals(InterfaceMethodConstant.GET_PAGE_COUNTS))
                {
                    responseMessage = JsonConvert.SerializeObject(new Response(InterfaceMethodConstant.GET_PAGE_COUNTS, GetPageCounts()));
                }
                if (json_resultMessage[ProfileConstant.METHOD].ToString().Equals(InterfaceMethodConstant.GET_CUR_PAGE_NO))
                {
                    responseMessage = JsonConvert.SerializeObject(new Response(InterfaceMethodConstant.GET_CUR_PAGE_NO, GetCurPageNo()));
                }
                if (json_resultMessage[ProfileConstant.METHOD].ToString().Equals(InterfaceMethodConstant.SET_PAGE_INDEX))
                {
                    responseMessage = JsonConvert.SerializeObject(new Response(InterfaceMethodConstant.SET_PAGE_INDEX,
                        SetPageIndex(json_resultMessage)));
                }
                if (json_resultMessage[ProfileConstant.METHOD].ToString().Equals(InterfaceMethodConstant.GOTO_BOOKMARK_BY_BOOKMARK_NAME))
                {
                    responseMessage = JsonConvert.SerializeObject(new Response(InterfaceMethodConstant.GOTO_BOOKMARK_BY_BOOKMARK_NAME,
                        GoToBookMarkByBookMarkName(json_resultMessage)));
                }
                if (json_resultMessage[ProfileConstant.METHOD].ToString().Equals(InterfaceMethodConstant.TZ_SIGN_BY_POS_3))
                {
                    responseMessage = JsonConvert.SerializeObject(new Response(InterfaceMethodConstant.TZ_SIGN_BY_POS_3, 
                        TZSignByPos3(json_resultMessage)));
                    selfSignState = true;    // 更新用户自主签章状态
                }
                if (json_resultMessage[ProfileConstant.METHOD].ToString().Equals(InterfaceMethodConstant.TZ_SIGN_BY_KEYWORD_3))
                {
                    responseMessage = JsonConvert.SerializeObject(new Response(InterfaceMethodConstant.TZ_SIGN_BY_KEYWORD_3,
                        TZSignByKeyword3(json_resultMessage)));
                    selfSignState = true;    // 更新用户自主签章状态
                }
                if (json_resultMessage[ProfileConstant.METHOD].ToString().Equals(InterfaceMethodConstant.GET_REAL_SIGNATURES))
                {
                    responseMessage = JsonConvert.SerializeObject(new Response(InterfaceMethodConstant.GET_REAL_SIGNATURES,
                        GetRealSignatures()));
                }
                if (json_resultMessage[ProfileConstant.METHOD].ToString().Equals(InterfaceMethodConstant.GET_CURRENT_SIGNATURE_COUNT))
                {
                    responseMessage = JsonConvert.SerializeObject(new Response(InterfaceMethodConstant.GET_CURRENT_SIGNATURE_COUNT,
                        GetCurrentSignatureCount()));
                }
                if (json_resultMessage[ProfileConstant.METHOD].ToString().Equals(InterfaceMethodConstant.TZ_GET_USER_SIGN_COUNT))
                {
                    responseMessage = JsonConvert.SerializeObject(new Response(InterfaceMethodConstant.TZ_GET_USER_SIGN_COUNT,
                        GetUserSignCount(json_resultMessage)));
                }
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
        private Result ShowPdfService(JObject Parameter)
        {
            if (Parameter[ProfileConstant.PARAMETER]["TopMost"] != null)
            {
                if ((bool)Parameter[ProfileConstant.PARAMETER]["TopMost"])
                    this.TopMost = true;
                else
                    this.TopMost = false;
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
                return Result.Success(MessageConstant.SHOWPDFSERVICE_SUCCESSFUL);
            }
            else
                return Result.Error(MessageConstant.PARAMETER_ERROR);
        }

        /// <summary>
        /// 隐藏Pdf插件
        /// </summary>
        /// <returns></returns>
        private Result ClosePdfService()
        {
            this.WindowState = FormWindowState.Minimized;
            return Result.Success(MessageConstant.CLOSEPDFSERVICE_SUCCESSFUL);
        }

        /// <summary>
        /// 插件工具栏按钮显示或隐藏
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        private Result EnableToolBarButton(JObject Parameter)
        {
            if (Parameter[ProfileConstant.PARAMETER]["IsEnable"] != null && 
                Parameter[ProfileConstant.PARAMETER]["ButtonId"] != null)
            {
                if ((bool)Parameter[ProfileConstant.PARAMETER]["IsEnable"])
                {
                    DealEnableToolBarButton(Parameter);
                    return Result.Success(MessageConstant.TOOLBAR_BUTTON_DISPLAY_SUCCESSFUL);
                }
                else
                {
                    DealEnableToolBarButton(Parameter);
                    return Result.Success(MessageConstant.TOOLBAR_BUTTON_HIDDEN_SUCCESSFUL);
                }
            }
            else
                return Result.Error(MessageConstant.PARAMETER_ERROR);  
        }

        /// <summary>
        /// 隐藏ToolBarButton具体实现
        /// </summary>
        /// <param name="Parameter"></param>
        private void DealEnableToolBarButton(JObject Parameter)
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
        private Result OpenPdf()
        {
            int ret = this.axPDFView1.SNCAOpenPdf();
            if (ret == 0)
                return Result.Success(DealGetFilePath());
            else
                return Result.Error(MessageConstant.OPEN_FILE_FAILED);
        }

        /// <summary>
        /// 获取当前打开文档的全路径
        /// </summary>
        /// <returns></returns>
        private string DealGetFilePath()
        {
            return this.axPDFView1.GetFilePath();
        }

        /// <summary>
        /// 根据路径打开文件
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        private Result OpenPdfByPath(JObject Parameter)
        {
            if (Parameter[ProfileConstant.PARAMETER]["Type"] != null && 
                Parameter[ProfileConstant.PARAMETER]["Path"] != null)
            {
                int.TryParse(Parameter[ProfileConstant.PARAMETER]["Type"].ToString(), out int type);
                string path = Parameter[ProfileConstant.PARAMETER]["Path"].ToString();
                if (this.axPDFView1.SNCAOpenPdfByPath(path, type) == 0)
                    return Result.Success(DealGetFilePath());
                else
                    return Result.Error(MessageConstant.OPEN_FILE_FAILED);
            }
            else
                return Result.Error(MessageConstant.PARAMETER_ERROR);
        }

        /// <summary>
        /// 判断是否有文件打开
        /// </summary>
        /// <returns></returns>
        private Boolean IsOpenAFile()
        {
            string filePath = DealGetFilePath();
            if (filePath != null && filePath != "")
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获取打开文件路径
        /// </summary>
        /// <returns></returns>
        private Result GetFilePath()
        {
            if (IsOpenAFile())
                return Result.Success(DealGetFilePath());
            else
                return Result.Error(MessageConstant.NO_OPEN_FILES);
        }

        /// <summary>
        /// 打印文件
        /// </summary>
        /// <returns></returns>
        private Result PrintFile()
        {
            if (IsOpenAFile())
            {
                this.axPDFView1.PrintFile();
                return Result.Success(MessageConstant.FILE_PRINTED_SUCCESSFULLY);
            }
            else
                return Result.Error(MessageConstant.FILE_PRINTING_FAILED);
        }

        /// <summary>
        /// 关闭文件
        /// </summary>
        /// <returns></returns>
        private Result CloseFile()
        {
            if (IsOpenAFile())
            {
                this.axPDFView1.CloseFile();
                return Result.Success(MessageConstant.CLOSE_FILE_SUCCESSFUL);
            }
            else
                return Result.Error(MessageConstant.NO_OPEN_FILES);
        }

        /// <summary>
        /// 另存为文件
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        private Result SaveAs(JObject Parameter)
        {
            if (Parameter[ProfileConstant.PARAMETER]["FilePath"] != null &&
                Parameter[ProfileConstant.PARAMETER]["IsReplace"] != null)
            {
                int.TryParse(Parameter[ProfileConstant.PARAMETER]["IsReplace"].ToString(), out int IsReplace);
                if (this.axPDFView1.SaveAs(Parameter[ProfileConstant.PARAMETER]["FilePath"].ToString(), IsReplace) == 1)
                    return Result.Success(MessageConstant.SAVE_AS_FILE_SUCCESSFUL);
                else
                    return Result.Error(MessageConstant.SAVE_AS_FILE_FAILED);
            }
            else
                return Result.Error(MessageConstant.PARAMETER_ERROR);
        }

        /// <summary>
        /// 获取文件总页数
        /// </summary>
        /// <returns></returns>
        private Result GetPageCounts()
        {
            if (IsOpenAFile())
                return Result.Success(this.axPDFView1.GetPageCounts().ToString());
            else
                return Result.Error(MessageConstant.NO_OPEN_FILES);
        }

        /// <summary>
        /// 获取当前文档停留页码
        /// </summary>
        /// <returns></returns>
        private Result GetCurPageNo()
        {
            if (IsOpenAFile())
                return Result.Success(this.axPDFView1.GetCurPageNo().ToString());
            else
                return Result.Error(MessageConstant.NO_OPEN_FILES);
        }

        /// <summary>
        /// 跳转页面
        /// </summary>
        /// <returns></returns>
        private Result SetPageIndex(JObject Parameter)
        {
            if (Parameter[ProfileConstant.PARAMETER]["PageIndex"] != null)
            {
                if (IsOpenAFile())
                {
                    int.TryParse(Parameter[ProfileConstant.PARAMETER]["PageIndex"].ToString(), out int pageIndex);
                    this.axPDFView1.TZGoToPage(pageIndex);
                    return Result.Success(MessageConstant.GOTO_PAGE_SUCCESSFUL);
                }
                else
                    return Result.Error(MessageConstant.NO_OPEN_FILES);
            }
            else
                return Result.Error(MessageConstant.PARAMETER_ERROR);
        }

        /// <summary>
        /// 书签跳转
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        private Result GoToBookMarkByBookMarkName(JObject Parameter)
        {
            if (Parameter[ProfileConstant.PARAMETER]["BookMarkName"] != null)
            {
                if (IsOpenAFile())
                {
                    this.axPDFView1.GoToBookMarkByBookMarkName(Parameter[ProfileConstant.PARAMETER]["BookMarkName"].ToString());
                    return Result.Success(MessageConstant.GOTO_BOOKMARK_SUCCESSFUL);
                }
                else
                    return Result.Error(MessageConstant.NO_OPEN_FILES);
            }
            else
                return Result.Error(MessageConstant.PARAMETER_ERROR);
        }

        /// <summary>
        /// 坐标签章
        /// </summary>
        /// <returns></returns>
        private Result TZSignByPos3(JObject Parameter)
        {
            if (Parameter[ProfileConstant.PARAMETER]["Pages"] != null &&
                Parameter[ProfileConstant.PARAMETER]["xCenter"] != null &&
                Parameter[ProfileConstant.PARAMETER]["yCenter"] != null)
            {
                if (IsOpenAFile())
                {
                    selfSignState = false;
                    messageSignState = true;
                    string pages = Parameter[ProfileConstant.PARAMETER]["Pages"].ToString();
                    int.TryParse(Parameter[ProfileConstant.PARAMETER]["xCenter"].ToString(), out int xCenter);
                    int.TryParse(Parameter[ProfileConstant.PARAMETER]["yCenter"].ToString(), out int yCenter);
                    if (this.axPDFView1.TZSignByPos3(pages, xCenter, yCenter) == 0)
                        return Result.Success(MessageConstant.SIGN_BY_POS_SUCCESSFUL);
                    else
                        return Result.Error(MessageConstant.SIGN_BY_POS_FAILED);
                }
                else
                    return Result.Error(MessageConstant.NO_OPEN_FILES);
            }
            else
                return Result.Error(MessageConstant.PARAMETER_ERROR);
        }

        /// <summary>
        /// 关键字签章
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        private Result TZSignByKeyword3(JObject Parameter)
        {
            if (null != Parameter[ProfileConstant.PARAMETER]["Keyword"] &&
                null != Parameter[ProfileConstant.PARAMETER]["Pages"] &&
                null != Parameter[ProfileConstant.PARAMETER]["Indexes"])
            {
                if (IsOpenAFile())
                {
                    selfSignState = false;
                    messageSignState = true;
                    int ret = this.axPDFView1.TZSignByKeyword3(Parameter[ProfileConstant.PARAMETER]["Keyword"].ToString(),
                        Parameter[ProfileConstant.PARAMETER]["Pages"].ToString(),
                        Parameter[ProfileConstant.PARAMETER]["Indexes"].ToString());
                    if (0 == ret)
                        return Result.Success(MessageConstant.SIGN_BY_KEYWORD_SUCCESSFUL);
                    else
                        return Result.Error(MessageConstant.SIGN_BY_KEYWORD_FAILED);
                }
                else
                    return Result.Error(MessageConstant.NO_OPEN_FILES);
            }
            else
                return Result.Error(MessageConstant.PARAMETER_ERROR);
        }

        /// <summary>
        /// 获取印章的详细信息,用于无序签章
        /// </summary>
        /// <returns></returns>
        private Result GetRealSignatures()
        {
            if (IsOpenAFile())
            {
                // 返回的为印章的详细信息，包括时间，坐标和签名值组成的xml文档的字符串，可以解析为xml文档对象进一步处理
                return Result.Success(this.axPDFView1.GetRealSignatures(null));
            }
            else
                return Result.Error(MessageConstant.GET_SEAL_INFO_FAILED);
        }

        /// <summary>
        /// 获取当前打开文档的印章数量
        /// </summary>
        /// <returns></returns>
        private Result GetCurrentSignatureCount()
        {
            if (IsOpenAFile())
                return Result.Success(this.axPDFView1.GetCurrentSignatureCount().ToString());
            else
                return Result.Error(MessageConstant.GET_SEAL_NUMBER_FAILED);
        }

        /// <summary>
        /// 获取相同证书序列号下的印章数量
        /// </summary>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        private Result GetUserSignCount(JObject Parameter)
        {
            
            if (Parameter[ProfileConstant.PARAMETER]["CertCode"] != null)
            {
                if (IsOpenAFile())
                {
                    string CertCode = Parameter[ProfileConstant.PARAMETER]["CertCode"].ToString();
                    return Result.Success(this.axPDFView1.TZGetUserSignCount(CertCode).ToString());
                }
                else
                    return Result.Error(MessageConstant.GET_SEAL_NUMBER_FAILED);
            }
            else
                return Result.Error(MessageConstant.PARAMETER_ERROR);
        }

        /// <summary>
        /// 签章结束消息处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AxPDFView1_AfterSignPDF(object sender, EventArgs e)
        {
            LogHelper.Info(userIpAddress + " => Executed signature operation");
            if (selfSignState || messageSignState != true)
            {
                string message = JsonConvert.SerializeObject(new Response(EventMessageConstant.AFTER_SIGN_PDF, Result.Success(MessageConstant.USER_SELF_SIGN)));
                byte[] responseBytes = Encoding.UTF8.GetBytes(message);
                websocket.SendAsync(new ArraySegment<byte>(responseBytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            throw new NotImplementedException();
        }
    }
}
