using FluentValidation;
using HotelBookingSystem.Application.DTO.GuestReviewDTO;

namespace HotelBookingSystem.Application.Validators
{
    public class GuestReviewRequestValidator : AbstractValidator<GuestReviewRequest>
    {
        public GuestReviewRequestValidator()
        {
            RuleFor(x => x.Rating).InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5");
            RuleFor(x => x.Comment).NotEmpty().WithMessage("Comment is required").MaximumLength(200).WithMessage("Hotel comment must be no more than 200 characters.");
        }
    }
}
