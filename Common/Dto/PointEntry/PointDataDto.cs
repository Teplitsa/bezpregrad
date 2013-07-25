using System;
using Newtonsoft.Json.Linq;

namespace Common.Dto.PointEntry
{
    public sealed class PointDataDto : AbstractDto
    {
        public PointDataDto()
        {
            PointPart2Invalid = new PointPart2Invalid();
            PointPart3Entry = new PointPart3Entry();
            PointPart4Lobby = new PointPart4Lobby();
            PointPart5Info = new PointPart5Info();
            PointPart6Hygiene = new PointPart6Hygiene();
            PointPart7Summary = new PointPart7Summary();
            PhotoName = null;
        }

        public string Address { get; set; }

        public string PhotoName { get; set; }

        public PointPart2Invalid PointPart2Invalid { get; set; }

        public PointPart3Entry PointPart3Entry { get; set; }

        public PointPart4Lobby PointPart4Lobby { get; set; }

        public PointPart5Info PointPart5Info { get; set; }
        public PointPart6Hygiene PointPart6Hygiene { get; set; }

        public PointPart7Summary PointPart7Summary { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        /// <summary>
        /// Не сериализуется, только для UI
        /// </summary>
        public bool HasNewPhoto
        {
            get;
            set;
        }

        /// <summary>
        /// Не сериализуется, только для UI
        /// </summary>
        public byte[] Photo
        {
            get;
            set;
        }

        public override JObject ToJson()
        {
            return new JObject
                {
                    new JProperty("Address", Address),
                    new JProperty("Longitude", Longitude),
                    new JProperty("Latitude", Latitude),
                    new JProperty("PhotoName", PhotoName),
                    new JProperty("PointPart2Invalid", PointPart2Invalid.ToJson()),
                    new JProperty("PointPart3Entry", PointPart3Entry.ToJson()),
                    new JProperty("PointPart4Lobby", PointPart4Lobby.ToJson()),
                    new JProperty("PointPart5Info", PointPart5Info.ToJson()),
                    new JProperty("PointPart6Hygiene", PointPart6Hygiene.ToJson()),
                    new JProperty("PointPart7Summary", PointPart7Summary.ToJson()),
                };
        }

        public override void FromJson(JObject obj)
        {
            try
            {
                Longitude = (double) obj.Property("Longitude").Value;
                Latitude = (double) obj.Property("Latitude").Value;
            }
            catch
            {
            }

            Address = (string) obj.Property("Address").Value;
            PhotoName = (string)obj.Property("PhotoName").Value;
            PointPart2Invalid.FromJson(GetChild(obj, "PointPart2Invalid"));
            PointPart3Entry.FromJson(GetChild(obj, "PointPart3Entry"));
            PointPart4Lobby.FromJson(GetChild(obj, "PointPart4Lobby"));
            PointPart5Info.FromJson(GetChild(obj, "PointPart5Info"));
            PointPart6Hygiene.FromJson(GetChild(obj, "PointPart6Hygiene"));
            PointPart7Summary.FromJson(GetChild(obj, "PointPart7Summary"));
        }

        //public int CountPointPart2Invalid()
        //{
        //    var col = 0;
        //    var ty = PointPart2Invalid.GetType();
        //    foreach (var s in ty.GetFields())
        //    {
        //        //Console.WriteLine(s.GetValue(t));
        //        if ((bool) s.GetValue(ty))
        //            col++;
        //    }
        //    return col;
        //}

    }
}