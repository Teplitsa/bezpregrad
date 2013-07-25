using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.OS;
using Android.Runtime;
using Common.Dto;

namespace AndroidApp
{
    internal static class CategoriesHelper
    {
        public static string GetName(PointDto.CategoryType category)
        {
            switch (category)
            {
                case PointDto.CategoryType.Administration:
                    return "Административные объекты";

                case PointDto.CategoryType.Commerce:
                    return "Коммерческие объекты";

                case PointDto.CategoryType.Office:
                    return "Офисные объекты";

                case PointDto.CategoryType.HealthArea:
                    return "Медицинские объекты";

                case PointDto.CategoryType.CultureArea:
                    return "Объекты культуры";

                case PointDto.CategoryType.Services:
                    return "Сфера услуг";

                case PointDto.CategoryType.Food:
                    return "Объекты питания";

                case PointDto.CategoryType.Sport:
                    return "Спортивные объекты";

                case PointDto.CategoryType.Education:
                    return "Образование";

                case PointDto.CategoryType.Other:
                    return "Иные объекты ";

                default:
                    throw new ArgumentOutOfRangeException("category", ((int)category).ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}