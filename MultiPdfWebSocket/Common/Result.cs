using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPdfWebSocket.Common
{
    class Result
    {
        /// <summary>
        /// 返回客户端信息的操作状态码
        /// </summary>
        public int code { get; set; }

        /// <summary>
        /// 返回客户端信息的数据
        /// </summary>
        public string data { get; set; }

        public Result()
        {
        }

        public Result(string data)
        {
            this.data = data;
        }

        public Result(int code, string data)
        {
            this.code = code;
            this.data = data;
        }

    }
}
