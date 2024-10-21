using FluentValidation.TestHelper;
using HotelBookingSystem.Application.DTO.GuestReviewDTO;
using HotelBookingSystem.Application.Validators;

namespace HotelBookingSystem.Tests.ValidatorTests
{
    public class GuestReviewRequestValidatorTests
    {
        private readonly GuestReviewRequestValidator _validator;

        public GuestReviewRequestValidatorTests()
        {
            _validator = new GuestReviewRequestValidator();
        }


        [Fact]
        public void ShouldHaveErrorWhenRatingIsOutsideValidRange()
        {
            var request = new GuestReviewRequest { Rating = 0 };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Rating).WithErrorMessage("Rating must be between 1 and 5");

            request.Rating = 6;
            result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Rating).WithErrorMessage("Rating must be between 1 and 5");
        }

        [Fact]
        public void ShouldHaveErrorWhenCommentIsEmpty()
        {
            var request = new GuestReviewRequest { Comment = string.Empty };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Comment).WithErrorMessage("Comment is required");
        }

        [Fact]
        public void ShouldHaveErrorWhenCommentIsTooLong()
        {
            var request = new GuestReviewRequest { Comment = new string('x', 201) };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Comment).WithErrorMessage("Hotel comment must be no more than 200 characters.");
        }

        [Fact]
        public void ShouldPassWhenAllPropertiesAreValid()
        {
            var request = new GuestReviewRequest
            {
                Rating = 4,
                Comment = "Great stay!"
            };
            var result = _validator.TestValidate(request);
            result.ShouldNotHaveValidationErrorFor(x => x.Rating);
            result.ShouldNotHaveValidationErrorFor(x => x.Comment);
        }
    }
}
