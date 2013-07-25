using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Common.Dto.PointEntry
{
    public sealed class PointPart2Invalid : AbstractDto
    {
        /// <summary>
        /// Доступны ли подъезды к объекту для инвалидов-колясочников
        /// </summary>
        public bool DriveAvailable { get; set; }

        /// <summary>
        /// Наличие тактильных средств для инвалидов по зрению на покрытии пешеходных путей
        /// </summary>
        public bool Pathes { get; set; }

        /// <summary>
        /// Есть ли возле объекта автопарковка
        /// </summary>
        public bool Parking { get; set; }

        /// <summary>
        /// Есть ли места для транспорта инвалидов
        /// </summary>
        public bool ParkinInvalid { get; set; }

        public override JObject ToJson()
        {
            return new JObject
                {
                    new JProperty("DriveAvailable", DriveAvailable),
                    new JProperty("Pathes", Pathes),
                    new JProperty("Parking", Parking),
                    new JProperty("ParkinInvalid", ParkinInvalid),
                };
        }

        public override void FromJson(JObject obj)
        {
            DriveAvailable = GetPropertyBool(obj, "DriveAvailable");
            Pathes = GetPropertyBool(obj, "Pathes");
            Parking = GetPropertyBool(obj, "Parking");
            ParkinInvalid = GetPropertyBool(obj, "ParkinInvalid");
        }
    }
}