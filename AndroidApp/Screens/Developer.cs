using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AndroidApp.Screens
{
    [Activity(Label = "Разработчики", ScreenOrientation = ScreenOrientation.Portrait)]
    public class Developer : Activity
    {
        public Button backButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Developer);

            backButton = FindViewById<Button>(Resource.Id.BackButton);
            backButton.Click += backButton_Click;
            var vebAspClick = FindViewById<ImageView>(Resource.Id.webAspectButton);
            vebAspClick.Click += vebAspClick_Click;

        }

        void vebAspClick_Click(object sender, EventArgs e)
        {
            var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse("http://webaspect.biz"));
            StartActivity(intent);
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Finish();
        }
    }
}