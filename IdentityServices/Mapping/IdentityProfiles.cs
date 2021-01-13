using AutoMapper;
using IdentityServices.DTOs;
using IdentityServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServices.Mapping
{
    public class IdentityProfiles: Profile
    {
        public IdentityProfiles()
        {

            InitUserMApper();


        }

        private void InitUserMApper()
        {
            CreateMap<UserDTO, User>().ReverseMap(); 

            CreateMap<User, RegisterDTO>();
            CreateMap<RegisterDTO, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(model => model.Email));
        }
    }
}
