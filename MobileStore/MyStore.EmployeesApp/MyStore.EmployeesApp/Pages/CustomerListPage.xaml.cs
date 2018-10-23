using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MyStore.Models;
using MyStore.Services;

namespace MyStore.EmployeesApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomerListPage : ContentPage
    {
        public CustomerListPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Loading(true);
            CustomersListView.ItemsSource = await StoreWebApiClient.Instance.GetItems<CustomerDTO>("Customers");
            Loading(false);
        }

        void Loading(bool show)
        {
            indicator.IsEnabled = show;
            indicator.IsRunning = show;
        }

        private async void CustomersListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var item = (CustomerDTO)e.SelectedItem;
                await Navigation.PushAsync(new CustomerPage(item));
                CustomersListView.SelectedItem = null;
            }
            catch (Exception ex)
            {
            }
        }

        public async void AddButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CustomerPage(new CustomerDTO()));
        }
    }
}