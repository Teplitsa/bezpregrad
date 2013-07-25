using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Common.Dto
{
    public sealed class IdDto : AbstractDto
    {
        public IdDto()
        {
            Auth = new AuthDto();
        }

        public AuthDto Auth { get; set; }

        public int Id { get; set; }

        public override JObject ToJson()
        {
            return new JObject(new JProperty("Id", Id),
                               new JProperty("Auth", Auth.ToJson()));
        }

        public override void FromJson(JObject obj)
        {
            Id = (int) obj.Property("Id");
            Auth.FromJson((JObject) obj.Property("Auth").Value);
        }
    }
}