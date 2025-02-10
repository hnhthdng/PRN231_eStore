using AutoMapper;
using BusinessObject.Models;
using DataAccess.DTO.Category;
using DataAccess.DTO.Member;
using DataAccess.DTO.Order;
using DataAccess.DTO.OrderDetail;
using DataAccess.DTO.Product;

namespace DataAccess.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile() {
            CreateMap<Category, CategoryResponseDTO>();
            CreateMap<CategoryRequestDTO, Category>();

            CreateMap<Product, ProductResponseDTO>();
            CreateMap<ProductRequestDTO, Product>();
            
            CreateMap<Member, MemberResponseDTO>();
            CreateMap<MemberRequestDTO, Member>();

            CreateMap<Order, OrderResponseDTO>();
            CreateMap<OrderRequestDTO, Order>();

            CreateMap<OrderDetail, OrderDetailResponseDTO>();
            CreateMap<OrderDetailRequestDTO, OrderDetail>();
        }
    }
}
