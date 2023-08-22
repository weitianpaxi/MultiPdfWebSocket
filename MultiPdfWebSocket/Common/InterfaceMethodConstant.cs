using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPdfWebSocket.Common
{
    /// <summary>
    /// 接口方法常量定义
    /// </summary>
    class InterfaceMethodConstant
    {
        /// <summary>
        /// 显示Pdf插件
        /// </summary>
        public const string SHOW_PDF_SERVICE = "ShowPdfService";

        /// <summary>
        /// 隐藏Pdf插件
        /// </summary>
        public const string CLOSE_PDF_SERVICE = "ClosePdfService";

        /// <summary>
        /// 插件工具栏按钮显示或隐藏
        /// </summary>
        public const string ENABLE_TOOLBAR_BUTTON = "EnableToolBarButton";

        /// <summary>
        /// 打开本地文件（对话框）
        /// </summary>
        public const string SNCA_OPEN_PDF = "SNCAOpenPdf";

        /// <summary>
        /// 根据路径打开文件
        /// </summary>
        public const string SNCA_OPEN_PDF_BY_PATH = "SNCAOpenPdfByPath";

        /// <summary>
        /// 获取打开文件路径
        /// </summary>
        public const string GET_FILE_PATH = "GetFilePath";

        /// <summary>
        /// 打印文件
        /// </summary>
        public const string PRINT_FILE = "PrintFile";

        /// <summary>
        /// 关闭文件
        /// </summary>
        public const string CLOSE_FILE = "CloseFile";

        /// <summary>
        /// 另存为文件
        /// </summary>
        public const string SAVS_AS = "SaveAs";

        /// <summary>
        /// 获取文件总页数
        /// </summary>
        public const string GET_PAGE_COUNTS = "GetPageCounts";

        /// <summary>
        /// 获取当前文档停留页码
        /// </summary>
        public const string GET_CUR_PAGE_NO = "GetCurPageNo";

        /// <summary>
        /// 跳转页面
        /// </summary>
        public const string SET_PAGE_INDEX = "SetPageIndex";

        /// <summary>
        /// 书签跳转
        /// </summary>
        public const string GOTO_BOOKMARK_BY_BOOKMARK_NAME = "GoToBookMarkByBookMarkName";

        /// <summary>
        /// 坐标签章
        /// </summary>
        public const string TZ_SIGN_BY_POS_3 = "TZSignByPos3";

        /// <summary>
        /// 关键字签章
        /// </summary>
        public const string TZ_SIGN_BY_KEYWORD_3 = "TZSignByKeyword3";

        /// <summary>
        /// 获取最后一个印章的详细信息（dll使用）
        /// </summary>
        public const string GET_REAL_SIGNATURE = "GetRealSignature";

        /// <summary>
        /// 获取印章的详细信息,用于无序签章
        /// </summary>
        public const string GET_REAL_SIGNATURES = "GetRealSignatures";

        /// <summary>
        /// 获取当前打开文档的印章数量
        /// </summary>
        public const string GET_CURRENT_SIGNATURE_COUNT = "GetCurrentSignatureCount";

        /// <summary>
        /// 获取相同证书序列号下的印章数量
        /// </summary>
        public const string TZ_GET_USER_SIGN_COUNT = "TZGetUserSignCount";
    }
}
