using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Common.Dto;
using Common.Dto.PointEntry;

namespace AndroidApp.Screens.Edit
{
    [Activity(Label = "Без преград: возле здания", WindowSoftInputMode = SoftInput.StateHidden)]
    internal sealed class Screen2Invalid : BaseScreen
    {
        private readonly CheckBox driveAvailable;
        private readonly CheckBox pathes;
        private readonly CheckBox parking;
        private readonly CheckBox parkingInvalid;

        public Screen2Invalid(Activity context, PointDto point)
            : base(context, Resource.Layout.Edit2Invalid, point)
        {
            if (View == null)
                return;

            driveAvailable = View.FindViewById<CheckBox>(Resource.Id.drive_available);
            pathes = View.FindViewById<CheckBox>(Resource.Id.pathes);
            parking = View.FindViewById<CheckBox>(Resource.Id.parking);
            parkingInvalid = View.FindViewById<CheckBox>(Resource.Id.parking_invalid);

            PointPart2Invalid pointPart2Invalid = Point.PointDataDto.PointPart2Invalid;

            driveAvailable.Checked = pointPart2Invalid.DriveAvailable;
            pathes.Checked = pointPart2Invalid.Pathes;
            parking.Checked = pointPart2Invalid.Parking;
            parkingInvalid.Checked = pointPart2Invalid.ParkinInvalid;
        }

        internal override void SaveToPoint()
        {
            PointPart2Invalid invalidData = Point.PointDataDto.PointPart2Invalid;

            invalidData.DriveAvailable = driveAvailable.Checked;
            invalidData.Pathes = pathes.Checked;
            invalidData.Parking = parking.Checked;
            invalidData.ParkinInvalid = parkingInvalid.Checked;
        }
    }
}