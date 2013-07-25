using Android.App;
using Android.Content.PM;
using Android.OS;

namespace AndroidApp.Screens
{
    [Activity(Label = "HelpSubActivity",
        ConfigurationChanges = ConfigChanges.Orientation)]			
	public class HelpSubActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView(Resource.Layout.HelpSubActivity);
		}
	}
}

