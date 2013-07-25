using Common.Dto.PointEntry;
using Newtonsoft.Json.Linq;

namespace Common.Dto
{
    public sealed class PointDto : AbstractDto
    {
        public enum CategoryType
        {
            //Административные объекты (органы власти, учреждения)
            Administration,
            //Коммерческие объекты (магазины, банки, торговые центры)
            Commerce,
            //Офисные объекты (предприятия, компании, бизнес-центры)
            Office,
            //Медицинские объекты (аптеки, больницы, санатории)
            HealthArea,
            //Объекты культуры (музеи, библиотеки, церкви)
            CultureArea,
            //Сфера услуг (парикмахерские, бытовые услуги)
            Services,
            //Объекты питания (кафе, бары, рестораны)
            Food,
            //Спортивные объекты (фитнес-центры, клубы)
            Sport,
            //Образование (сады, школы, вузы, секции)
            Education,
            //Иные объекты
            Other
        }

        public PointDto()
        {
            //Name = "Точка";
            Name = "";
            PointDataDto = new PointDataDto();
            
            //Category = CategoryType.Other;
        }

        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int Category
        {
            get;
            set;
        }

        public PointDataDto PointDataDto
        {
            get;
            set;
        }

        public override JObject ToJson()
        {
            return new JObject
                {
                    new JProperty("Id", Id),
                    new JProperty("Name", Name),
                    new JProperty("Category", Category),
                    new JProperty("PointDataDto", PointDataDto.ToJson()),
                };
        }

        public override void FromJson(JObject obj)
        {
            Id = (int)obj.Property("Id").Value;
            Name = (string)obj.Property("Name").Value;
            Category = (int)obj.Property("Category").Value;
            PointDataDto.FromJson((JObject)obj.Property("PointDataDto").Value);
        }
    }
}