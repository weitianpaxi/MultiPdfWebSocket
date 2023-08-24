using System;
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
        /// 参数问题返回消息

        /// <summary>
        /// 异常参数
        /// </summary>
        public const string PARAMETER_ERROR = "异常参数";

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

        /// <summary>
        /// 文件打开成功
        /// </summary>
        public const string OPEN_FILE_SUCCESSFUL = "文件打开成功";

        /// <summary>
        /// 文件打开失败
        /// </summary>
        public const string OPEN_FILE_FAILED = "文件打开失败";

        /// <summary>
        /// 没有打开文件
        /// </summary>
        public const string NO_OPEN_FILES = "未打开文件，请打开文件后操作";

        /// <summary>
        /// 文件打印成功
        /// </summary>
        public const string FILE_PRINTED_SUCCESSFULLY = "文件打印成功";

        /// <summary>
        /// 文件打印失败
        /// </summary>
        public const string FILE_PRINTING_FAILED = "文件打印失败";

        /// <summary>
        /// 文件关闭成功
        /// </summary>
        public const string CLOSE_FILE_SUCCESSFUL = "文件关闭成功";

        /// <summary>
        /// 另存为文件成功
        /// </summary>
        public const string SAVE_AS_FILE_SUCCESSFUL = "另存为文件成功";

        /// <summary>
        /// 另存为文件失败
        /// </summary>
        public const string SAVE_AS_FILE_FAILED = "另存为文件失败";

        /// <summary>
        /// 跳转页面成功
        /// </summary>
        public const string GOTO_PAGE_SUCCESSFUL = "跳转页面成功";

        /// <summary>
        /// 书签跳转成功
        /// </summary>
        public const string GOTO_BOOKMARK_SUCCESSFUL = "书签跳转成功";

        /// <summary>
        /// 坐标签章成功
        /// </summary>
        public const string SIGN_BY_POS_SUCCESSFUL = "坐标签章成功";

        /// <summary>
        /// 坐标签章失败
        /// </summary>
        public const string SIGN_BY_POS_FAILED = "坐标签章失败";

        /// <summary>
        /// 关键字签章成功
        /// </summary>
        public const string SIGN_BY_KEYWORD_SUCCESSFUL = "关键字签章成功";

        /// <summary>
        /// 关键字签章失败
        /// </summary>
        public const string SIGN_BY_KEYWORD_FAILED = "关键字签章失败";

        /// <summary>
        /// 获取印章数量失败
        /// </summary>
        public const string GET_SEAL_NUMBER_FAILED = "获取印章数量失败";

        /// <summary>
        /// 获取印章信息失败
        /// </summary>
        public const string GET_SEAL_INFO_FAILED = "获取印章信息失败";
    }
}
