using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPdfWebSocket.Common
{
    /// <summary>
    /// 响应消息定义
    /// </summary>
    class Response
    {
        /// <summary>
        /// 执行的方法
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 执行方法的结果
        /// </summary>
        public Result Result { get; set; }

        public Response(string method, Result result)
        {
            this.Method = method;
            this.Result = result;
        }
    }
}
