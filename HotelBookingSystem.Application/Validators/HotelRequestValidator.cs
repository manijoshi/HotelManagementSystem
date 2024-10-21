using FluentValidation;
using HotelBookingSystem.Application.DTO.HotelDTO;
using HotelBookingSystem.Domain.Enums;

namespace HotelBookingSystem.Application.Validators
{
    public class HotelRequestValidator : AbstractValidator<HotelRequest>
    {
        public HotelRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Owner).NotEmpty().WithMessage("Owner is required");
            RuleFor(x => x.HotelType).NotEmpty().WithMessage("HotelType is required");
            RuleFor(x => x.CityId).GreaterThan(0).WithMessage("CityId must be greater than 0");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
            RuleFor(x => x.ThumbnailUrl).NotEmpty().WithMessage("ThumbnailUrl is required");
            RuleFor(x => x.Amenities)
                .Must(BeValidAmenities).WithMessage("One or more amenities are invalid");

        }

        private bool BeValidAmenities(ICollection<string> amenities)
        {
            if (amenities == null || !amenities.Any())
            {
                return true; 
            }

            foreach (var amenity in amenities)
            {
                if (!Enum.TryParse(typeof(HotelAmenity), amenity, true, out _))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
