using System;
using System.IO;
using System.Net;
using Common.Dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common.Services
{
    internal static class CommonHelper
    {
        internal static void PrepareRequest<T>(T inputDto, WebRequest request)
            where T : AbstractDto
        {
            if (Equals(inputDto, null))
                throw new ArgumentNullException("inputDto");

            request.Method = "POST";
            //request.ContentType = "application/json";


            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(inputDto.ToJson());
            }
        }

        public static T ProcessRequest<T>(WebRequest request)
            where T : AbstractDto, new()
        {
            WebResponse response = request.GetResponse();

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var result = new T();

                result.FromJson(JObject.Parse(reader.ReadToEnd()));

                return result;
            }

        }
    }
}