using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace StoreWebApi.Hubs
{
    public class StoreHub : Hub<IStoreHub>
    {
        const string Customer = "Customer";
        const string Employee = "Employee";
        const string Store = "Store";

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, Store);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, Store);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SubscribeToGroup(string grupo)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, grupo);
        }

        public async Task UnsubscribeFromGroup(string grupo)
        {
            // De acuerdo a la notificación de SignalR:
            //You should not manually remove the user from the group when the user disconnects. 
            //This action is automatically performed by the SignalR framework.
            //await Groups.RemoveFromGroupAsync(Context.ConnectionId, grupo);
        }

        public async Task PostNewInfo(DTOs.CustomerOrderDTO order, string grupo)
        {
            await Clients.Group(grupo).NotifyNewInfo(order);
        }
    }
}
