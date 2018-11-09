using System;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MyStore.Models;
using MyStore.Services;

namespace MyStore.CustomersApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CustomerOrderPage : ContentPage
	{
        public CustomerOrderPage(CustomerOrderDTO item)
        {
            InitializeComponent();
            LoadItem(item);
        }

        public async Task LoadItem(CustomerOrderDTO item)
        {
            this.BindingContext = item;
            var list = await StoreWebApiClient.Instance.GetItems<OrderStatusDTO>("OrderStatuses");

            if (item.Id == 0)
            {
                this.ToolbarItems.RemoveAt(1);

                var unpaidStatus = list.First(x => x.Name == "1-Unpaid");
                item.OrderStatusId = unpaidStatus.Id;
                item.OrderStatus = unpaidStatus;
            }
            
            StatusPicker.ItemsSource = list;
            StatusPicker.ItemDisplayBinding = new Binding("Name");
            StatusPicker.SelectedItem = list.First(x => x.Id == item.OrderStatusId);
        }

        void Loading(bool show)
        {
            indicator.IsEnabled = show;
            indicator.IsRunning = show;
        }

        async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            Loading(true);
            var item = (CustomerOrderDTO)this.BindingContext;

            if (item.Id > 0)
                await StoreWebApiClient.Instance.PutItem<CustomerOrderDTO>("CustomerOrders", item, item.Id);
            else
                await StoreWebApiClient.Instance.PostItem<CustomerOrderDTO>("CustomerOrders", item);

            Loading(false);

            await DisplayAlert("Success", "Item registered!", "OK");
            await NotifyOrder(item);
            await Navigation.PopAsync();
        }

        async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Warning!", "Do you really want to delete this item?", "Yes", "No"))
            {
                Loading(true);

                var item = (CustomerOrderDTO)this.BindingContext;

                var op = await StoreWebApiClient.Instance.DeleteItem<CustomerOrderDTO>("CustomerOrders", item.Id);

                Loading(false);

                if (op)
                {
                    await DisplayAlert("Success!", "Item deleted!", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error!", "Item was not deleted!", "OK");
                }
            }
        }

        async Task NotifyOrder(CustomerOrderDTO order)
        {
            if (order.OrderStatus.Name == "1-Unpaid" 
                || order.OrderStatus.Name == "2-Paid")
            {
                await App.SignalRService.Notify(order, Constants.Employee);
            }
        }
    }
}