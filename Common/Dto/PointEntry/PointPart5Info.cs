using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Common.Dto.PointEntry
{
    public sealed class PointPart5Info : AbstractDto
    {
        /// <summary>
        /// На объекте размещена информация, указатели об услугах, назначении функциональных зон, путях эвакуации и т.п.
        /// </summary>
        public bool Evacuation { get; set; }

        /// <summary>
        /// Информация о назначении помещений, залов расположена не выше 1,6 м от пола
        /// </summary>
        public bool InfoWeight { get; set; }

        /// <summary>
        /// Информация на табличках о помещениях дублирована рельефными знаками
        /// </summary>
        public bool InfoSigns { get; set; }

        /// <summary>
        /// Есть световые текстовые табло для вывода оперативной информации
        /// </summary>
        public bool InfoLuminance { get; set; }

        /// <summary>
        /// Звуковые информаторы
        /// </summary>
        public bool InfoVoice { get; set; }

        public override JObject ToJson()
        {
            return new JObject(
                new JProperty("Evacuation", Evacuation),
                new JProperty("InfoWeight", InfoWeight),
                new JProperty("InfoSigns", InfoSigns),
                new JProperty("InfoLuminance", InfoLuminance),
                new JProperty("InfoVoice", InfoVoice)
                );
        }

        public override void FromJson(JObject obj)
        {
            Evacuation = GetPropertyBool(obj, "Evacuation");
            InfoWeight = GetPropertyBool(obj, "InfoWeight");
            InfoSigns = GetPropertyBool(obj, "InfoSigns");
            InfoLuminance = GetPropertyBool(obj, "InfoLuminance");
            InfoVoice = GetPropertyBool(obj, "InfoVoice");
        }
    }
}