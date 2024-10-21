using FluentValidation;
using HotelBookingSystem.Application.DTO.HotelDTO;

namespace HotelBookingSystem.Application.Validators
{
    public class HotelSearchParametersValidator : AbstractValidator<HotelSearchParameters>
    {
        public HotelSearchParametersValidator()
        {
            RuleFor(x => x.Query)
                .Matches("^[a-zA-Z0-9 ]*$").WithMessage("Query contains invalid characters.");

            RuleFor(x => x.MinRating)
                .InclusiveBetween(1, 5).WithMessage("Minimum rating must be between 1 and 5.")
                .When(x => x.MinRating.HasValue);

            RuleFor(x => x.MaxRating)
                .InclusiveBetween(1, 5).WithMessage("Maximum rating must be between 1 and 5.")
                .When(x => x.MaxRating.HasValue);

            RuleFor(x => x)
                .Must(x => x.MinRating <= x.MaxRating)
                .WithMessage("Minimum rating should not be greater than maximum rating.")
                .When(x => x.MinRating.HasValue && x.MaxRating.HasValue);

            RuleFor(x => x.Amenities)
                .Must(x => x.All(a => !string.IsNullOrEmpty(a)))
                .WithMessage("All amenities must be valid non-empty strings.")
                .When(x => x.Amenities != null);

            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Page number must be a positive integer.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 50).WithMessage("Page size must be between 1 and 50.");
        }
    }

}
