using System;

using Xamarin.Forms;

using MyStore.Models;
using MyStore.CustomersApp.Services;

namespace MyStore.CustomersApp
{
	public partial class App : Application
	{
        public static CustomerDTO CurrentCustomer;
        public static SignalRService SignalRService;

        public App ()
		{
			InitializeComponent();

            MainPage = new NavigationPage(new Pages.LoginPage());
		}
	}
}
