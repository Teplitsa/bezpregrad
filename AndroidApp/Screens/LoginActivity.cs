using System;
using System.IO;
using System.Net;
using Android.App;
using Android.Content.PM;
using Android.Widget;
using Android.OS;
using Common.Dto;
using Common.Services;
using Java.Lang;
using Newtonsoft.Json.Linq;
using Exception = System.Exception;
using Thread = System.Threading.Thread;

namespace AndroidApp.Screens
{
    [Activity(Label = "Без преград", MainLauncher = true,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public sealed class LoginActivity : Activity
    {
        private EditText loginText;
        private EditText passwordText;
        private Button loginButton;
        private CheckBox savePassword;
        private Button developerButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.LoginActivity);

            loginText = FindViewById<EditText>(Resource.Id.loginText);
            passwordText = FindViewById<EditText>(Resource.Id.passwordText);
            savePassword = FindViewById<CheckBox>(Resource.Id.savePassword);

            savePassword.Checked = true;

//#if DEBUG
//            loginText.Text = "admin";
//            passwordText.Text = "123";
//#endif

            loginButton = FindViewById<Button>(Resource.Id.loginButton);
            loginButton.Click += loginButton_Click;
            developerButton = FindViewById<Button>(Resource.Id.developerButton);
            developerButton.Click += developerButton_Click;

            if (File.Exists(AuthJson))
            {
                using (var reader = new StreamReader(File.OpenRead(AuthJson)))
                {
                    string serialized = reader.ReadToEnd();

                    var dto = new LoginRequestDto();

                    dto.FromJson(JObject.Parse(serialized));

                    loginText.Text = dto.Login;
                    passwordText.Text = dto.Password;
                }
            }
        }

        private void developerButton_Click(object sender, EventArgs e)
        {
            StartActivity(typeof (Developer));
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            string login = loginText.Text;
            string password = passwordText.Text;

            ProgressDialog dialog = ProgressDialog.Show(this, "Вход", "Пожалуйста, ждите", true);
            //loginButton.Enabled = false;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                dialog.Dismiss();
                MessageBox.ShowMessage("Введите логин и пароль", this);

                //loginButton.Enabled = true;
                return;
            }
            new Thread(() =>
                {
                    string error = null;

                    try
                    {
                        var loginRequestData = new LoginRequestDto
                            {
                                Login = login,
                                Password = password
                            };

                        bool res = LoginService.Login(loginRequestData);

                        if (!res)
                            error = "Логин или пароль неверен";
                    }
                    catch (WebException ex)
                    {
                        error = ex.Message;
                    }
                    catch (Exception ex)
                    {
                        Exception inner = ex;
                        while (inner.InnerException != null)
                        {
                            inner = inner.InnerException;
                        }
                        error = inner.Message;
                    }

                    RunOnUiThread(() =>
                        {
                            //loginButton.Enabled = true;
                            dialog.Dismiss();
                            if (!string.IsNullOrWhiteSpace(error))
                            {
                                MessageBox.ShowMessage(error, this);
                                return;
                            }
                            SaveLoginData();
                            StartActivity(typeof (MainActivity));
                        });
                }).Start();
        }

        private void SaveLoginData()
        {
            try
            {
                if (File.Exists(AuthJson))
                {
                    File.Delete(AuthJson);
                }

                if (savePassword.Checked)
                {
                    string login = loginText.Text;
                    string password = passwordText.Text;

                    var loginRequestData = new LoginRequestDto()
                        {
                            Login = login,
                            Password = password
                        };

                    using (var writer = new StreamWriter(File.Create(AuthJson)))
                    {
                        writer.Write(loginRequestData.ToJson().ToString());
                    }
                }
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

        private string AuthJson
        {
            get { return FilesDir + "questions_auth.json"; }
        }
    }
}