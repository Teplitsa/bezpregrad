using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Common.Dto;
using Newtonsoft.Json.Linq;

namespace WebApp.Database.Entities
{
    public class DbPoint
    {
        public DbPoint()
        {
            CreationTimeUtc = DateTime.UtcNow;
        }

        public void Update(PointDto point)
        {
            Name = point.Name;
            PointData = point.PointDataDto.ToJson().ToString();
            CategoryId = (int)point.Category;
        }

        [Required]
        [Key]
        public virtual int Id
        {
            get;
            set;
        }

        [Required]
        public virtual string Name
        {
            get;
            set;
        }

        [Required]
        public virtual DateTime CreationTimeUtc
        {
            get;
            set;
        }

        /// <summary>
        /// В базе храним только xml, чтобы не копировать одинаковые поля и не сслыаться из базы на внешние сборки
        /// </summary>
        [Required]
        public virtual string PointData
        {
            get;
            set;
        }

        [Required]
        public virtual int CategoryId
        {
            get;
            set;
        }

        public PointDto ToDto()
        {
            var pointDto = new PointDto
                {
                    Id = Id,
                    Name = Name,
                    Category = CategoryId
                };

            pointDto.PointDataDto.FromJson(JObject.Parse(PointData));
            return pointDto;
        }
    }
}