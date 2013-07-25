using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using Common.Dto;
using Common.Services;
using Java.Lang;
using Newtonsoft.Json.Linq;
using Exception = System.Exception;
using File = Java.IO.File;
using FileNotFoundException = Java.IO.FileNotFoundException;
using IOException = Java.IO.IOException;
using Thread = System.Threading.Thread;

namespace AndroidApp.Screens
{
    [Activity(Label = "ItemsSubActivity",
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class ItemsSubActivity : Activity
    {
        private PointsSetDto points;

        private ListView items;
        private readonly PointItemsAdapter adapter;
        private Button SentToServ;

        public ItemsSubActivity()
        {
            adapter = new PointItemsAdapter(this, EditItem);
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.ItemsSubActivity);

            var addButton = FindViewById<Button>(Resource.Id.addButton);
            var refreshButton = FindViewById<Button>(Resource.Id.refreshButton);
            items = FindViewById<ListView>(Resource.Id.itemsList);
            SentToServ = FindViewById<Button>(Resource.Id.sendtoser);

            SentToServ.Click += SentToServ_Click;

            items.Adapter = adapter;
            addButton.Click += addButton_Click;

            refreshButton.Click += (sender, args) => GetItems();

            GetItems();
        }

        private void SentToServ_Click(object sender, EventArgs e)
        {
            LoadPoints();
        }

        /// <summary>
        /// отправка всех точек из файлов на сервер
        /// </summary>
        private void LoadPoints()
        {
            try
            {
                var dialog = ProgressDialog.Show(this, "Сохранение", "Пожалуйста, ждите", true);
                var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                var directory = new File(path, "/" + Resource.String.dirname);
                foreach (var file in directory.ListFiles())
                {
                    // отрываем поток для чтения
                    using (var br = new StreamReader(file.AbsolutePath))
                    {
                        var str = br.ReadToEnd();
                        var point = new PointDto();
                        point.FromJson(JObject.Parse(str));
                        save_Click(point);
                        //MessageBox.ShowMessage(file.AbsolutePath + "\n" + str, this);
                    }
                    file.Delete();
                }
                directory.Delete();
                dialog.Dismiss();
                var dial = new AlertDialog.Builder(this);
                dial.SetTitle("Сообщение");
                dial.SetMessage("Информация занесена на карту");
                dial.SetPositiveButton(Android.Resource.String.Ok, aaaa_CancelEvent);
                dial.Show();
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

        /// <summary>
        /// Метод отправки точки на сервер
        /// </summary>
        private void save_Click(PointDto point)
        {
            try
            {
                ThreadPool.QueueUserWorkItem(state =>
                    {
                        string error = null;
                        try
                        {
                            if (point.PointDataDto.HasNewPhoto && point.PointDataDto.Photo != null)
                            {
                                point.PointDataDto.PhotoName = PhotoSaverHelper.SavePhoto(point.PointDataDto.Photo);
                            }
                            if (point.Id == 0)
                            {
                                var result = PointService.NewPoint(point);
                                error = result.Result;
                                point.Id = result.ResultObjectId;
                            }
                            else
                            {
                                var result = PointService.EditPoint(point);
                                error = result.Result;
                            }

                            if (!string.IsNullOrWhiteSpace(point.PointDataDto.PhotoName) &&
                                string.IsNullOrWhiteSpace(error))
                            {
                                error = PointService.SavePhoto(point.Id, point.PointDataDto.PhotoName).Result;
                            }
                        }
                        catch (Exception ex)
                        {
                            var inner = ex;
                            while (inner.InnerException != null)
                            {
                                inner = inner.InnerException;
                            }
                            error = inner.Message;
                        }

                        RunOnUiThread(() =>
                            {
                                if (string.IsNullOrWhiteSpace(error))
                                {
                                    MessageBox.ShowMessage("Ошибка сохранения", this);
                                    return;
                                }
                                if (error.ToLower().Contains("занесена на карту"))
                                {
                                    //var dial = new AlertDialog.Builder(this);
                                    //dial.SetTitle("Сообщение");
                                    //dial.SetMessage(error);
                                    //dial.SetPositiveButton(Android.Resource.String.Ok, aaaa_CancelEvent);
                                    //dial.Show();
                                }
                                else
                                {
                                    MessageBox.ShowMessage(error, this);
                                }
                            });
                    });
            }
            catch (Throwable ex)
            {
                MessageBox.ShowMessage(ex.Message, this);
            }
            catch (Exception ex)
            {
                MessageBox.ShowMessage(ex.Message, this);
            }
        }

        private void aaaa_CancelEvent(object sender, EventArgs e)
        {
            GetItems();
        }

        public void GetItems()
        {
            var dialog = ProgressDialog.Show(this, "Обновление", "Пожалуйста, ждите", true);
            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var file = new File(path, "/" + Resource.String.dirname);
            SentToServ.Visibility = file.Exists() ? ViewStates.Visible : ViewStates.Gone;
            var thread = new Thread(() =>
                {
                    string error = null;

                    try
                    {
                        points = PointService.ListPoints();
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
                    }

                    RunOnUiThread(() =>
                        {
                            dialog.Dismiss();

                            if (!string.IsNullOrWhiteSpace(error))
                            {
                                MessageBox.ShowMessage(error, this);
                                return;
                            }

                            adapter.ReAdd(points.Points);
                        });
                });

            thread.Start();

        }

        private void EditItem(PointDto obj)
        {
            var newActivity = new Intent(this, typeof (EditItemMainActivity));
            newActivity.PutExtra(EditItemMainActivity.PointJsonExtraKey, obj.ToJson().ToString());
            StartActivity(newActivity);
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            StartActivity(typeof (EditItemMainActivity));
        }

        private sealed class PointItemsAdapter : BaseAdapter<string>
        {
            private readonly List<PointDto> points = new List<PointDto>();

            private readonly ItemsSubActivity activity;
            private readonly Action<PointDto> editAction;

            public PointItemsAdapter(ItemsSubActivity activity, Action<PointDto> editAction)
            {
                this.editAction = editAction;
                this.activity = activity;
            }

            public void ReAdd(IEnumerable<PointDto> newPoints)
            {
                points.Clear();
                points.AddRange(newPoints);

                NotifyDataSetChanged();
            }

            public override long GetItemId(int position)
            {
                return points[position].Id;
            }

            public override View GetView(int position, View convertView, ViewGroup parent)
            {
                var view = activity.LayoutInflater.Inflate(Resource.Layout.PointItemView, null);

                var editButton = view.FindViewById<Button>(Resource.Id.edit_button);

                editButton.Click += (s, a) => editAction(points[position]);

                view.FindViewById<TextView>(Resource.Id.text).Text = points[position].Name;

                return view;
            }

            public override int Count
            {
                get { return points.Count; }
            }

            public override string this[int position]
            {
                get { return points[position].Name; }
            }
        }
    }
}