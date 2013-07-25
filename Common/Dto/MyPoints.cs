using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common.Dto
{
    public sealed class MyPoints : AbstractDto
    {
        public MyPoints()
        {
            Auth = new AuthDto();
        }

        public AuthDto Auth { get; set; }


        public override JObject ToJson()
        {
            var result = new JObject
                {
                    new JProperty("Auth", Auth.ToJson())
                };

            return result;
        }

        public override void FromJson(JObject obj)
        {
            Auth.FromJson((JObject) obj.Property("Auth").Value);
        }
    }
}