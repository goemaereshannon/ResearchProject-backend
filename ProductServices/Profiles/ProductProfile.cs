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
        }
    }
}
