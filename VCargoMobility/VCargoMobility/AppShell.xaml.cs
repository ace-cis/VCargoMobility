using System;
using System.Collections.Generic;
using System.Net;

using VCargoMobility.Services;
using VCargoMobility.ViewModels;
using VCargoMobility.Views;
using Xamarin.Forms;

namespace VCargoMobility
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SP_Page), typeof(SP_Page));
            
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }

        public NavigationPage MainPage { get; }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
