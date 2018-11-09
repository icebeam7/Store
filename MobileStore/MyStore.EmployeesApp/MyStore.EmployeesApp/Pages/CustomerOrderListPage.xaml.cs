using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MyStore.Models;
using MyStore.Services;

namespace MyStore.EmployeesApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomerOrderListPage : ContentPage
    {
        CustomerDTO Item;

        public CustomerOrderListPage(CustomerDTO item)
        {
            InitializeComponent();

            Item = item;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            Loading(true);
            CustomerOrdersListView.ItemsSource = await StoreWebApiClient.Instance.GetItems<CustomerOrderDTO>("CustomerOrders", $"Customer/{Item.Id}");
            Loading(false);
        }

        void Loading(bool show)
        {
            indicator.IsEnabled = show;
            indicator.IsRunning = show;
        }

        private async void CustomerOrdersListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem != null)
                {
                    var item = (CustomerOrderDTO)e.SelectedItem;
                    await Navigation.PushAsync(new CustomerOrderPage(item));
                    CustomerOrdersListView.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}