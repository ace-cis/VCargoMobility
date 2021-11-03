using System;
using System.Net;
using VCargoMobility.Services;
using VCargoMobility.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VCargoMobility
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            DependencyService.Register<UserMockDataStore>();
            DependencyService.Register<CarrierMockDataStore>();
            MainPage = new LoginPage();

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
