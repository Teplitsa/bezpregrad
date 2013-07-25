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
using Common;

namespace AndroidApp.Screens
{
    [Activity(Label = "Мои пункты", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : TabActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            if (AuthData.AuthResult == null)
            {
                StartActivity(typeof(LoginActivity));
                FinishActivity(0);

                return;
            }

            SetContentView(Resource.Layout.MainActivity);

            TabHost.TabSpec spec;     // Resusable TabSpec for each tab
            Intent intent;            // Reusable Intent for each tab

            // Create an Intent to launch an Activity for the tab (to be reused)
            intent = new Intent(this, typeof(HelpSubActivity));
            intent.AddFlags(ActivityFlags.NewTask);

            // Initialize a TabSpec for each tab and add it to the TabHost
            spec = TabHost.NewTabSpec("subHelp");
            spec.SetIndicator("Главная");
            spec.SetContent(intent);
            TabHost.AddTab(spec);

            // Do the same for the other tabs
            intent = new Intent(this, typeof(ItemsSubActivity));
            intent.AddFlags(ActivityFlags.NewTask);

            spec = TabHost.NewTabSpec("myPoint");
            spec.SetIndicator("Мои пункты");
            spec.SetContent(intent);
            TabHost.AddTab(spec);

            TabHost.CurrentTab = 0;

            /*
             
             spec.SetIndicator(new TextView(this)
            {
                Text = "Главная"
            });
             
             spec.SetIndicator(new TextView(this)
            {
                Text = "Мои пункты"
            });
             */
        }
    }
}