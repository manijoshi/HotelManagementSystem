

using AutoMapper;
using HotelBookingSystem.Application.DTO.HotelDTO;
using HotelBookingSystem.Domain.Entities;

namespace HotelBookingSystem.Application.MappingProfiles
{
    public class HotelProfile : Profile
    {
        public HotelProfile() 
        {
            CreateMap<HotelRequest, Hotel>();
            CreateMap<Hotel, HotelResponseWithReviews>()
                .ForMember(dest => dest.GuestReviews, opt => opt.MapFrom(src => src.GuestReviews));
            CreateMap<Hotel, HotelResponse>();

        }
    }
}
