using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Common.Dto.PointEntry
{
    public sealed class PointPart7Summary : AbstractDto
    {
        /// <summary>
        /// Отметьте фактическую доступность объекта для инвалидов на кресле-коляске и с поражением опорно-двигательного аппарата
        /// </summary>
        public bool InvalidMoveAvailability { get; set; }

        /// <summary>
        /// Отметьте фактическую доступность объекта для инвалидов по зрению
        /// </summary>
        public bool InvalidEyeAvailability { get; set; }

        /// <summary>
        /// Отметьте фактическую доступность объекта для инвалидов с нарушениями слуха
        /// </summary>
        public bool InvalidHearingAvailability { get; set; }

        /// <summary>
        /// Иные комментарии и замечания по объекту
        /// </summary>
        public string OtherComments { get; set; }

        /// <summary>
        /// Дата составления протокола
        /// </summary>
        public DateTime ReportDate { get; set; }


        public override JObject ToJson()
        {
            return new JObject(
                new JProperty("InvalidMoveAvailability", InvalidMoveAvailability),
                new JProperty("InvalidEyeAvailability", InvalidEyeAvailability),
                new JProperty("InvalidHearingAvailability", InvalidHearingAvailability),
                new JProperty("OtherComments", OtherComments),
                new JProperty("ReportDate", ReportDate)
                );
        }

        public override void FromJson(JObject obj)
        {
            InvalidMoveAvailability = GetPropertyBool(obj, "InvalidMoveAvailability");
            InvalidEyeAvailability = GetPropertyBool(obj, "InvalidEyeAvailability");
            InvalidHearingAvailability = GetPropertyBool(obj, "InvalidHearingAvailability");
            OtherComments = GetPropertyString(obj, "OtherComments");
            ReportDate = GetPropertyDateTime(obj, "ReportDate");
        }
    }
}