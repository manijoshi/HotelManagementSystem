

using AutoMapper;
using HotelBookingSystem.Application.DTO.BookingDTO;
using HotelBookingSystem.Domain.Entities;

namespace HotelBookingSystem.Application.MappingProfiles
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<BookingRequest, Booking>();
            CreateMap<Booking, BookingResponse>()
            .ForMember(dest => dest.UserFirstName, opt => opt.MapFrom(src => src.User.FirstName))
            .ForMember(dest => dest.UserLastName, opt => opt.MapFrom(src => src.User.LastName))
            .ForMember(dest => dest.RoomType, opt => opt.MapFrom(src => src.Room.RoomType))
            .ForMember(dest => dest.HotelName, opt => opt.MapFrom(src => src.Hotel.Name))
            .ForMember(dest => dest.HotelAddress, opt => opt.MapFrom(src => src.Hotel.Address));
        }
    }
}
