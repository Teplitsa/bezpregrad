using System;
using System.Collections.Generic;
using System.Globalization;
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
    internal sealed class Screen7Summary : BaseScreen
    {
        private readonly Spinner invalidMoveAvailable;
        private readonly Spinner invalidHearingAvailable;
        private readonly Spinner invalidEyeAvailable;
        private readonly EditText otherText;
        private readonly EditText reportDate;

        public Screen7Summary(Activity context, PointDto point)
            : base(context, Resource.Layout.Edit7Summary, point)
        {
            PointPart7Summary item = Point.PointDataDto.PointPart7Summary;

            invalidMoveAvailable = View.FindViewById<Spinner>(Resource.Id.invalid_move_availability);
            invalidHearingAvailable = View.FindViewById<Spinner>(Resource.Id.invalid_hearing_availability);
            invalidEyeAvailable = View.FindViewById<Spinner>(Resource.Id.invalid_eye_availability);
            otherText = View.FindViewById<EditText>(Resource.Id.otherComments);
            reportDate = View.FindViewById<EditText>(Resource.Id.protocolDate);

            invalidMoveAvailable.Adapter = new BoolSpinnerAdapter(context, "Объект доступен",
                                                                  "Объект не доступен (если нет пандуса, не соответствует ширина проемов, высота порогов)");
            invalidHearingAvailable.Adapter = new BoolSpinnerAdapter(context, "Объект доступен",
                                                                     "Объект не доступен (если отсутствуют световые табло, индукционная петля)");
            invalidEyeAvailable.Adapter = new BoolSpinnerAdapter(context, "Объект доступен",
                                                                 "Объект не доступен (если отсутствуют тактильные маршруты, звуковое оповещение, контрастная маркировка дверей и стеклянных поверхностей)");

            reportDate.Text = item.ReportDate.ToLocalTime().Date.ToString(CultureInfo.CurrentCulture);

            invalidMoveAvailable.SetSelection(BoolSpinnerAdapter.SetPositionOf(item.InvalidMoveAvailability));
            invalidHearingAvailable.SetSelection(BoolSpinnerAdapter.SetPositionOf(item.InvalidHearingAvailability));
            invalidEyeAvailable.SetSelection(BoolSpinnerAdapter.SetPositionOf(item.InvalidEyeAvailability));

            otherText.Text = item.OtherComments;
        }

        internal override void SaveToPoint()
        {
            PointPart7Summary item = Point.PointDataDto.PointPart7Summary;

            item.InvalidMoveAvailability = BoolSpinnerAdapter.IsTrue(invalidMoveAvailable.SelectedItemPosition);
            item.InvalidHearingAvailability = BoolSpinnerAdapter.IsTrue(invalidHearingAvailable.SelectedItemPosition);
            item.InvalidEyeAvailability = BoolSpinnerAdapter.IsTrue(invalidEyeAvailable.SelectedItemPosition);
            item.OtherComments = otherText.Text;
            DateTime a;
            if (DateTime.TryParse(reportDate.Text, out a))
            {
                item.ReportDate = a;
            }
        }
    }
}