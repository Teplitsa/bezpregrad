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
using Java.Lang;
using Newtonsoft.Json.Linq;

namespace AndroidApp.Screens.Edit
{
    public abstract class BaseScreen
    {
        protected BaseScreen(Activity context, int viewResourseId, PointDto point)
        {
            Point = point;
            try
            {
                View = context.LayoutInflater.Inflate(viewResourseId, null);
            }
            catch (Throwable t)
            {
                MessageBox.ShowMessage("Ошибка при построени " + GetType().Name + ": " + t.LocalizedMessage, context);
            }
        }

        public View View
        {
            get;
            private set;
        }

        protected PointDto Point
        {
            get;
            private set;
        }

        internal abstract void SaveToPoint();
    }
}