using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Common.Dto
{
    public sealed class PointPhotoDto : AbstractDto
    {
        public PointPhotoDto()
        {
            Auth = new AuthDto();
            PhotoBase64 = null;
        }

        public AuthDto Auth
        {
            get;
            set;
        }

        public int Id
        {
            get;
            set;
        }

        public string PhotoBase64
        {
            get;
            set;
        }

        public override JObject ToJson()
        {
            return new JObject(new JProperty("Auth", Auth.ToJson()), new JProperty("Id", Id), new JProperty("PhotoBase64", PhotoBase64));
        }

        public override void FromJson(JObject obj)
        {
            Auth.FromJson((JObject) obj.Property("Auth").Value);
            Id = (int) obj.Property("Id");
            PhotoBase64 = (string) obj.Property("PhotoBase64");
        }
    }
}
