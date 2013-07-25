using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Common.Dto
{
    public abstract class AbstractDto
    {
        public abstract JObject ToJson();
        public abstract void FromJson(JObject obj);

        protected static JObject GetChild(JObject obj, string name)
        {
            JProperty jProperty = obj.Property(name);

            if (jProperty == null)
                return new JObject();

            return (JObject)jProperty.Value;
        }

        protected static bool GetPropertyBool(JObject obj, string name)
        {
            if (obj == null)
                return false;

            JProperty resultProperty = obj.Property(name);

            if (resultProperty == null)
                return false;

            JToken resultValue = resultProperty.Value;

            return (bool)resultValue;
        }

        protected static int GetPropertyInt(JObject obj, string name)
        {
            if (obj == null)
                return 0;

            JProperty resultProperty = obj.Property(name);

            if (resultProperty == null)
                return 0;

            JToken resultValue = resultProperty.Value;

            return (int)resultValue;
        }

        protected static DateTime GetPropertyDateTime(JObject obj, string name)
        {
            if (obj == null)
                return DateTime.UtcNow.Date;

            JProperty resultProperty = obj.Property(name);

            if (resultProperty == null)
                return DateTime.UtcNow.Date;

            JToken resultValue = resultProperty.Value;

            return (DateTime)resultValue;
        }

        protected static string GetPropertyString(JObject obj, string name)
        {
            if (obj == null)
                return string.Empty;

            JProperty resultProperty = obj.Property(name);

            if (resultProperty == null)
                return string.Empty;

            JToken resultValue = resultProperty.Value;

            return (string)resultValue;
        }
    }
}
