using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPdfWebSocket.Common
{
    /// <summary>
    /// 事件消息常量定义
    /// </summary>
    class EventMessageConstant
    {
        /// <summary>
        /// 打开文件完成事件
        /// </summary>
        public const string AFTER_OPEN_FILE = "Event_AfterOpenFile";

        /// <summary>
        /// 签章完成事件（只适用于工具栏中的电子签章按钮执行后事件）
        /// </summary>
        public const string AFTER_SIGN_PDF = "Event_AfterSignPDF";

        /// <summary>
        /// 删除签章完成事件
        /// </summary>
        public const string AFTER_DEL_SIGNATURE = "Event_AfterDelSignature";

        /// <summary>
        /// 点击电子签章按钮事件
        /// </summary>
        public const string BUTTONED_SIGN = "Event_ButtonedSign";

        /// <summary>
        /// 电子签章按钮执行失败事件
        /// </summary>
        public const string BUTTONED_SIGN_ERR = "Event_ButtonedSignErr";

        /// <summary>
        /// 打字机模式时点击"对号"事件
        /// </summary>
        public const string ON_SAVE_COMMENT = "Event_OnSaveComment";

        /// <summary>
        /// 印章拖动时单击鼠标确认事件
        /// </summary>
        public const string ON_MOVE_CACHE_SIG_COMPLETE = "Event_OnMoveCacheSigComplete";
    }
}
