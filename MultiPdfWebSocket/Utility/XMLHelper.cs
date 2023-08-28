using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using static MultiPdfWebSocket.Utility.LogParameter;

namespace MultiPdfWebSocket.Utility
{
    public static class XMLHelper
    {
        public static void ReadXml()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "Config.xml"));
                var node = doc.SelectSingleNode("appSettings");
                LogParameter.LogLevel = (LogLevelEnum)Enum.Parse(typeof(LogLevelEnum), node.SelectSingleNode("LogLevel").InnerText);
                LogParameter.LogFilePath = node.SelectSingleNode("LogFilePath").InnerText;
                LogParameter.LogFileExistDay = int.Parse(node.SelectSingleNode("LogFileExistDay").InnerText);

                LogHelper.Debug("Configuration file read successfully.");
            }
            catch (Exception ex)
            {
                LogHelper.log.Error(string.Format("XML file read failure. {0}", ex));
            }
        }
    }
}
