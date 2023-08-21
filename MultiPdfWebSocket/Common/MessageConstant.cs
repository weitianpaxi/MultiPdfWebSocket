﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPdfWebSocket.Common
{
    /// <summary>
    /// 响应消息常量定义
    /// </summary>
    class MessageConstant
    {
        /// 以下是处理OCX控件消息的响应消息
        
        /// <summary>
        /// 签章消息结束提示信息
        /// </summary>
        public const string USER_SELF_SIGN = "用户自主签章完成";

        /// 以下是正常函数操作的提示信息

        /// <summary>
        /// 展示PDF服务端成功
        /// </summary>
        public const string SHOWPDFSERVICE_SUCCESSFUL = "PDF服务端展示成功";

        /// <summary>
        /// 隐藏PDF服务端成功
        /// </summary>
        public const string CLOSEPDFSERVICE_SUCCESSFUL = "PDF服务端隐藏成功";

        /// <summary>
        /// 工具栏按钮隐藏成功
        /// </summary>
        public const string TOOLBAR_BUTTON_HIDDEN_SUCCESSFUL = "工具栏按钮隐藏成功";

        /// <summary>
        /// 工具栏按钮显示成功
        /// </summary>
        public const string TOOLBAR_BUTTON_DISPLAY_SUCCESSFUL = "工具栏按钮显示成功";
    }
}
