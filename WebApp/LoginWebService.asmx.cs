using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.ServiceModel.Dispatcher;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml;
using Common.Dto;
using WebApp.Database;
using WebApp.Database.Entities;

namespace WebApp
{
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class LoginWebService : WebService
    {
        [WebMethod]
        public void Login()
        {
            var input = ResponseOperator.ConvertStringToValue<LoginRequestDto>(Context);

/*
 * var v = new AuthDto
            {
                Token = 1.ToString(CultureInfo.InvariantCulture)
            };
            ResponseOperator.ConvertValueToString(v, Context);

            return;
*/

            using (var model = new DatabaseModel())
            {
                AuthDto value;
                var response =
                    string.Format(
                        "http://www.streetjournal.org/api2/validateuser?" +
                        "user={0}&pswd={1}&",
                        input.Login, input.Password);
                var myHttpWebRequest = (HttpWebRequest) WebRequest.Create(response);
                var myHttpWebResponse = (HttpWebResponse) myHttpWebRequest.GetResponse();
                var myStreamReader = new StreamReader(myHttpWebResponse.GetResponseStream());
                var result = myStreamReader.ReadToEnd();
                if (result.Contains("Fault"))
                {
                    //#if DEBUG
                    //                    if (!model.UsersSet.Any())
                    //                    {
                    //                        model.UsersSet.Add(new DbUser
                    //                            {
                    //                                Login = "admin",
                    //                                Password = "123"
                    //                            });

                    //                        model.SaveChanges();
                    //                    }
                    //#endif
                    value = new AuthDto
                        {
                            Token = null
                        };
                    ResponseOperator.ConvertValueToString(value, Context);
                }
                else
                {
                    var name = input.Login.ToUpperInvariant();
                    var pass = input.Password;

                    var user = model.UsersSet.FirstOrDefault(u => u.Login == name);
                    if (user == null)
                    {
                        user = model.UsersSet.Add(new DbUser
                            {
                                Login = name,
                                Password = pass
                            });

                        model.SaveChanges();
                    }
                    else
                    {
                        if (user.Password != pass)
                        {
                            user.Password = pass;
                            model.SaveChanges();
                        }
                    }
                    value = new AuthDto
                        {
                            Token = user.Id.ToString(CultureInfo.InvariantCulture)
                        };
                    ResponseOperator.ConvertValueToString(value, Context);
                }
            }
        }
    }
}