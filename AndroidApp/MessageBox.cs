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

namespace AndroidApp
{
    internal static class MessageBox
    {
        public static void ShowMessage(string message, Context context)
        {
            var builder = new AlertDialog.Builder(context);

            builder.SetTitle(Resource.String.app_name);
            builder.SetMessage(message);

            builder.SetPositiveButton("OK", (sender, args) =>
                {
                });

            builder.Create().Show();
        }
    }
}