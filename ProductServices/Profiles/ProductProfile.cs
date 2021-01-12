using AutoMapper;
using ProductServices.DTOs;
using ProductServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductServices.Profiles
{
    public class ProductProfile: Profile
    {
        public ProductProfile()
        {
            InitProductMapper(); 
        }
        private void InitProductMapper()
        {
            CreateMap<ProductDTO, Product>().ReverseMap();
            CreateMap<CategoryDTO, Category>().ReverseMap();
            CreateMap<PriceDTO, Price>().ReverseMap();
            CreateMap<ProductHasPropertyDTO, ProductHasProperty>().ReverseMap();
            CreateMap<ProductHasSizeDTO, ProductHasSize>().ReverseMap();
            CreateMap<PropertyDTO, Property>().ReverseMap();
            CreateMap<PropertyValueDTO, PropertyValue>().ReverseMap();
            CreateMap<SizeDTO, Size>().ReverseMap();
            CreateMap<SubcategoryDTO, Subcategory>().ReverseMap();


        }
    }
}
