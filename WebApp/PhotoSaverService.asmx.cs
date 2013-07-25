using System;
using System.ComponentModel;
using System.IO;
using System.Web.Script.Services;
using System.Web.Services;
using Common.Dto;
using WebApp.Models;

namespace WebApp
{

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class PhotoSaverService : WebService
    {
        [WebMethod]
        public void SavePhoto()
        {
#if DEBUG
            ExeptionModel.WriteToLog("Вошли в SavePhoto()");
#endif
            var lenghth = (int) Context.Request.InputStream.Length;
            var imageData = new byte[lenghth];
            Context.Request.InputStream.Read(imageData, 0, lenghth);
            var pathToServer = AppDomain.CurrentDomain.BaseDirectory;
            var guid = Guid.NewGuid();
            var fileName = string.Format(GlobalContants.FilePathFormat, pathToServer, guid);
            while (File.Exists(fileName))
            {
                guid = Guid.NewGuid();
                fileName = string.Format(GlobalContants.FilePathFormat, pathToServer, guid);
            }
            File.WriteAllBytes(string.Format(GlobalContants.FilePathFormat, pathToServer, guid), imageData);
            Context.Response.Write(guid.ToString());
        }
    }
}