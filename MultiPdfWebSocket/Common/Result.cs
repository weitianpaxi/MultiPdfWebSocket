using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiPdfWebSocket.Common
{
    /// <summary>
    /// 操作结果返回消息定义，统一数据格式定义
    /// </summary>
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

        /// <summary>
        /// 一些默认无返回值的操作成功的提醒
        /// </summary>
        /// <param name="message">提醒消息</param>
        /// <returns></returns>
        public static Result Success(string message)
        {
            Result result = new Result
            {
                code = 0,
                data = message
            };
            return result;
        }

        /// <summary>
        /// 一些默认无返回值的操作失败的提醒
        /// </summary>
        /// <param name="message">提醒消息</param>
        /// <returns></returns>
        public static Result Error(string message)
        {
            Result result = new Result
            {
                code = 1,
                data = message
            };
            return result;
        }
    }
}
