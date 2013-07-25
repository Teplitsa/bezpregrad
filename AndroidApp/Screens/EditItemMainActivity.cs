using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidApp.Screens.Edit;
using Common;
using Common.Dto;
using Java.Lang;
using Newtonsoft.Json.Linq;
using Exception = System.Exception;
using File = Java.IO.File;
using FileNotFoundException = Java.IO.FileNotFoundException;
using IOException = Java.IO.IOException;

namespace AndroidApp.Screens
{
    [Activity(Label = "Без преград", ScreenOrientation = ScreenOrientation.Portrait)]
    public sealed class EditItemMainActivity : Activity
    {
        internal const int GetImageIntent = 0;

        internal const string PointJsonExtraKey = "data";

        private PointDto point;

        private Button nextButton;
        private Button backButton;
        private Button objectsButton;
        private Button saveButton;

        private FrameLayout contentLayout;

        private int viewIndex = 0;

        private readonly List<BaseScreen> views = new List<BaseScreen>();
        private Screen1Main screen1Main;

        internal bool cameraBusy = false;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Title = "Без преград";

            if (AuthData.AuthResult == null)
            {
                StartActivity(typeof (LoginActivity));
                FinishActivity(0);

                return;
            }

            try
            {
                ReadPoint();

                SetContentView(Resource.Layout.EditItemMainActivity);

                nextButton = FindViewById<Button>(Resource.Id.next_button);
                backButton = FindViewById<Button>(Resource.Id.back_button);
                objectsButton = FindViewById<Button>(Resource.Id.objects_button);
                saveButton = FindViewById<Button>(Resource.Id.save_button);
                contentLayout = FindViewById<FrameLayout>(Resource.Id.content);

                saveButton.Click += saveButton_Click;
                objectsButton.Click += objectsButton_Click;

                nextButton.Click += nextButton_Click;
                backButton.Click += backButton_Click;

                CreateViews();

                foreach (var screen in views)
                {
                    contentLayout.AddView(screen.View);
                }

                UpdateViewAndButtons();
            }
            catch (Throwable t)
            {
                MessageBox.ShowMessage(t.Message, this);
            }
        }

        private void CreateViews()
        {
            screen1Main = new Screen1Main(this, point);

            views.Add(screen1Main);
            views.Add(new Screen2Invalid(this, point));
            views.Add(new Screen3Entry(this, point));
            views.Add(new Screen4Lobby(this, point));
            views.Add(new Screen5Info(this, point));
            views.Add(new Screen6Hygiene(this, point));
            views.Add(new Screen7Summary(this, point));
        }

        private void ReadPoint()
        {
            string json = Intent.GetStringExtra(PointJsonExtraKey);

            point = new PointDto();

            if (json != null)
            {
                point.FromJson(JObject.Parse(json));
            }
        }

        private void UpdateViewAndButtons()
        {
            switch (viewIndex)
            {
                case 1:
                    Title = "Без преград: возле здания";
                    break;
                case 2:
                    Title = "Без преград: вход в здание";
                    break;
                case 3:
                    Title = "Без преград: внутри здания";
                    break;
                case 4:
                    Title = "Без преград: информация в здании";
                    break;
                case 5:
                    Title = "Без преград: санузел в здании";
                    break;
                default:
                    Title = "Без преград";
                    break;
            }
            if (viewIndex == 0)
            {
                backButton.Visibility = ViewStates.Gone;
                objectsButton.Visibility = ViewStates.Visible;
                nextButton.Visibility = ViewStates.Visible;
                saveButton.Visibility = ViewStates.Gone;
            }
            else if (viewIndex + 1 >= views.Count)
            {
                backButton.Visibility = ViewStates.Visible;
                objectsButton.Visibility = ViewStates.Gone;
                nextButton.Visibility = ViewStates.Gone;
                saveButton.Visibility = ViewStates.Visible;
            }
            else if (viewIndex > 0 && viewIndex + 1 < views.Count)
            {
                backButton.Visibility = ViewStates.Visible;
                objectsButton.Visibility = ViewStates.Gone;
                nextButton.Visibility = ViewStates.Visible;
                saveButton.Visibility = ViewStates.Gone;
            }

            foreach (BaseScreen screen in views)
            {
                screen.View.Visibility = ViewStates.Invisible;
            }

            views[viewIndex].View.Visibility = ViewStates.Visible;
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            if (viewIndex == 0)
                return;

            viewIndex--;
            contentLayout.BringChildToFront(views[viewIndex].View);
            UpdateViewAndButtons();
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            if (viewIndex + 1 >= views.Count)
                return;

            viewIndex++;
            contentLayout.BringChildToFront(views[viewIndex].View);
            UpdateViewAndButtons();
        }

        private void objectsButton_Click(object sender, EventArgs e)
        {
            Finish();
        }

        /// <summary>
        /// Событие нажатия кнопки сохранить. Точка сохраняется в файл.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveButton_Click(object sender, EventArgs e)
        {
            var dialog = ProgressDialog.Show(this, "Сохранение", "Пожалуйста, ждите", true);
            SaveAllToPoint();
            SavePoint(point);
            dialog.Dismiss();
            var dial = new AlertDialog.Builder(this);
            dial.SetTitle("Сообщение");
            dial.SetMessage("Точка сохранена в файл");
            dial.SetPositiveButton(Android.Resource.String.Ok, aaaa_CancelEvent);
            dial.Show();
        }

        private void aaaa_CancelEvent(object sender, EventArgs e)
        {
            Finish();
        }

        /// <summary>
        /// сохранение точки в файл
        /// </summary>
        /// <param name="pointDto">точка</param>
        private void SavePoint(PointDto pointDto)
        {
            try
            {
                var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                var file = new File(path, "/" + Resource.String.dirname);
                if (!file.Exists())
                {
                    file.Mkdirs();
                }
                File sdFile;
                string nameFile;
                if (point.Id > 0)
                {
                    nameFile = point.Id.ToString(CultureInfo.InvariantCulture);
                    sdFile = new File(file, nameFile);
                }
                else
                    do
                    {
                        nameFile = "NewPoint_" + Guid.NewGuid();
                        sdFile = new File(file, nameFile);
                    } while (sdFile.Exists());

                using (var bw = new StreamWriter(sdFile.AbsolutePath))
                {
                    bw.Write(pointDto.ToJson().ToString());
                    bw.Close();
                }
            }
            catch (FileNotFoundException e)
            {
                e.PrintStackTrace();
                var inner1 = e;
                MessageBox.ShowMessage("FileNotFoundException: " + inner1.Message, this);

            }
            catch (IOException e)
            {
                e.PrintStackTrace();
                var inner = e;
                MessageBox.ShowMessage("IOException: " + inner.Message, this);

            }
            catch (Exception e)
            {
                var inner = e;

                while (inner.InnerException != null)
                {
                    inner = inner.InnerException;
                }

                MessageBox.ShowMessage(inner.Message + inner.StackTrace, this);
            }
        }

        private void SaveAllToPoint()
        {
            foreach (var screen in views)
            {
                screen.SaveToPoint();
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == GetImageIntent)
            {
                cameraBusy = false;

                if (resultCode != Result.Ok)
                    return;

                string filePath = screen1Main.imageFile.AbsolutePath;

                byte[] image = System.IO.File.ReadAllBytes(filePath);

                point.PointDataDto.Photo = image;
                point.PointDataDto.HasNewPhoto = true;


                screen1Main.UpdatePhoto();

                /*                   
                    Android.Net.Uri photoUri = data.Data;
                    if (photoUri == null)
                        return;

                    var filePathColumn = new[] { MediaStore.Images.ImageColumns.Data };
                    using (ICursor cursor = ContentResolver.Query(photoUri, filePathColumn, null, null, null))
                    {
                        cursor.MoveToFirst();
                        int columnIndex = cursor.GetColumnIndex(filePathColumn[0]);
                        string filePath = cursor.GetString(columnIndex);

                        byte[] image = System.IO.File.ReadAllBytes(filePath);

                        point.PointDataDto.PhotoName = Convert.ToBase64String(image);

                        screen1Main.UpdatePhoto(point.PointDataDto.PhotoName);
                
                }*/
            }
        }
    }
}