using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MyStore.Models;

namespace MyStore.EmployeesApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomerOrderListPage : ContentPage
    {
        public CustomerOrderListPage(CustomerDTO item)
        {
            InitializeComponent();
        }
    }
}