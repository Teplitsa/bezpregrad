using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AndroidApp
{
    internal sealed class BoolSpinnerAdapter : BaseAdapter<string>
    {
        private readonly Context context;
        private readonly string trueText;
        private readonly string falseText;

        public BoolSpinnerAdapter(Context context, string trueText, string falseText)
        {
            this.falseText = falseText;
            this.trueText = trueText;
            this.context = context;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var textView = new TextView(context)
                {
                    Text = this[position],
                };

            textView.SetTextColor(Color.Black);

            textView.SetWidth(0);

            return textView;
        }

        public override int Count
        {
            get
            {
                return 2;
            }
        }

        public override string this[int position]
        {
            get
            {
                return IsTrue(position) ? trueText : falseText;
            }
        }

        public static bool IsTrue(int position)
        {
            return position == 0;
        }

        public static int SetPositionOf(bool value)
        {
            return value ? 0 : 1;
        }
    }
}