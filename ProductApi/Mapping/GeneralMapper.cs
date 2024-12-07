using AutoMapper;
using ProductApi.Dtos.CategoryDtos;
using ProductApi.Dtos.ProductDtos;
using ProductApi.Entities;

namespace ProductApi.Mapping
{
    public class GeneralMapper:Profile
    {
        public GeneralMapper() {
            CreateMap<Category, ResultCategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
            CreateMap<Category, GetByIdCategoryDto>().ReverseMap();

            CreateMap<Product, ResultProductDto>().ReverseMap();
            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap();
            CreateMap<Product, GetByIdProductDto>().ReverseMap();
        }
    }
}
