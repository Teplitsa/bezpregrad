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
    internal sealed class Screen5Info: BaseScreen
    {
        public Screen5Info(Activity context,  PointDto point)
            : base(context, Resource.Layout.Edit5Info, point)
        {
            PointPart5Info item = Point.PointDataDto.PointPart5Info;

            View.FindViewById<CheckBox>(Resource.Id.evacuation).Checked = item.Evacuation;
            View.FindViewById<CheckBox>(Resource.Id.infoLuminance).Checked = item.InfoLuminance;
            View.FindViewById<CheckBox>(Resource.Id.infoSigns).Checked = item.InfoSigns;
            View.FindViewById<CheckBox>(Resource.Id.infoVoice).Checked = item.InfoVoice;
            View.FindViewById<CheckBox>(Resource.Id.infoWeight).Checked = item.InfoWeight;

        }

        internal override void SaveToPoint()
        {
            PointPart5Info item = Point.PointDataDto.PointPart5Info;

            item.Evacuation = View.FindViewById<CheckBox>(Resource.Id.evacuation).Checked;
            item.InfoLuminance = View.FindViewById<CheckBox>(Resource.Id.infoLuminance).Checked;
            item.InfoSigns = View.FindViewById<CheckBox>(Resource.Id.infoSigns).Checked;
            item.InfoVoice = View.FindViewById<CheckBox>(Resource.Id.infoVoice).Checked;
            item.InfoWeight = View.FindViewById<CheckBox>(Resource.Id.infoWeight).Checked;
        }
    }
}