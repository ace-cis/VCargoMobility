using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCargoMobility.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VCargoMobility.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SP_Page : TabbedPage
    {
        SP_ViewModel _viewModel;
        public SP_Page()
        {
            InitializeComponent();
            BindingContext = _viewModel = new SP_ViewModel();
            CurrentPageChanged += CurrentPageHasChanged;

            if (Application.Current.Properties["UserType"].ToString() == "SHP Coordinator")
            {
               
            }
            else
            {

              
            }



           






        }
        private void CurrentPageHasChanged(object sender, EventArgs e)
        {
            var tabbedPage = (TabbedPage)sender;
            Title = tabbedPage.CurrentPage.Title;

        }

       
       
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();

          

            
            //App.HardwareBackPressed = () => Task.FromResult<bool?>(null);
        }

        protected override bool OnBackButtonPressed()
        {
            //_viewModel.OnClickbackAsync();



            //  base.OnBackButtonPressed();
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (await DisplayAlert("Exit?", "Are you sure you want to exit from this page?", "Yes", "No"))
                {
                    base.OnBackButtonPressed();
                    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                    Application.Current.Properties["UserCode"]="";
                    Application.Current.Properties["UserType"] ="";
                }
            });

            return true;
        }

        private void CourierPicker_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}