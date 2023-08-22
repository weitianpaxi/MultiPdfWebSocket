using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPdfWebSocket.Common
{
    /// <summary>
    /// 配置类常量定义
    /// </summary>
    class ProfileConstant
    {
        /// <summary>
        /// 服务监听地址及端口号
        /// </summary>
        public const string LISTENER_URL = "http://localhost:10055/";

        /// <summary>
        /// 收到响应体的大小
        /// </summary>
        public const int REQUEST_SIZE = 4096;

        /// <summary>
        /// OCX控件的rc路径
        /// </summary>
        public const string RC_PATH = "c:\\seal\\snca\\rc";

        /// <summary>
        /// 设置签章厂商，0为陕西ca，110为北京ca
        /// </summary>
        public const int CA_TYPE = 0;

        /// <summary>
        /// 窗体默认坐标X轴位置
        /// </summary>
        public const int DEFAULT_POS_X = 0;

        /// <summary>
        /// 窗体默认坐标Y轴位置
        /// </summary>
        public const int DEFAULT_POS_Y = 0;

        /// <summary>
        /// 窗体默认宽度
        /// </summary>
        public const int DEFAULT_WIDTH = 1300;

        /// <summary>
        /// 窗体默认高度
        /// </summary>
        public const int DEFAULT_HEIGHT = 650;


        /// <summary>
        /// 客户端请求JSON中执行方法key值
        /// </summary>
        public const string METHOD = "Method";

        /// <summary>
        /// 客户端请求JSON中执行方法所需参数key值
        /// </summary>
        public const string PARAMETER = "Parameter";
    }
}
