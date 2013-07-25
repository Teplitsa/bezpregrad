using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Dispatcher;
using System.Web;
using Common.Dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebApp
{
    internal static class ResponseOperator
    {
        internal static void ConvertValueToString<T>(T value, HttpContext context)
            where T : AbstractDto
        {
            string outText = value.ToJson().ToString();

            context.Response.Write(outText);

            context.Response.ContentType = "application/json";
        }

        internal static T ConvertStringToValue<T>(HttpContext context)
            where T : AbstractDto, new()
        {
            using (var reader = new StreamReader(context.Request.InputStream))
            {
                var result = new T();

                result.FromJson(JObject.Parse(reader.ReadToEnd()));

                return result;
            }
        }
    }
}