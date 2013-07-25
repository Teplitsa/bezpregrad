using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Authentication;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml;
using Common.Dto;
using Common.Dto.PointEntry;
using Newtonsoft.Json.Linq;
using WebApp.Database;
using WebApp.Database.Entities;
using WebApp.Models;

namespace WebApp
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class PointManager : WebService
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void MyPoints()
        {
#if DEBUG
            ExeptionModel.WriteToLog("Вошли в MyPoints()");
#endif
            //using (var reader = new StreamReader(Context.Request.InputStream))
            //{
            //    var res = reader.ReadToEnd();
            //    ExeptionModel.WriteToLog(res);
            //    return;
            //}
            var result = new PointsSetDto();
            try
            {
                var input = ResponseOperator.ConvertStringToValue<AuthDto>(Context);
#if DEBUG
                ExeptionModel.WriteToLog("Распарсили объект:");
                ExeptionModel.WriteToLog(input.ToJson().ToString());
#endif
                var userId = input.Token;
                if (string.IsNullOrEmpty(userId))
                    throw new AuthenticationException("Ошибка авторизации");
                DbUser user;
                using (var db = new DatabaseModel())
                {
                    var id = int.Parse(userId);
                    user = db.UsersSet.FirstOrDefault(x => x.Id == id);
                }
                var response = new StringBuilder("http://www.streetjournal.org/api2/issuelist?");
                response.Append("user=" + user.Login + "&pswd=" + user.Password);
                response.Append("&latitude=" + 58.004785.ToString(CultureInfo.InvariantCulture) + "&longitude=" +
                                56.237654.ToString(CultureInfo.InvariantCulture));
                response.Append("&radius=" + 10000);
                response.Append("&isself=true");

#if DEBUG
                ExeptionModel.WriteToLog("response:" + response);
#endif
                var myHttpWebRequest = (HttpWebRequest) WebRequest.Create(response.ToString());
                var myHttpWebResponse = (HttpWebResponse) myHttpWebRequest.GetResponse();
                var myStreamReader = new StreamReader(myHttpWebResponse.GetResponseStream());
                var str = myStreamReader.ReadToEnd();
                var doc = new XmlDocument();
                doc.LoadXml(str);
                if (str.Contains("Fault"))
                {
                    result.Message = doc.GetElementsByTagName("Fault")[0].FirstChild.Value;
                    //    var exept = new ExeptionModel();
                    //    var js = exept.ConvertToJSON(str);
                    //    //var jsonstring = exept.ConvertToJSON(str);
                    //    exept.FromJson(JObject.Parse(js));
                    //    result.Result = exept.Fault.Message;
                }
                else
                {
                    result = new PointsSetDto();
                    var a = new List<PointDto>();
                    foreach (XmlNode node in doc.GetElementsByTagName("Item"))
                    {
                        //if (bool.Parse(node.Attributes["isSelf"].Value))
                        {
                            string str1 = null;
                            if (node.FirstChild.HasChildNodes)
                            {
                                var s = node.FirstChild.FirstChild.Attributes["Value"].Value;
                                var myHttpWebRequest1 = (HttpWebRequest) WebRequest.Create(s);
                                var myHttpWebResponse1 = (HttpWebResponse) myHttpWebRequest1.GetResponse();
                                var myStreamReader1 = new StreamReader(myHttpWebResponse1.GetResponseStream());
                                str1 = myStreamReader1.ReadToEnd();

                            }
                            a.Add(new PointDto
                                {
                                    Id = int.Parse(node.Attributes["Id"].Value),
                                    Name = node.Attributes["Name"].Value,
                                    Category = 0,
                                    PointDataDto = new PointDataDto
                                        {
                                            Address = node.Attributes["Address"].Value,
                                            Latitude =
                                                double.Parse(node.Attributes["Lat"].Value,
                                                             CultureInfo.InvariantCulture),
                                            Longitude =
                                                double.Parse(node.Attributes["Lng"].Value,
                                                             CultureInfo.InvariantCulture),
                                            PhotoName = !string.IsNullOrEmpty(str1) ? str1 : null
                                        }
                                });
                        }
                    }
                    result.Points = a.ToArray();
                }
                //result = new PointsSetDto
                //    {
                //        Points = user.Points.Select(p => p.ToDto()).ToArray()
                //    };
                //}
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                ExeptionModel.WriteToLog(ex.Message + " " + ex.StackTrace);
            }
#if DEBUG
            ExeptionModel.WriteToLog(result.ToJson().ToString());
#endif
            ResponseOperator.ConvertValueToString(result, Context);
        }

        /// <summary>
        /// Возвратим текст ошибки, если всё плохо
        /// </summary>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void NewPoint()
        {
#if DEBUG
            ExeptionModel.WriteToLog("Вошли в NewPoint()");
#endif
            PointCreateEdit(true);
        }

        /// <summary>
        /// Возвратим текст ошибки, если всё плохо
        /// </summary>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void EditPoint()
        {
#if DEBUG
            ExeptionModel.WriteToLog("Вошли в EditPoint()");
#endif
            PointCreateEdit(false);
        }

        /// <summary>
        /// Возвратим текст ошибки, если всё плохо
        /// </summary>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void RemovePoint()
        {
            var input = ResponseOperator.ConvertStringToValue<EditPointDto>(Context);

            var result = new StringResultDto();

            int userId = InternalSessionManager.GetUserId(input.Auth.Token);

            if (userId <= 0)
            {
                result = new StringResultDto("Ошибка авторизации");
            }
            else
            {
                try
                {
                    using (var db = new DatabaseModel())
                    {
                        DbUser user = db.UsersSet.Find(userId);
                        int pointId = input.Point.Id;

                        DbPoint oldPoint = user.Points.FirstOrDefault(p => p.Id == pointId);
                        //Именно тут, чтобы не удалить чужое

                        if (oldPoint == null)
                        {
                            result = new StringResultDto("Редактируемая точка не найден");
                        }
                        else
                        {
                            user.Points.Remove(oldPoint);

                            db.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = new StringResultDto(ex.Message);
                }
            }

            ResponseOperator.ConvertValueToString(result, Context);
        }


        /// <summary>
        /// Возвращаем фото в Base64.
        /// Если в result id == 0 - то в тексте будет ошибка
        /// </summary>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void LoadPhoto()
        {
#if DEBUG
            ExeptionModel.WriteToLog("Зашли в LoadPhoto()");
#endif
            var result = new StringResultDto();
            ResponseOperator.ConvertValueToString(result, Context);
            //PhotoSaveLoad(false);

            //var input = ResponseOperator.ConvertStringToValue<IdDto>(Context);

            //var result = new StringResultDto();

            //int userId = InternalSessionManager.GetUserId(input.Auth.Token);

            //if (userId <= 0)
            //{
            //    result = new StringResultDto("Ошибка авторизации");
            //}
            //else
            //{
            //    try
            //    {
            //        using (var db = new DatabaseModel())
            //        {
            //            DbUser user = db.UsersSet.Find(userId);

            //            int pointId = input.Id;

            //            DbPoint oldPoint = user.Points.FirstOrDefault(p => p.Id == pointId);
            //            //Именно тут, чтобы не удалить чужое

            //            if (oldPoint == null)
            //            {
            //                result = new StringResultDto("Редактируемая точка не найден");
            //            }
            //            else
            //            {
            //                var photosDir = GetPhotosDir();

            //                if (!Directory.Exists(photosDir))
            //                {
            //                    result.Result = "Отсутствует дирректория для фото";
            //                }
            //                else
            //                {
            //                    string fileName = Path.Combine(photosDir, input.Id + ".jpg");

            //                    if (File.Exists(fileName))
            //                        result.Result = Convert.ToBase64String(File.ReadAllBytes(fileName));

            //                    result.ResultObjectId = pointId;
            //                }
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        result = new StringResultDto(ex.Message);
            //    }
            //}

            //ResponseOperator.ConvertValueToString(result, Context);
        }



        /// <summary>
        /// Возвратим текст ошибки, если всё плохо
        /// </summary>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public void SavePhoto()
        {
#if DEBUG
            ExeptionModel.WriteToLog("Зашли в SavePhoto()");
#endif
            var result = new StringResultDto();
            ResponseOperator.ConvertValueToString(result, Context);
            //PhotoSaveLoad(true);
        }

        /// <summary>
        /// Сохранение или получение фотки с сервера сервиса
        /// </summary>
        /// <param name="isSave">True - SavePhoto, False - LoadPhoto</param>
        private void PhotoSaveLoad(bool isSave)
        {
            var result = new StringResultDto();
            try
            {
                var input = ResponseOperator.ConvertStringToValue<PointPhotoDto>(Context);
#if DEBUG
                ExeptionModel.WriteToLog("Распарсили объект:");
                ExeptionModel.WriteToLog(input.ToJson().ToString());
#endif
                var userId = input.Auth.Token;
                if (string.IsNullOrEmpty(userId))
                {
                    throw new AuthenticationException("Ошибка авторизации");
                }
                try
                {
                    using (var db = new DatabaseModel())
                    {
                        var id = int.Parse(userId);
                        var user = db.UsersSet.FirstOrDefault(x => x.Id == id);
                        var pointId = input.Id;
                        if (user == null)
                        {
                            throw new AuthenticationException("Ошибка авторизации");
                        }
                        //var oldPoint = user.Points.FirstOrDefault(p => p.Id == pointId);
                        ////Именно тут, чтобы не удалить чужое

                        //if (oldPoint == null)
                        //{
                        //    result = new StringResultDto("Редактируемая точка не найдена");
                        //}
                        //else
                        //{
                        var photosDir = GetPhotosDir();

                        if (!Directory.Exists(photosDir))
                        {
                            result.Result = "Отсутствует дирректория для фото";
#if DEBUG
                            ExeptionModel.WriteToLog("Отсутствует дирректория для фото");
#endif
                        }
                        else
                        {
                            var fileName = Path.Combine(photosDir, input.Id + ".jpg");
                            if (isSave)
                            {
                                File.WriteAllBytes(fileName, Convert.FromBase64String(input.PhotoBase64));
#if DEBUG
                                ExeptionModel.WriteToLog("Создали фото: " + fileName);
#endif

                            }
                            else
                            {
                                if (File.Exists(fileName))
                                {
                                    result.Result = Convert.ToBase64String(File.ReadAllBytes(fileName));
                                    result.ResultObjectId = pointId;
                                }
                                else
                                {
                                    result.Result = "Фото не найдено";
                                    result.ResultObjectId = 0;
                                }
#if DEBUG
                                ExeptionModel.WriteToLog("Вернули фото: " + fileName);
#endif
                            }
                        }

                        /*oldPoint.Photo = Convert.FromBase64String(input.PhotoBase64);

                            db.SaveChanges();*/
                        //}
                    }
                }
                catch (Exception ex)
                {
                    result = new StringResultDto(ex.Message);
                    ExeptionModel.WriteToLog("Token:" + input.Auth.Token + " request:    " + input.ToJson());
                    ExeptionModel.WriteToLog("Token:" + input.Auth.Token + "  " + ex.Message + " " + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                result = new StringResultDto(ex.Message);
                ExeptionModel.WriteToLog(ex.Message + " " + ex.StackTrace);
            }
#if DEBUG
            ExeptionModel.WriteToLog(result.ToJson().ToString());
#endif
            ResponseOperator.ConvertValueToString(result, Context);
        }

        /// <summary>
        /// Возвращает путь до каталога с фотографиями
        /// </summary>
        /// <returns></returns>
        private static string GetPhotosDir()
        {
            var parentDir = AppDomain.CurrentDomain.BaseDirectory;

            return Path.Combine(parentDir, "Photos");
        }

        public static Bitmap ResizePhoto(Bitmap inStream)
        {
            var originalBitmap = new Bitmap(inStream);
            var ratioWidthToHeight = originalBitmap.Width/(double) originalBitmap.Height;
            int newHeight;
            int newWidth;
            if (ratioWidthToHeight > 1)
            {
                newWidth = 200;
                newHeight = (int) (200/ratioWidthToHeight);
            }
            else
            {
                newHeight = 200;
                newWidth = (int) (200*ratioWidthToHeight);
            }
            var newBitmap = new Bitmap(newWidth, newHeight);
            var graphic = Graphics.FromImage(newBitmap);
            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.DrawImage(originalBitmap, 0, 0, newBitmap.Width, newBitmap.Height);

            //newBitmap.SetResolution(resolutionDPI, resolutionDPI);
            return newBitmap;
        }

        /// <summary>
        /// Метод для создания или редактирования точки
        /// </summary>
        /// <param name="isNew">Если True, то новая точка</param>
        private void PointCreateEdit(bool isNew)
        {
            var logString = new StringBuilder();
            var result = new StringResultDto();
            try
            {
                var input = ResponseOperator.ConvertStringToValue<EditPointDto>(Context);
#if DEBUG
                logString.Append(DateTime.Now + " " + "Распарсили объект:\r\n");
                logString.Append(DateTime.Now + " " + input.ToJson().ToString() + "\r\n");
                //ExeptionModel.WriteToLog("Распарсили объект:");
                //ExeptionModel.WriteToLog(input.ToJson().ToString());
#endif
                var userId = input.Auth.Token;
                if (string.IsNullOrEmpty(userId))
                {
                    throw new AuthenticationException("Ошибка авторизации");
                }
                try
                {
                    DbUser user;
                    using (var db = new DatabaseModel())
                    {
                        var id = int.Parse(userId);
                        user = db.UsersSet.FirstOrDefault(x => x.Id == id);
                    }
                    if (user == null)
                    {
                        throw new AuthenticationException("Ошибка авторизации");
                    }
                    //var newPoint = db.PointsSet.Create();
                    //newPoint.Update(input.Point);
                    //user.Points.Add(newPoint);
                    //db.SaveChanges();
                    var poss = new[]
                        {
                            input.Point.PointDataDto.Longitude.ToString(CultureInfo.InvariantCulture),
                            input.Point.PointDataDto.Latitude.ToString(CultureInfo.InvariantCulture)
                        };
                    var a = new Anketa();
                    var text = a.GetResult(input.Point.PointDataDto);
                    text = input.Point.PointDataDto.PointPart7Summary.OtherComments + "\n\n" + text;
#if DEBUG
                    logString.Append(DateTime.Now + " " + "Посчитали коэффициенты" + "\r\n");
                    //ExeptionModel.WriteToLog("Посчитали коэффициенты");
#endif
                    if (!string.IsNullOrEmpty(input.Point.PointDataDto.Address) &&
                        (poss[0] == "0" || poss[1] == "0"))
                    {
#if DEBUG
                        logString.Append(DateTime.Now + " " + "Координаты яндекса" + "\r\n");
                        //ExeptionModel.WriteToLog("Координаты яндекса");
#endif
                        var coor = "http://geocode-maps.yandex.ru/1.x/?format=json&geocode=" +
                                   input.Point.PointDataDto.Address;
                        var requestToYa = (HttpWebRequest) WebRequest.Create(coor);
                        var responseFromYa = (HttpWebResponse) requestToYa.GetResponse();
                        var streamReader = new StreamReader(responseFromYa.GetResponseStream());
                        coor = streamReader.ReadToEnd();
                        var o = JObject.Parse(coor);
                        var pos =
                            (string)
                            o["response"]["GeoObjectCollection"]["featureMember"][0]["GeoObject"]["Point"]["pos"
                                ];
                        poss = pos.Split(' ');
#if DEBUG
                        logString.Append(DateTime.Now + " " + "Яндекс посчитался" + "\r\n");
                        //ExeptionModel.WriteToLog("Яндекс посчитался");
#endif
                    }
                    var response = new StringBuilder("http://www.streetjournal.org/api2/");
                    response.Append(isNew ? "addissue?" : "editissue?");
                    response.Append("user=" + user.Login + "&pswd=" + user.Password);
                    if (!isNew)
                        response.Append("&id=" + input.Point.Id);
                    response.Append("&latitude=" + poss[1] + "&longitude=" + poss[0]);
                    response.Append("&name=" + input.Point.Name);
                    response.Append("&address=" + input.Point.PointDataDto.Address);
                    response.Append("&text=" + text);
                    response.Append("&anonymous=false");
                    response.Append("&categorymain=" + 176 /*input.Point.Category*/);
#warning закоменчена отправка дполнительной категории на АПИ.
                    //response.Append("&categoryadd=" + input.Point.Category);

#if DEBUG
                    logString.Append(DateTime.Now + " " + "Ищем фотографию" + "\r\n");
                    //ExeptionModel.WriteToLog("Ищем фотографию");
#endif
                    if (!string.IsNullOrEmpty(input.Point.PointDataDto.PhotoName))
                    {
                        var pathToServer = AppDomain.CurrentDomain.BaseDirectory;
                        var fileName = String.Format(GlobalContants.FilePathFormat, pathToServer,
                                                     input.Point.PointDataDto.PhotoName);
                        if (File.Exists(fileName))
                        {
#if DEBUG
                            logString.Append(DateTime.Now + " " + "Нашли фотографию" + "\r\n");
                            //ExeptionModel.WriteToLog("Нашли фотографию");
#endif
                            var image = new Bitmap(fileName);
#if DEBUG
                            logString.Append(DateTime.Now + " " + "Ресайз фотографии" + "\r\n");
                            //ExeptionModel.WriteToLog("Ресайз фотографии");
#endif
                            image = ResizePhoto(image);
                            using (var ms = new MemoryStream())
                            {
#if DEBUG
                                logString.Append(DateTime.Now + " " + "Конвертация в Base64" + "\r\n");
                                //ExeptionModel.WriteToLog("Конвертация в Base64");
#endif
                                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                var imageBytes = ms.ToArray();
                                var photoStr = Convert.ToBase64String(imageBytes);

#warning закоменчена отправка фотографии на АПИ.
                                //response.Append(!string.IsNullOrEmpty(photoStr)
                                //                    ? "&photo=" + photoStr
                                //                    : string.Empty);
                            }
                        }
                    }
#if DEBUG
                    logString.Append(DateTime.Now + " " + "response:" + response + "\r\n");
                    //ExeptionModel.WriteToLog("response:" + response);
#endif
                    var myHttpWebRequest = (HttpWebRequest) WebRequest.Create(response.ToString());
                    var myHttpWebResponse = (HttpWebResponse) myHttpWebRequest.GetResponse();
                    var myStreamReader = new StreamReader(myHttpWebResponse.GetResponseStream());
                    var str = myStreamReader.ReadToEnd();
                    var doc = new XmlDocument();
                    doc.LoadXml(str);
                    if (str.Contains("Fault"))
                    {
                        result.Result = doc.GetElementsByTagName("Fault")[0].FirstChild.Value;
                    }
                    else
                    {
                        result.Result = "Ваша информация занесена на карту";
                        result.ResultObjectId =
                            int.Parse(doc.GetElementsByTagName("Issue")[0].Attributes["Id"].Value);
                    }
                }
                catch (Exception ex)
                {
                    result = new StringResultDto(ex.Message);
                    logString.Append(DateTime.Now + " " + "Token:" + input.Auth.Token + " request:    " + input.ToJson() +
                                     "\r\n");
                    logString.Append(DateTime.Now + " " + "Token:" + input.Auth.Token + "  " + ex.Message + " " +
                                     ex.StackTrace + "\r\n");
                    ExeptionModel.WriteToLog("Token:" + input.Auth.Token + " request:    " + input.ToJson());
                    ExeptionModel.WriteToLog("Token:" + input.Auth.Token + "  " + ex.Message + " " + ex.StackTrace);
                }
            }
            catch (Exception ex)
            {
                result = new StringResultDto(ex.Message);
                logString.Append(DateTime.Now + " " + ex.Message + " " + ex.StackTrace + "\r\n");
                ExeptionModel.WriteToLog(ex.Message + " " + ex.StackTrace);
            }
#if DEBUG
            logString.Append(DateTime.Now + " " + result.ToJson().ToString() + "\r\n");
            ExeptionModel.WriteToLog(logString.ToString());
            //ExeptionModel.WriteToLog(result.ToJson().ToString());
#endif
            ResponseOperator.ConvertValueToString(result, Context);
        }
    }
}