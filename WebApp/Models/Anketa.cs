using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.Dto.PointEntry;
using System.Reflection;

namespace WebApp.Models
{
    public class Anketa
    {
        private const int MaxCount_InvalidMoveAvailability = 24;
        private const int MaxCount_InvalidEyeAvailability = 19;
        private const int MaxCount_InvalidHearingAvailability = 5;

        public string GetResult(PointDataDto point)
        {
            var a = InvalidMoveAvailability(point);
            var b = InvalidEyeAvailability(point);
            var c = InvalidHearingAvailability(point);
            var res =
                string.Format(
                    "Доступность для инвалидов колясочников: {0:0.00}%.\n" +
                    "Доступность для инвалидов по зрению: {1:0.00}%.\n" +
                    "Доступность для инвалидов по слуху: {2:0.00}%.",
                    a/MaxCount_InvalidMoveAvailability*100,
                    b/MaxCount_InvalidEyeAvailability*100,
                    c/MaxCount_InvalidHearingAvailability*100);
            return res;
        }

        private double InvalidHearingAvailability(PointDataDto point)
        {
            if (!point.PointPart4Lobby.SoundAmplifyingEquipment)
            {
                return 0;
            }
            var col = 0.0;
            col += Convert.ToDouble(point.PointPart2Invalid.ParkinInvalid);
            col += Convert.ToDouble(point.PointPart3Entry.Awning);
            col += Convert.ToDouble(point.PointPart4Lobby.SoundAmplifyingEquipment);
            col += Convert.ToDouble(point.PointPart5Info.Evacuation);
            col += Convert.ToDouble(point.PointPart5Info.InfoLuminance);

            return col;
        }

        private double InvalidEyeAvailability(PointDataDto point)
        {
            if (!point.PointPart4Lobby.RiffleSurface)
            {
                return 0;
            }
            var col = 0.0;
            col += Convert.ToDouble(point.PointPart2Invalid.Pathes);
            col += Convert.ToDouble(point.PointPart2Invalid.ParkinInvalid);
            col += Convert.ToDouble(point.PointPart3Entry.RampNotNeeded && !point.PointPart3Entry.RampNeeded);
            col += Convert.ToDouble(point.PointPart3Entry.RampType);
            col += Convert.ToDouble(point.PointPart3Entry.Handle);
            col += Convert.ToDouble(point.PointPart3Entry.InvalidEntry);
            col += Convert.ToDouble(point.PointPart3Entry.Awning);
            col += Convert.ToDouble(point.PointPart3Entry.DoorMarks);
            col += Convert.ToDouble(point.PointPart3Entry.DoorWidth);
            col += Convert.ToDouble(point.PointPart4Lobby.InvalidMove);
            col += Convert.ToDouble(point.PointPart4Lobby.DoorWeight);
            col += Convert.ToDouble(point.PointPart4Lobby.Floor);
            col += Convert.ToDouble(point.PointPart4Lobby.NoThresholds);
            col += Convert.ToDouble(point.PointPart4Lobby.NoSteps || point.PointPart4Lobby.StepsHaveRamp);
            col += Convert.ToDouble(point.PointPart4Lobby.RiffleSurface);
            col += Convert.ToDouble(point.PointPart4Lobby.LiftDoorWeight && point.PointPart4Lobby.LiftWight);
            col += Convert.ToDouble(point.PointPart4Lobby.LiftBrail);
            col += Convert.ToDouble(point.PointPart5Info.InfoSigns);
            col += Convert.ToDouble(point.PointPart5Info.InfoVoice);

            return col;
        }

        private double InvalidMoveAvailability(PointDataDto point)
        {
            if (point.PointPart3Entry.RampNeeded || !point.PointPart3Entry.InvalidEntry)
            {
                return 0;
            }
            //var ty = point.PointPart2Invalid.GetType();
            var col = 0.0;
            //foreach (PropertyInfo s in ty.GetProperties())
            //{
            //    if (s.GetValue(point.PointPart2Invalid).GetType().Name == "Boolean" &&
            //        (bool) s.GetValue(point.PointPart2Invalid))
            //        if (s.Name == "DriveAvailable") continue;
            //            col++;
            //}
            //ty = point.PointPart3Entry.GetType();
            //col += ty.GetProperties()
            //         .Count(s =>
            //                s.GetValue(point.PointPart3Entry).GetType().Name == "Boolean" &&
            //                (bool) s.GetValue(point.PointPart3Entry));
            //ty = point.PointPart4Lobby.GetType();
            //col += ty.GetProperties()
            //         .Count(s =>
            //                s.GetValue(point.PointPart4Lobby).GetType().Name == "Boolean" &&
            //                (bool) s.GetValue(point.PointPart4Lobby));
            //ty = point.PointPart5Info.GetType();
            //col += ty.GetProperties()
            //         .Count(s =>
            //                s.GetValue(point.PointPart5Info).GetType().Name == "Boolean" &&
            //                (bool) s.GetValue(point.PointPart5Info));
            //ty = point.PointPart6Hygiene.GetType();
            //col += ty.GetProperties()
            //         .Count(s =>
            //                s.GetValue(point.PointPart6Hygiene).GetType().Name == "Boolean" &&
            //                (bool) s.GetValue(point.PointPart6Hygiene));
            col += Convert.ToDouble(point.PointPart2Invalid.DriveAvailable);
            col += Convert.ToDouble(point.PointPart2Invalid.ParkinInvalid);
            col += Convert.ToDouble(point.PointPart3Entry.RampNotNeeded && !point.PointPart3Entry.RampNeeded);
            col += Convert.ToDouble(point.PointPart3Entry.RampType); col += Convert.ToDouble(point.PointPart3Entry.Handle);
            col += Convert.ToDouble(point.PointPart3Entry.InvalidEntry);
            col += Convert.ToDouble(point.PointPart3Entry.Awning);
            col += Convert.ToDouble(point.PointPart3Entry.DoorWidth && point.PointPart3Entry.Threshold);
            col += Convert.ToDouble(point.PointPart4Lobby.InvalidMove);
            col += Convert.ToDouble(point.PointPart4Lobby.DoorWeight);
            col += Convert.ToDouble(point.PointPart4Lobby.Floor);
            col += Convert.ToDouble(point.PointPart4Lobby.NoThresholds);
            col += Convert.ToDouble(point.PointPart4Lobby.NoSteps || point.PointPart4Lobby.StepsHaveRamp);
            col += Convert.ToDouble(point.PointPart4Lobby.RiffleSurface);
            col += Convert.ToDouble(point.PointPart4Lobby.InvalidManeuver);
            col += Convert.ToDouble(point.PointPart4Lobby.EntryPost);
            col += Convert.ToDouble(point.PointPart4Lobby.LiftDoorWeight && point.PointPart4Lobby.LiftWight);
            col += Convert.ToDouble(point.PointPart5Info.Evacuation);
            col += Convert.ToDouble(point.PointPart5Info.InfoWeight);
            col += Convert.ToDouble(point.PointPart6Hygiene.Wc && point.PointPart6Hygiene.WcInvalid);
            col += Convert.ToDouble(point.PointPart6Hygiene.WcUniversal);
            col += Convert.ToDouble(point.PointPart6Hygiene.WcInvalidSigns);
            col += Convert.ToDouble(point.PointPart6Hygiene.WcDoorsOut);
            col += Convert.ToDouble(point.PointPart6Hygiene.WcInnerHandles);
            return col;
        }
    }
}