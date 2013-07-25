
using Android.App;
using Android.Widget;
using Common.Dto;
using Common.Dto.PointEntry;

namespace AndroidApp.Screens.Edit
{
    internal sealed class Screen3Entry : BaseScreen
    {

        public Screen3Entry(Activity context, PointDto point)
            : base(context, Resource.Layout.Edit3Entry, point)
        {
            if (View == null)
                return;

            PointPart3Entry item = Point.PointDataDto.PointPart3Entry;

            var awning = View.FindViewById<CheckBox>(Resource.Id.awning);
            var doorMarks = View.FindViewById<CheckBox>(Resource.Id.doorMarks);
            var doorWidth = View.FindViewById<CheckBox>(Resource.Id.doorWidth);
            var handle = View.FindViewById<CheckBox>(Resource.Id.handle);
            var invalidEntry = View.FindViewById<CheckBox>(Resource.Id.invalidEntry);
            var rampNeeded = View.FindViewById<CheckBox>(Resource.Id.rampNeeded);
            var rampNotNeeded = View.FindViewById<CheckBox>(Resource.Id.rampNotNeeded);
            var rampType = View.FindViewById<CheckBox>(Resource.Id.rampType);
            var threshold = View.FindViewById<CheckBox>(Resource.Id.threshold);

            awning.Checked = item.Awning;
            doorMarks.Checked = item.DoorMarks;
            doorWidth.Checked = item.DoorWidth;
            invalidEntry.Checked = item.InvalidEntry;
            rampNeeded.Checked = item.RampNeeded;
            handle.Checked = item.Handle;
            rampNotNeeded.Checked = item.RampNotNeeded;
            rampType.Checked = item.RampType;
            threshold.Checked = item.Threshold;
        }

        internal override void SaveToPoint()
        {
            PointPart3Entry item = Point.PointDataDto.PointPart3Entry;

            item.Awning = View.FindViewById<CheckBox>(Resource.Id.awning).Checked;
            item.DoorMarks = View.FindViewById<CheckBox>(Resource.Id.doorMarks).Checked;
            item.DoorWidth = View.FindViewById<CheckBox>(Resource.Id.doorWidth).Checked;
            item.Handle = View.FindViewById<CheckBox>(Resource.Id.handle).Checked;
            item.InvalidEntry = View.FindViewById<CheckBox>(Resource.Id.invalidEntry).Checked;
            item.RampNeeded = View.FindViewById<CheckBox>(Resource.Id.rampNeeded).Checked;
            item.RampNotNeeded = View.FindViewById<CheckBox>(Resource.Id.rampNotNeeded).Checked;
            item.RampType = View.FindViewById<CheckBox>(Resource.Id.rampType).Checked;
            item.Threshold = View.FindViewById<CheckBox>(Resource.Id.threshold).Checked;
        }
    }
}