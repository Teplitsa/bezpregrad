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
    internal sealed class Screen6Hygiene : BaseScreen
    {
        public Screen6Hygiene(Activity context, PointDto point)
            : base(context, Resource.Layout.Edit6Hygiene, point)
        {
            PointPart6Hygiene item = Point.PointDataDto.PointPart6Hygiene;

            View.FindViewById<CheckBox>(Resource.Id.wc).Checked = item.Wc;
            View.FindViewById<CheckBox>(Resource.Id.wc_doors_out).Checked = item.WcDoorsOut;
            View.FindViewById<CheckBox>(Resource.Id.wc_inner_handles).Checked = item.WcInnerHandles;
            View.FindViewById<CheckBox>(Resource.Id.wc_invalid).Checked = item.WcInvalid;
            View.FindViewById<CheckBox>(Resource.Id.wc_invalid_signs).Checked = item.WcInvalidSigns;
            View.FindViewById<CheckBox>(Resource.Id.wc_universal).Checked = item.WcUniversal;
        }

        internal override void SaveToPoint()
        {
            PointPart6Hygiene item = Point.PointDataDto.PointPart6Hygiene;

            item.Wc = View.FindViewById<CheckBox>(Resource.Id.wc).Checked;
            item.WcDoorsOut = View.FindViewById<CheckBox>(Resource.Id.wc_doors_out).Checked;
            item.WcInnerHandles = View.FindViewById<CheckBox>(Resource.Id.wc_inner_handles).Checked;
            item.WcInvalid = View.FindViewById<CheckBox>(Resource.Id.wc_invalid).Checked;
            item.WcInvalidSigns = View.FindViewById<CheckBox>(Resource.Id.wc_invalid_signs).Checked;
            item.WcUniversal = View.FindViewById<CheckBox>(Resource.Id.wc_universal).Checked;
        }
    }
}