using MyStore.Models;
using Plugin.LocalNotifications;

namespace MyStore.CustomersApp.Notifications
{
    public static class StoreNotification
    {
        public static void Notify(CustomerOrderDTO order, string message)
        {
            CrossLocalNotifications.Current.Show("Store Notification", message);
        }
    }
}
