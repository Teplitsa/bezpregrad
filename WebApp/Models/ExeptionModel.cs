using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using Common.Dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebApp.Models
{
    public class ExeptionModel : AbstractDto
    {
        public ExeptionModel()
        {
        }

        public Fault Fault { get; set; }

        public string ConvertToJSON(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            var element = doc.GetElementsByTagName("Message").Item(0);
            var result = JsonConvert.SerializeXmlNode(element);
            return result;
        }

        public override JObject ToJson()
        {
            return new JObject
                {
                    new JProperty("Fault", Fault.ToJson()),
                };
        }

        public override void FromJson(JObject obj)
        {
            Fault.FromJson((JObject) obj.Property("Fault").Value);
        }

        /// <summary>
        /// Вывод в лог строки
        /// </summary>
        /// <param name="message">Строка</param>
        public static void WriteToLog(string message)
        {
            using (var file = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"\Database\Log.txt", true))
            {
                lock (file)
                {
                    var d = DateTime.Now;
                    file.WriteLine(d + " " + message);
                    file.Close();
                }
            }
        }
    }

    public class Fault : AbstractDto
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public override JObject ToJson()
        {
            return new JObject
                {
                    new JProperty("Message", Message),
                    new JProperty("StackTrace", StackTrace),
                };
        }

        public override void FromJson(JObject obj)
        {
            Message = (string) obj.Property("Message").Value;
            StackTrace = (string) obj.Property("StackTrace").Value;
        }

    }
}