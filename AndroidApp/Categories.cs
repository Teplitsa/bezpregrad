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
                    return "���������������� �������";

                case PointDto.CategoryType.Commerce:
                    return "������������ �������";

                case PointDto.CategoryType.Office:
                    return "������� �������";

                case PointDto.CategoryType.HealthArea:
                    return "����������� �������";

                case PointDto.CategoryType.CultureArea:
                    return "������� ��������";

                case PointDto.CategoryType.Services:
                    return "����� �����";

                case PointDto.CategoryType.Food:
                    return "������� �������";

                case PointDto.CategoryType.Sport:
                    return "���������� �������";

                case PointDto.CategoryType.Education:
                    return "�����������";

                case PointDto.CategoryType.Other:
                    return "���� ������� ";

                default:
                    throw new ArgumentOutOfRangeException("category", ((int)category).ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}