using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MyStore.Services;
using MyStore.Models;
using MyStore.CustomersApp.Services;

namespace MyStore.CustomersApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();
		}

        public async void SignInButton_Clicked(object sender, EventArgs e)
        {
            var username = UserNameEntry.Text;
            var password = PasswordEntry.Text;

            var customer = await StoreWebApiClient.Instance.LoginCustomer(username, password);

            if (customer != default(CustomerDTO))
            {
                App.CurrentCustomer = customer;

                App.SignalRService = new SignalRService();
                await App.SignalRService.InitCustomer(App.CurrentCustomer);

                await DisplayAlert("Welcome!", $"Welcome {customer.FullName}", "OK");
                await Navigation.PushAsync(new CustomerOrderListPage());
            }
            else
            {
                await DisplayAlert("Error!", "Wrong credentials!", "OK");
            }
        }
    }
}