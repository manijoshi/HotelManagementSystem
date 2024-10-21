
using AutoMapper;
using HotelBookingSystem.Application.DTO.UserDTO;
using HotelBookingSystem.Domain.Entities;

namespace HotelBookingSystem.Application.MappingProfiles
{
    public class UserProfile : Profile 
    {
        public UserProfile() 
        {
            CreateMap<UserRequest, User>();
            CreateMap<User, UserResponse>();
        }
    }
}
