using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MyStore.Models;
using MyStore.Services;

namespace MyStore.EmployeesApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomerPage : ContentPage
    {
        public CustomerPage(CustomerDTO item)
        {
            InitializeComponent();

            this.BindingContext = item;
            if (item.Id == 0)
            {
                this.ToolbarItems.RemoveAt(2);
                this.ToolbarItems.RemoveAt(1);
            }
        }

        void Loading(bool show)
        {
            indicator.IsEnabled = show;
            indicator.IsRunning = show;
        }

        async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            Loading(true);
            var item = (CustomerDTO)this.BindingContext;

            if (item.Id > 0)
                await StoreWebApiClient.Instance.PutItem<CustomerDTO>("Customers", item, item.Id);
            else
            {
                item.Password = MD5Security.ToMD5Hash("abc");
                await StoreWebApiClient.Instance.PostItem<CustomerDTO>("Customers", item);
            }

            Loading(false);

            await DisplayAlert("Success", "Item registered!", "OK");
            await Navigation.PopAsync();
        }

        async void ViewCustomerOrdersButton_Clicked(object sender, EventArgs e)
        {
            var item = (CustomerDTO)this.BindingContext;
            await Navigation.PushAsync(new CustomerOrderListPage(item));
        }

        async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Warning!", "Do you really want to delete this item?", "Yes", "No"))
            {
                Loading(true);

                var item = (CustomerDTO)this.BindingContext;

                var op = await StoreWebApiClient.Instance.DeleteItem<CustomerDTO>("Customers", item.Id);

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
    }
}