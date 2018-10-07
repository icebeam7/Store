using AutoMapper;
using StoreWebApi.Models;

namespace StoreWebApi.DTOs
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Customer, CustomerDTO>()
                   .ForMember(x => x.CustomerOrder, o => o.Ignore())
                   .ReverseMap();

                cfg.CreateMap<CustomerOrder, CustomerOrderDTO>()
                   .ForMember(x => x.OrderDetail, o => o.Ignore())
                   .ReverseMap();

                cfg.CreateMap<Employee, EmployeeDTO>()
                   .ReverseMap();

                cfg.CreateMap<OrderDetail, OrderDetailDTO>()
                   .ReverseMap();

                cfg.CreateMap<OrderStatus, OrderStatusDTO>()
                   .ForMember(x => x.CustomerOrder, o => o.Ignore())
                   .ReverseMap();

                cfg.CreateMap<Product, ProductDTO>()
                   .ForMember(x => x.OrderDetail, o => o.Ignore())
                   .ReverseMap();
            });
        }
    }
}
