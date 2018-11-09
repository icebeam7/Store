using System;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MyStore.Models;
using MyStore.Services;

namespace MyStore.EmployeesApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CustomerOrderPage : ContentPage
	{
        public CustomerOrderPage (CustomerOrderDTO item)
		{
			InitializeComponent ();
            LoadItem(item);
        }

        public async Task LoadItem(CustomerOrderDTO item)
        {
            this.BindingContext = item;
            var list = await StoreWebApiClient.Instance.GetItems<OrderStatusDTO>("OrderStatuses");

            StatusPicker.ItemsSource = list;
            StatusPicker.ItemDisplayBinding = new Binding("Name");
            StatusPicker.SelectedItem = list.First(x => x.Id == item.OrderStatusId);
        }

        void Loading(bool show)
        {
            indicator.IsEnabled = show;
            indicator.IsRunning = show;
        }

        async void UpdateStatusButton_Clicked(object sender, EventArgs e)
        {
            Loading(true);
            var item = (CustomerOrderDTO)this.BindingContext;
            item.OrderStatus = StatusPicker.SelectedItem as OrderStatusDTO;
            item.OrderStatusId = item.OrderStatus.Id;

            if (item.Id > 0)
                await StoreWebApiClient.Instance.PutItem<CustomerOrderDTO>("CustomerOrders", item, item.Id);

            Loading(false);

            await DisplayAlert("Success", "Item registered!", "OK");
            await NotifyOrder(item);
            await Navigation.PopAsync();
        }

        async Task NotifyOrder(CustomerOrderDTO order)
        {
            if (!(order.OrderStatus.Name == "1-Unpaid" 
                || order.OrderStatus.Name == "2-Paid"))
            {
                await App.SignalRService.Notify(order, $"{Constants.Customer}-{order.CustomerId}");
            }
        }
    }
}