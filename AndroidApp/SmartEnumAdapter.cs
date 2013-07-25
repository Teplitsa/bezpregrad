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
    internal sealed class SmartEnumAdapter<TEnum> : BaseAdapter<string>
        where TEnum : struct
    {
        private static readonly TEnum[] values = (TEnum[])Enum.GetValues(typeof(TEnum));
        private Func<TEnum, string> textGetter;
        private Context context;

        public SmartEnumAdapter(Func<TEnum, string> textGetter, Context context)
        {
            this.context = context;
            this.textGetter = textGetter;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var textView = new TextView(context)
                {
                    Text = textGetter(values[position])
                };

            //textView.SetTextColor(Color.Black);
            textView.SetTextAppearance(context, Android.Resource.Attribute.TextAppearanceMedium);
            return textView;
        }

        public override int Count
        {
            get
            {
                return values.Length;
            }
        }

        public int PositionOfItem(TEnum item)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (Equals(item, values[i]))
                    return i;
            }

            throw new ArgumentOutOfRangeException("item");
        }

        public TEnum GetValue(int position)
        {
            return values[position];
        }

        public override string this[int position]
        {
            get
            {
                return textGetter(values[position]);
            }
        }
    }
}