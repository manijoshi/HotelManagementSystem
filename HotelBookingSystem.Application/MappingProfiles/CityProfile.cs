using AutoMapper;
using HotelBookingSystem.Application.DTO.CityDTO;
using HotelBookingSystem.Domain.Entities;


namespace HotelBookingSystem.Application.MappingProfiles
{
    public class CityProfile : Profile
    {
        public CityProfile() 
        {
            CreateMap<CityRequest, City>();
            CreateMap<City, CityResponseWithNumberOfHotels>()
                .ForMember(dest => dest.NumberOfHotels, opt => opt.MapFrom(src => src.Hotels != null ? src.Hotels.Count : 0));
            CreateMap<City, CityResponse>();
        }


    }
}
