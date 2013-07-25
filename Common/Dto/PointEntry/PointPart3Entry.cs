using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Common.Dto.PointEntry
{
    public sealed class PointPart3Entry : AbstractDto
    {
        /// <summary>
        /// Пандус не требуется, вход вровень с улицей
        /// </summary>
        public bool RampNotNeeded { get; set; }

        /// <summary>
        /// Пандуса нет, но он нужен
        /// </summary>
        public bool RampNeeded { get; set; }

        /// <summary>
        /// Пандус соответствует требованиям
        /// </summary>
        public bool RampType { get; set; }

        /// <summary>
        /// Завершающие части поручня длинее наклонной части на 30см
        /// </summary>
        public bool Handle { get; set; }

        /// <summary>
        /// Входной проем доступен для инвалидов
        /// </summary>
        public bool InvalidEntry { get; set; }

        /// <summary>
        /// Над дверной площадкой оборудован водоотвод
        /// </summary>
        public bool Awning { get; set; }

        /// <summary>
        /// Если двери прозрачные они оборудованы яркой контрастной маркировкой
        /// </summary>
        public bool DoorMarks { get; set; }

        /// <summary>
        /// Ширина дверных и открытых проемов не менее 90см
        /// </summary>
        public bool DoorWidth { get; set; }

        /// <summary>
        /// Высота порога не более 0,025м
        /// </summary>
        public bool Threshold { get; set; }

        public override JObject ToJson()
        {
            return new JObject(
                new JProperty("RampNotNeeded", RampNotNeeded),
                new JProperty("RampNeeded", RampNeeded),
                new JProperty("RampType", RampType),
                new JProperty("Handle", Handle),
                new JProperty("InvalidEntry", InvalidEntry),
                new JProperty("Awning", Awning),
                new JProperty("DoorMarks", DoorMarks),
                new JProperty("DoorWidth", DoorWidth),
                new JProperty("Threshold", Threshold)
                );
        }

        public override void FromJson(JObject obj)
        {
            RampNotNeeded = GetPropertyBool(obj, "RampNotNeeded");
            RampNeeded = GetPropertyBool(obj, "RampNeeded");
            RampType = GetPropertyBool(obj, "RampType");
            Handle = GetPropertyBool(obj, "Handle");
            InvalidEntry = GetPropertyBool(obj, "InvalidEntry");
            Awning = GetPropertyBool(obj, "Awning");
            DoorMarks = GetPropertyBool(obj, "DoorMarks");
            DoorWidth = GetPropertyBool(obj, "DoorWidth");
            Threshold = GetPropertyBool(obj, "Threshold");
        }
    }
}