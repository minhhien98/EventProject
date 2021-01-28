using AutoMapper;
using DomainModel.Entities;
using EventWeb.Models.ViewModel.EventVM;
using EventWeb.Models.ViewModel.UserVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventWeb.Models.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Mapping
            CreateMap<Event,EventVM>().ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.Id)).ReverseMap();
            //CreateMap<DateTimeOffset, DateTime>().ReverseMap();
            CreateMap<User,UserVM>().ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id)).ReverseMap();
        }
    }
}
