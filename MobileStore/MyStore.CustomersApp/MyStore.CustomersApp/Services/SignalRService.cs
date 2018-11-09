using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using MyStore.CustomersApp.Notifications;
using MyStore.Models;
using MyStore.Services;

namespace MyStore.CustomersApp.Services
{
    public class SignalRService
    {
        HubConnection Connection;

        public async Task<bool> InitCustomer(CustomerDTO customer)
        {
            try
            {
                Connection = new HubConnectionBuilder()
                    .WithUrl(Constants.SignalRURL)
                    .Build();

                // Client: NotifyNewInfo
                Connection.On<CustomerOrderDTO>(Constants.ClientMethod, (order) =>
                {
                    switch (order.OrderStatus.Name)
                    {
                        case "1-Unpaid":
                        case "2-Paid":
                            NotifyEmployee(order);
                            break;
                        case "3-Processing":
                        case "4-Shipped":
                        case "5-Delivered":
                            NotifyCustomer(order);
                            break;
                    }
                });

                await Connection.StartAsync();

                Connection.Closed += async (exception) =>
                {
                    TimeSpan retryDuration = TimeSpan.FromSeconds(30);

                    while (DateTime.UtcNow < DateTime.UtcNow.Add(retryDuration))
                    {
                        bool connected = await InitCustomer(App.CurrentCustomer);
                        if (connected)
                            return;
                    }
                };

                await Suscribe($"{Constants.Customer}-{customer.Id}");

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        private Task Connection_Closed(Exception arg)
        {
            
            throw new NotImplementedException();
        }

        public async Task Suscribe(string group)
        {
            await Connection.InvokeAsync(Constants.ServerSubscribeMethod, group);
        }

        public async Task Unsuscribe(string group)
        {
            await Connection.InvokeAsync(Constants.ServerUnsuscribeMethod, group);
        }

        public async Task Notify(CustomerOrderDTO order, string group)
        {
            try
            {
                await Connection.InvokeAsync(Constants.ServerMethod, order, group);
            }
            catch(Exception ex)
            {

            }
        }

        public void NotifyEmployee(CustomerOrderDTO order)
        {
            StoreNotification.Notify(order, $"An order has a new status: {order.OrderStatus.Name}. Please review it.");
        }

        public void NotifyCustomer(CustomerOrderDTO order)
        {
            StoreNotification.Notify(order, $"An employee has updated your oder with the status: {order.OrderStatus.Name}.");
        }
    }
}