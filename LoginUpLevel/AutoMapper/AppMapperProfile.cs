using AutoMapper;
using LoginUpLevel.DTOs;
using LoginUpLevel.Models;
using System.Globalization;

namespace MovieTheaterAPI.AutoMapper
{
    public class AppMapperProfile : Profile
    {
        public AppMapperProfile()
        {
            CreateMap<string, TimeOnly>().ConvertUsing(src => TimeOnly.Parse(src));
            CreateMap<string, DateOnly>().ConvertUsing(src => DateOnly.Parse(src, CultureInfo.InvariantCulture));
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<Employee, EmployeeDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailDTO>().ReverseMap();
            CreateMap<OrderAdress, OrderAdressDTO>().ReverseMap();
            CreateMap<Cart, CartDTO>().ReverseMap();
            CreateMap<CartItem, CartItemDTO>().ReverseMap();
            CreateMap<Color, ColorDTO>().ReverseMap();
            CreateMap<ProductColor, ProductColorDTO>().ReverseMap();
            CreateMap<Comment, CommentDTO>().ReverseMap();
        }
    }
}
