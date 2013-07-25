using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Common.Dto.PointEntry
{
    public sealed class PointPart4Lobby : AbstractDto
    {
        /// <summary>
        /// Внутри помещения инвалиды могут свободно перемещаться между разными функциональными зонами
        /// </summary>
        public bool InvalidMove { get; set; }

        /// <summary>
        /// Ширина дверных и открытых проемов, выходов из помещений и из коридоров на лестничную клетку не менее 90см
        /// </summary>
        public bool DoorWeight { get; set; }

        /// <summary>
        /// Покрытие пола в коридорах выполнено из нескользких материалов
        /// </summary>
        public bool Floor { get; set; }

        /// <summary>
        /// Пути движения не имеют порогов и иных препятствий выше 2см
        /// </summary>
        public bool NoThresholds { get; set; }

        /// <summary>
        /// Внутри осутствуют ступени между функциональными зонами
        /// </summary>
        public bool NoSteps { get; set; }

        /// <summary>
        /// При наличии, они оборудованы пандусом
        /// </summary>
        public bool StepsHaveRamp { get; set; }

        /// <summary>
        /// Предупредительная рифленая и\или контрастно окрашенная поверхность перед дверными проемами и входами на лестницу
        /// </summary>
        public bool RiffleSurface { get; set; }

        /// <summary>
        /// В зоне обслуживания достаточно пространства для маневров на инвалидной коляске
        /// </summary>
        public bool InvalidManeuver { get; set; }

        /// <summary>
        /// Стойка приема (прилавка) располагается не выше 1,0м от уровня пола
        /// </summary>
        public bool EntryPost { get; set; }

        /// <summary>
        /// В зоне обслуживания имеется звукоусиливающая аппаратура, световые табло 
        /// </summary>
        public bool SoundAmplifyingEquipment { get; set; }

        /// <summary>
        /// Наличие лифта
        /// </summary>
        public bool Lift { get; set; }

        /// <summary>
        /// Ширина кабины лифта не менее 1,1м 
        /// </summary>
        public bool LiftWight { get; set; }

        /// <summary>
        /// Глубина кабины лифта не менее 1,4м
        /// </summary>
        public bool LiftDepth { get; set; }

        /// <summary>
        /// Ширина двери лифта не менее 90см 
        /// </summary>
        public bool LiftDoorWeight { get; set; }

        /// <summary>
        /// Кнопки лифта помимо графических символов содержат шрифт Брайля 
        /// </summary>
        public bool LiftBrail { get; set; }

        public override JObject ToJson()
        {
            return new JObject(
                new JProperty("DoorWeight", DoorWeight),
                new JProperty("EntryPost", EntryPost),
                new JProperty("SoundAmplifyingEquipment", SoundAmplifyingEquipment),
                new JProperty("Floor", Floor),
                new JProperty("InvalidManeuver", InvalidManeuver),
                new JProperty("InvalidMove", InvalidMove),
                new JProperty("Lift", Lift),
                new JProperty("LiftBrail", LiftBrail),
                new JProperty("LiftDepth", LiftDepth),
                new JProperty("LiftDoorWeight", LiftDoorWeight),
                new JProperty("LiftWight", LiftWight),
                new JProperty("NoSteps", NoSteps),
                new JProperty("RiffleSurface", RiffleSurface),
                new JProperty("StepsHaveRamp", StepsHaveRamp),
                new JProperty("NoThresholds", NoThresholds)
                );
        }

        public override void FromJson(JObject obj)
        {
            DoorWeight = GetPropertyBool(obj, "DoorWeight");
            EntryPost = GetPropertyBool(obj, "EntryPost");
            SoundAmplifyingEquipment = GetPropertyBool(obj, "SoundAmplifyingEquipment");
            Floor = GetPropertyBool(obj, "Floor");
            InvalidManeuver = GetPropertyBool(obj, "InvalidManeuver");
            InvalidMove = GetPropertyBool(obj, "InvalidMove");
            Lift = GetPropertyBool(obj, "Lift");
            LiftBrail = GetPropertyBool(obj, "LiftBrail");
            LiftDepth = GetPropertyBool(obj, "LiftDepth");
            LiftDoorWeight = GetPropertyBool(obj, "LiftDoorWeight");
            LiftWight = GetPropertyBool(obj, "LiftWight");
            NoSteps = GetPropertyBool(obj, "NoSteps");
            RiffleSurface = GetPropertyBool(obj, "RiffleSurface");
            StepsHaveRamp = GetPropertyBool(obj, "StepsHaveRamp");
            NoThresholds = GetPropertyBool(obj, "NoThresholds");
        }
    }
}