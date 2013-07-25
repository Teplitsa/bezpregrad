using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common.Dto
{
    public sealed class StringResultDto : AbstractDto
    {
        public StringResultDto(string val)
        {
            Result = val;
        }

        public StringResultDto()
            : this(null)
        {
        }

        public string Result { get; set; }

        /// <summary>
        /// Id объекта, с которым завершена операция
        /// </summary>
        public int ResultObjectId { get; set; }

        public override JObject ToJson()
        {
            return new JObject
                {
                    new JProperty("Result", Result),
                    new JProperty("ResultObjectId", ResultObjectId),
                };
        }

        public override void FromJson(JObject obj)
        {
            Result = (string) obj.Property("Result").Value;
            ResultObjectId = (int) obj.Property("ResultObjectId").Value;
        }
    }
}