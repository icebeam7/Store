using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MyStore.Models;
using MyStore.Services;

namespace MyStore.CustomersApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CustomerOrderListPage : ContentPage
	{
        CustomerDTO Item;

        public CustomerOrderListPage ()
		{
			InitializeComponent ();

            Item = App.CurrentCustomer;
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

        public async void AddButton_Clicked(object sender, EventArgs e)
        {
            var item = new CustomerOrderDTO()
            {
                Id = 0,
                Amount = 0,
                Customer = App.CurrentCustomer,
                CustomerId = App.CurrentCustomer.Id,
                Date = DateTime.UtcNow                
            };

            await Navigation.PushAsync(new CustomerOrderPage(item));
        }
    }
}