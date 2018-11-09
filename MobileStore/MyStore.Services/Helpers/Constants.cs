namespace MyStore.Services
{
    public class Constants
    {
        public const string StoreWebApiURL = "http://IPServidor:5001/api/";

        #region SignalR Constants
        // URL
        public const string SignalRURL = "http://IPServidor:5001/StoreHub";

        // Groups
        public const string Customer = "Customer";
        public const string Employee = "Employee";
        public const string Store = "Store";

        // Server Methods 
        public static readonly string ServerSubscribeMethod = "SubscribeToGroup";
        public static readonly string ServerUnsuscribeMethod = "UnsubscribeFromGroup";
        public static readonly string ServerMethod = "PostNewInfo";

        // Client Methods
        public static readonly string ClientMethod = "NotifyNewInfo";
        public static readonly string ClientSubscribeMethod = "Subscribe";
        public static readonly string ClientUnsuscribeMethod = "Unsuscribe";

        #endregion
    }
}

