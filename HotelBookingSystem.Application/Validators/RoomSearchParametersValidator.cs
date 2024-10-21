using FluentValidation;
using HotelBookingSystem.Application.DTO.RoomDTO;

namespace HotelBookingSystem.Application.Validators
{
    public class RoomSearchParametersValidator : AbstractValidator<RoomSearchParameters>
    {
        public RoomSearchParametersValidator()
        {


            RuleFor(x => x.Adults)
                .GreaterThan(0).WithMessage("Number of adults must be at least 1.");

            RuleFor(x => x.Children)
                .GreaterThanOrEqualTo(0).WithMessage("Number of children cannot be negative.");

            RuleFor(x => x.MinPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Minimum price must be a non-negative number.")
                .When(x => x.MinPrice.HasValue);

            RuleFor(x => x.MaxPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Maximum price must be a non-negative number.")
                .When(x => x.MaxPrice.HasValue);

            RuleFor(x => x)
                .Must(x => x.MinPrice <= x.MaxPrice)
                .WithMessage("Minimum price should not be greater than maximum price.")
                .When(x => x.MinPrice.HasValue && x.MaxPrice.HasValue);

            RuleFor(x => x.RoomTypes)
                .Must(x => x.All(rt => !string.IsNullOrEmpty(rt)))
                .WithMessage("All room types must be valid non-empty strings.")
                .When(x => x.RoomTypes != null);

            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Page number must be a positive integer.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 50).WithMessage("Page size must be between 1 and 50.");
        }
    }

}
