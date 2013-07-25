using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common.Dto
{
    public sealed class AuthDto : AbstractDto
    {
        public string Token { get; set; }

        public override JObject ToJson()
        {
            var result = new JObject
                {
                    new JProperty("Token", Token)
                };

            return result;
        }

        public override void FromJson(JObject obj)
        {
            Token = (string) obj.Property("Token");
        }
    }
}