using FluentValidation;
using HotelBookingSystem.Application.DTO.UserDTO;

namespace HotelBookingSystem.Application.Validators
{
    public class UserRequestValidator : AbstractValidator<UserRequest>
    {
        public UserRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is required");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is required");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Valid Email is required");
            RuleFor(x => x.Password).MinimumLength(5).NotEmpty().WithMessage("Password is required");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("PhoneNumber is required");
            RuleFor(x => x.DateOfBirth).LessThan(DateTime.Now).WithMessage("DateOfBirth must be in the past");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required");
            RuleFor(x => x.City).NotEmpty().WithMessage("City is required");
            RuleFor(x => x.Country).NotEmpty().WithMessage("Country is required");
            RuleFor(x => x.Role).NotEmpty().WithMessage("Role is required");
        }
    }
}
