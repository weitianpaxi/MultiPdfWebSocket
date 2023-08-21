using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPdfWebSocket.Common
{
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
    }
}
