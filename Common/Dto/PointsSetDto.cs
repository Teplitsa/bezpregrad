using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common.Dto
{
    public sealed class PointsSetDto : AbstractDto
    {
        public string Message { get; set; }

        public PointDto[] Points { get; set; }

        public override JObject ToJson()
        {
            var jArray = new JArray();
            foreach (PointDto point in Points)
            {
                jArray.Add(point.ToJson());
            }
            return new JObject
                {
                    new JProperty("Points", jArray),
                    new JProperty("Message", Message)
                };
        }

        public override void FromJson(JObject obj)
        {
            JProperty jProperty = obj.Property("Points");
            var val = (JArray) jProperty.Value;
            var ps = new List<PointDto>();
            foreach (JObject jObj in val)
            {
                var point = new PointDto();
                point.FromJson(jObj);
                ps.Add(point);
            }
            Points = ps.ToArray();
            Message = (string) obj.Property("Message").Value;
        }
    }
}