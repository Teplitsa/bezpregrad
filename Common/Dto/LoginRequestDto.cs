using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common.Dto
{
    public sealed class LoginRequestDto : AbstractDto
    {
        public string Login
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public override JObject ToJson()
        {
            var obj = new JObject();

            obj.Add(new JProperty("Login", Login));
            obj.Add(new JProperty("Password", Password));

            return obj;
        }

        public override void FromJson(JObject obj)
        {
            Login = (string)obj.Property("Login").Value;
            Password = (string)obj.Property("Password").Value;
        }
    }
}