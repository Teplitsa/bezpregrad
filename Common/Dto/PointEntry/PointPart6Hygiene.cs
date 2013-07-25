using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Common.Dto.PointEntry
{
    public sealed class PointPart6Hygiene : AbstractDto
    {
        /// <summary>
        /// На объекте размещена общественная уборная
        /// </summary>
        public bool Wc { get; set; }

        /// <summary>
        /// есть хотя бы одна универсальная кабина для инвалидов
        /// </summary>
        public bool WcInvalid { get; set; }

        /// <summary>
        /// Универсальная кабина с шириной не менее 1,65м и глубиной не менее 1,8м в которой унитаз расположен по центру с возможностью подхода с любой стороны
        /// </summary>
        public bool WcUniversal { get; set; }

        /// <summary>
        /// Входы в санузлы и специальные кабины для инвалидов выделены тактильными указателями
        /// </summary>
        public bool WcInvalidSigns { get; set; }

        /// <summary>
        /// Двери из санитарно-гигиенических кабин и помещений для инвалидов открываются наружу
        /// </summary>
        public bool WcDoorsOut { get; set; }

        /// <summary>
        /// В санитарно-гигиенических помещениях есть поручни, штанги, подвесные трапеции и т.п.
        /// </summary>
        public bool WcInnerHandles { get; set; }

        public override JObject ToJson()
        {
            return new JObject(
                new JProperty("Wc", Wc),
                new JProperty("WcInvalid", WcInvalid),
                new JProperty("WcUniversal", WcUniversal),
                new JProperty("WcInvalidSigns", WcInvalidSigns),
                new JProperty("WcDoorsOut", WcDoorsOut),
                new JProperty("WcInnerHandles", WcInnerHandles)
                );
        }

        public override void FromJson(JObject obj)
        {
            Wc = GetPropertyBool(obj, "Wc");
            WcInvalid = GetPropertyBool(obj, "WcInvalid");
            WcUniversal = GetPropertyBool(obj, "WcUniversal");
            WcInvalidSigns = GetPropertyBool(obj, "WcInvalidSigns");
            WcDoorsOut = GetPropertyBool(obj, "WcDoorsOut");
            WcInnerHandles = GetPropertyBool(obj, "WcInnerHandles");
        }
    }
}