using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using static MultiPdfWebSocket.Utility.Parameter;

namespace MultiPdfWebSocket.Utility
{
    public static class XMLHelper
    {
        public static void ReadXml()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "XML", "Config.xml"));
                var node = doc.SelectSingleNode("appSettings");
                Parameter.LogLevel = (LogLevelEnum)Enum.Parse(typeof(LogLevelEnum), node.SelectSingleNode("LogLevel").InnerText);
                Parameter.LogFilePath = node.SelectSingleNode("LogFilePath").InnerText;
                Parameter.LogFileExistDay = int.Parse(node.SelectSingleNode("LogFileExistDay").InnerText);

                LogHelper.Debug("XML文件读取成功。");
            }
            catch (Exception ex)
            {
                LogHelper.log.Error(string.Format("XML文件读取失败。{0}", ex));
            }
        }
    }
}
