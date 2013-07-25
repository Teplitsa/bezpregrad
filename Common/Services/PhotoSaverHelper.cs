using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Common.Dto;

namespace Common.Services
{
    public static class PhotoSaverHelper
    {
        //PhotoSaverService

        public static string SavePhoto(byte[] photoData)
        {
            WebRequest request = WebRequest.Create(GlobalContants.ServerPath + "PhotoSaverService.asmx/SavePhoto");

            request.Method = "POST";

            using (var writer = new BinaryWriter(request.GetRequestStream()))
            {
                writer.Write(photoData);
            }

            using (WebResponse response = request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }


        public static byte[] LoadPhoto(string photoName)
        {
            string url = string.Format(GlobalContants.FilePathFormat, GlobalContants.ServerPath, photoName);

            using (var client = new WebClient())
            {
                return client.DownloadData(url);
            }
        }
    }
}
