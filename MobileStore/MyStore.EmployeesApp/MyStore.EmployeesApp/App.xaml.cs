using System;

using Xamarin.Forms;

using MyStore.Models;
using MyStore.EmployeesApp.Services;

namespace MyStore.EmployeesApp
{
    public partial class App : Application
    {
        public static EmployeeDTO CurrentEmployee;
        public static SignalRService SignalRService;

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Pages.LoginPage());
        }
    }
}
