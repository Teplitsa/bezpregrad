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
    internal sealed class Screen4Lobby : BaseScreen
    {
        public Screen4Lobby(Activity context, PointDto point)
            : base(context, Resource.Layout.Edit4Lobby, point)
        {
            PointPart4Lobby item = Point.PointDataDto.PointPart4Lobby;

            View.FindViewById<CheckBox>(Resource.Id.doorWeight).Checked = item.DoorWeight;
            View.FindViewById<CheckBox>(Resource.Id.entryPost).Checked = item.EntryPost;
            View.FindViewById<CheckBox>(Resource.Id.SoundAmplifyingEquipment).Checked = item.SoundAmplifyingEquipment;
            View.FindViewById<CheckBox>(Resource.Id.floor).Checked = item.Floor;
            View.FindViewById<CheckBox>(Resource.Id.invalidManeuver).Checked = item.InvalidManeuver;
            View.FindViewById<CheckBox>(Resource.Id.invalidMove).Checked = item.InvalidMove;
            var liftCheckBox = View.FindViewById<CheckBox>(Resource.Id.lift);
            liftCheckBox.Checked = item.Lift;
            View.FindViewById<CheckBox>(Resource.Id.liftBrail).Checked = item.LiftBrail;
            View.FindViewById<CheckBox>(Resource.Id.liftDepth).Checked = item.LiftDepth;
            View.FindViewById<CheckBox>(Resource.Id.liftDoorWeight).Checked = item.LiftDoorWeight;
            View.FindViewById<CheckBox>(Resource.Id.liftWight).Checked = item.LiftWight;
            View.FindViewById<CheckBox>(Resource.Id.noSteps).Checked = item.NoSteps;
            View.FindViewById<CheckBox>(Resource.Id.noThresholds).Checked = item.NoThresholds;
            View.FindViewById<CheckBox>(Resource.Id.riffleSurface).Checked = item.RiffleSurface;
            View.FindViewById<CheckBox>(Resource.Id.stepsHaveRamp).Checked = item.StepsHaveRamp;
            View.FindViewById<CheckBox>(Resource.Id.liftBrail).Enabled = liftCheckBox.Checked;
            View.FindViewById<CheckBox>(Resource.Id.liftDepth).Enabled = liftCheckBox.Checked;
            View.FindViewById<CheckBox>(Resource.Id.liftDoorWeight).Enabled = liftCheckBox.Checked;
            View.FindViewById<CheckBox>(Resource.Id.liftWight).Enabled = liftCheckBox.Checked;
            View.FindViewById<CheckBox>(Resource.Id.lift).CheckedChange += Screen4Lobby_CheckedChange;
        }

        private void Screen4Lobby_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            var liftCheckBox = (CheckBox) sender;
            View.FindViewById<CheckBox>(Resource.Id.liftBrail).Enabled = liftCheckBox.Checked;
            View.FindViewById<CheckBox>(Resource.Id.liftDepth).Enabled = liftCheckBox.Checked;
            View.FindViewById<CheckBox>(Resource.Id.liftDoorWeight).Enabled = liftCheckBox.Checked;
            View.FindViewById<CheckBox>(Resource.Id.liftWight).Enabled = liftCheckBox.Checked;
            if (!liftCheckBox.Checked)
            {
                View.FindViewById<CheckBox>(Resource.Id.liftBrail).Checked = liftCheckBox.Checked;
                View.FindViewById<CheckBox>(Resource.Id.liftDepth).Checked = liftCheckBox.Checked;
                View.FindViewById<CheckBox>(Resource.Id.liftDoorWeight).Checked = liftCheckBox.Checked;
                View.FindViewById<CheckBox>(Resource.Id.liftWight).Checked = liftCheckBox.Checked;
            }
        }

        internal override void SaveToPoint()
        {
            PointPart4Lobby item = Point.PointDataDto.PointPart4Lobby;

            item.DoorWeight = View.FindViewById<CheckBox>(Resource.Id.doorWeight).Checked;
            item.EntryPost = View.FindViewById<CheckBox>(Resource.Id.entryPost).Checked;
            item.SoundAmplifyingEquipment = View.FindViewById<CheckBox>(Resource.Id.SoundAmplifyingEquipment).Checked;
            item.Floor = View.FindViewById<CheckBox>(Resource.Id.floor).Checked;
            item.InvalidManeuver = View.FindViewById<CheckBox>(Resource.Id.invalidManeuver).Checked;
            item.InvalidMove = View.FindViewById<CheckBox>(Resource.Id.invalidMove).Checked;
            item.Lift = View.FindViewById<CheckBox>(Resource.Id.lift).Checked;
            item.LiftBrail = View.FindViewById<CheckBox>(Resource.Id.liftBrail).Checked;
            item.LiftDepth = View.FindViewById<CheckBox>(Resource.Id.liftDepth).Checked;
            item.LiftDoorWeight = View.FindViewById<CheckBox>(Resource.Id.liftDoorWeight).Checked;
            item.LiftWight = View.FindViewById<CheckBox>(Resource.Id.liftWight).Checked;
            item.NoSteps = View.FindViewById<CheckBox>(Resource.Id.noSteps).Checked;
            item.NoThresholds = View.FindViewById<CheckBox>(Resource.Id.noThresholds).Checked;
            item.RiffleSurface = View.FindViewById<CheckBox>(Resource.Id.riffleSurface).Checked;
            item.StepsHaveRamp = View.FindViewById<CheckBox>(Resource.Id.stepsHaveRamp).Checked;
        }
    }
}