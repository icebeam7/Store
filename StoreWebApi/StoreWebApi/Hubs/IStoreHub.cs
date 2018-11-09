using System.Threading.Tasks;

namespace StoreWebApi.Hubs
{
    public interface IStoreHub
    {
        Task NotifyNewInfo(DTOs.CustomerOrderDTO order);
    }
}
