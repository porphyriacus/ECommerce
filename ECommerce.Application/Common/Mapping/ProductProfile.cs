using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ECommerce.Application.Features.Product;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Common.Mapping
{
    public class ProductProfile : Profile 
    {
        public ProductProfile() {
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.ReviewsCount,
                           opt => opt.MapFrom(src => src.Reviews.Count))
                .ForMember(dest => dest.PriceCurrency,
                           opt => opt.MapFrom(src => src.Price.Currency))
                .ForMember(dest => dest.PriceAmount,
                           opt => opt.MapFrom(src => src.Price.Amount))
                .ForMember(dest => dest.CategoryId,
                           opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.CategoryName,
                           opt => opt.MapFrom(src => src.Category.Name));
        }
    }
}
