using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DCTMRestAPI.Models;
using DCTMRestAPI.Models.Custom;

namespace DCTMRestAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TblLocation, LocationDTO>().ReverseMap();
        }
    }
}
