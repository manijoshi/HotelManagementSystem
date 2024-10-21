
using AutoMapper;
using HotelBookingSystem.Application.DTO.RoomDTO;
using HotelBookingSystem.Domain.Entities;


namespace HotelBookingSystem.Application.MappingProfiles
{
    public class RoomProfile : Profile
    {
        public RoomProfile() 
        {
            CreateMap<RoomRequest, Room>();
            CreateMap<Room, RoomResponse>()
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src =>
                            !src.Bookings.Any(b =>
                                b.CheckInDate < DateTime.Now && b.CheckOutDate > DateTime.Now)));
        }
    }
}
