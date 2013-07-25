using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common.Dto
{
    public sealed class EditPointDto : AbstractDto
    {
        public EditPointDto()
        {
            Auth = new AuthDto();
            Point = new PointDto();
        }

        public AuthDto Auth { get; set; }

        public PointDto Point { get; set; }

        public override JObject ToJson()
        {
            return new JObject
                {
                    new JProperty("Auth", Auth.ToJson()),
                    new JProperty("Point", Point.ToJson())
                };
        }

        public override void FromJson(JObject obj)
        {
            Auth.FromJson((JObject) obj.Property("Auth").Value);
            Point.FromJson((JObject) obj.Property("Point").Value);
        }
    }
}