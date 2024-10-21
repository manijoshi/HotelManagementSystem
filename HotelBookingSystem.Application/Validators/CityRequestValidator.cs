using FluentValidation;
using HotelBookingSystem.Application.DTO.CityDTO;

namespace HotelBookingSystem.Application.Validators
{
    public class CityRequestValidator : AbstractValidator<CityRequest>
    {
        public CityRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Country).NotEmpty().WithMessage("Country is required");
            RuleFor(x => x.PostOffice).NotEmpty().WithMessage("PostOffice is required");
        }
    }
}
