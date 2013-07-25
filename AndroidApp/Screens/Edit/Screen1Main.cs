using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Locations;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Common.Dto;
using Common.Services;
using Java.IO;
using Java.Lang;
using Environment = Android.OS.Environment;
using Exception = System.Exception;

namespace AndroidApp.Screens.Edit
{
    [Activity(Label = "Без преград: добавление точки")]
    internal sealed class Screen1Main : BaseScreen
    {
        private readonly EditText name;
        private readonly EditText address;
        private readonly Spinner category;
        private readonly ImageView photo;
        private readonly Button loadPhotoButton;
        //private readonly SmartEnumAdapter<PointDto.CategoryType> adapter;
        private readonly ArrayAdapter<string> adapter;
        private readonly EditItemMainActivity context;
        internal File imageFile;
        private File _dir;

        public Screen1Main(EditItemMainActivity context, PointDto point)
            : base(context, Resource.Layout.Edit1Main, point)
        {
            this.context = context;
            if (View == null)
                return;

            name = View.FindViewById<EditText>(Resource.Id.nameText);
            address = View.FindViewById<EditText>(Resource.Id.addressText);
            category = View.FindViewById<Spinner>(Resource.Id.category);
            photo = View.FindViewById<ImageView>(Resource.Id.photo);
            loadPhotoButton = View.FindViewById<Button>(Resource.Id.loadPhotoButton);

            //adapter = new SmartEnumAdapter<PointDto.CategoryType>(CategoriesHelper.GetName, context);
            //var list = typeof(PointDto.CategoryType);
            //var list = PointDto.EnumValues;
            //var list1 = (from object s in list select CategoriesHelper.GetName((PointDto.CategoryType)s)).ToList();
            var list1 = new List<string>
                {
                    "Административные объекты",
                    "Коммерческие объекты",
                    "Офисные объекты",
                    "Медицинские объекты",
                    "Объекты культуры",
                    "Сфера услуг",
                    "Объекты питания",
                    "Спортивные объекты",
                    "Образование",
                    "Иные объекты "
                };
            adapter = new ArrayAdapter<string>(context, Android.Resource.Layout.SimpleSpinnerItem, list1);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            category.Adapter = adapter;

            photo.Touch += photo_Touch;
            loadPhotoButton.Click += loadPhotoButton_Click;

            //adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);


            name.Text = Point.Name;
            address.Text = Point.PointDataDto.Address;

            if (point.Id == 0)
            {
                loadPhotoButton.Visibility = ViewStates.Invisible;
            }
            else
            {
                photo.Visibility = ViewStates.Invisible;
            }
        }

        private void loadPhotoButton_Click(object sender, EventArgs e)
        {
            loadPhotoButton.Enabled = false;

            if(string.IsNullOrWhiteSpace(Point.PointDataDto.PhotoName))
                return;

            ThreadPool.QueueUserWorkItem(state =>
                {
                    string error = string.Empty;
                    try
                    {
                        Point.PointDataDto.Photo = PhotoSaverHelper.LoadPhoto(Point.PointDataDto.PhotoName);
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
                    }

                    context.RunOnUiThread(() =>
                        {
                            if (!string.IsNullOrWhiteSpace(error))
                            {
                                MessageBox.ShowMessage(error, context);

                                loadPhotoButton.Enabled = true;
                            }
                            else
                            {
                                try
                                {
                                    UpdatePhoto();

                                    loadPhotoButton.Visibility = ViewStates.Invisible;
                                    photo.Visibility = ViewStates.Visible;
                                }
                                catch (Throwable ex)
                                {
                                    MessageBox.ShowMessage(ex.Message, context);

                                    loadPhotoButton.Enabled = true;
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.ShowMessage(ex.Message, context);
                                    loadPhotoButton.Enabled = true;
                                }
                            }
                        });
                });
        }

        private void photo_Touch(object sender, View.TouchEventArgs e)
        {
            if (context.cameraBusy)
                return;

            context.cameraBusy = true;

            _dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), "Pictures");

            if (!_dir.Exists())
            {
                if (!_dir.Mkdirs())
                    throw new InvalidOperationException("Unable to create temporary directory");
            }

            var intent = new Intent(MediaStore.ActionImageCapture);

            imageFile = new File(_dir, string.Format("image_{0}.jpg", Guid.NewGuid()));

            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(imageFile));

            context.StartActivityForResult(intent, EditItemMainActivity.GetImageIntent);

            /*   var intent = new Intent();
               intent.SetType("image/*");
               intent.SetAction(Intent.ActionGetContent);
               context.StartActivityForResult(Intent.CreateChooser(intent, "Выберите фото"), EditItemMainActivity.GetImageIntent);*/
        }

        internal void UpdatePhoto()
        {
            byte[] data = Point.PointDataDto.Photo;
            Bitmap image = BitmapFactory.DecodeByteArray(data, 0, data.Length);

            photo.SetImageBitmap(image);
        }

        internal override void SaveToPoint()
        {
            var type = category.SelectedItemPosition;

            Point.Name = name.Text;
            Point.PointDataDto.Address = address.Text;
            Point.Category = type;

            try
            {
                var geocoder = new Geocoder(context);
                IList<Address> addresses = geocoder.GetFromLocationName(Point.PointDataDto.Address, 1);
                if (addresses.Any())
                {
                    Point.PointDataDto.Latitude = addresses.First().Latitude;
                    Point.PointDataDto.Longitude = addresses.First().Longitude;
                }
                else
                {
                    //MessageBox.ShowMessage(UnableToSaveAddress(), context);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.ShowMessage(UnableToSaveAddress() + ". Ошибка: " + ex.Message, context);
            }
        }

        private string UnableToSaveAddress()
        {
            return string.Format(
                "Невозможно распознать адрес, координаты сохранены как ({0},{1})",
                Point.PointDataDto.Latitude,
                Point.PointDataDto.Longitude);
        }
    }
}