using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VCargoMobility.Views;
using Xamarin.Forms;

namespace VCargoMobility.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }

        //public Command LoginCommand { get; }

        public string _UserName;
        public string UserName
        {
            get => _UserName;
            set
            {
                _UserName = value;

            }
        }

        public string _Password;
        public string Password
        {
            get => _Password;
            set
            {
                _Password = value;

            }
        }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);

            // LoginCommand = new Command(async () => await OnLoginClickedAsync());
        }

        //private void OnLoginClicked(object obj)
        //{


        //    // Application.Current.MainPage = new AppShell();
        //    //return;

        //    using (var client = new System.Net.Http.HttpClient())
        //    {

        //        // HTTP GET
        //        client.BaseAddress = new Uri("http://192.168.0.24:80/");
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
        //        var response = client.GetAsync("/api/WarehouseLogin/" + _UserName).Result;

        //        using (HttpContent content = response.Content)
        //        {
        //            // ... Read the string.
        //            Task<string> result = content.ReadAsStringAsync();

        //            if (response.IsSuccessStatusCode)
        //            {
        //                CultureInfo culture_info = Thread.CurrentThread.CurrentCulture;
        //                TextInfo text_info = culture_info.TextInfo;


        //                XmlDocument xmlDoc = new XmlDocument();
        //                xmlDoc.LoadXml(result.Result);


        //                XmlNodeList UserdIdx = xmlDoc.DocumentElement.GetElementsByTagName("TLC_UserID");
        //                XmlNodeList Passwordx = xmlDoc.DocumentElement.GetElementsByTagName("TLC_Password");

        //                string UserNameM = UserdIdx[0].InnerXml;
        //                string PasswordM = Passwordx[0].InnerXml;


        //                UserNameM = text_info.ToLower(UserNameM);
        //                //_UserName = text_info.ToLower(_UserName);
        //                PasswordM = text_info.ToLower(PasswordM);

        //                if (_UserName == UserNameM && _Password == "1234")
        //                {
        //                    userNameX = _UserName;
        //                    Application.Current.MainPage.DisplayAlert("Message", " Login successfully.", "Ok");
        //                    Application.Current.MainPage = new AppShell(_UserName);
        //                }
        //                else
        //                {
        //                    Application.Current.MainPage.DisplayAlert("Message", " Login failed!", "Ok");
        //                }

        //            }

        //        }
        //    }

        //}



        //public LoginViewModel()
        //{
        //    LoginCommand = new Command(OnLoginClicked);
        //}
        private async void OnLoginClicked(object obj)
        {

            if (_UserName == "" || _UserName == null)
            {
                await Application.Current.MainPage.DisplayAlert("VCargo", "Login Failed!", "Ok");
                return;
            }

            if (_Password == "" || _Password == null)
            {
                await Application.Current.MainPage.DisplayAlert("VCargo", "Login Failed!", "Ok");
                return;
            }


            // var user = await UserDataStore.GetUserAsync(true);
            var userValidation = await UserDataStore.GetUserAsync(_UserName);

            string UserX = userValidation.userCode;
            string PasswordX = userValidation.pssword;
            string CustomerType = userValidation.position;

            if (UserX.ToString() == _UserName && PasswordX.ToString() == _Password)
            {
                if (CustomerType == "Shipping Agent" || CustomerType == "SHP Coordinator")
                {


                    Application.Current.Properties["UserCode"] = _UserName;
                    Application.Current.Properties["UserType"] = CustomerType;
                    await Application.Current.MainPage.DisplayAlert("Message", " Login successfully.", "Ok");


                    Application.Current.MainPage = new AppShell();



                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Message", "This application is for Shipping Agent and Shipping Coordinator only.", "Ok");
                    return;
                }




            }
            else
            {

                await Application.Current.MainPage.DisplayAlert("VCargo", " Login failed!", "Ok");
            }



           
        }



    }
}
