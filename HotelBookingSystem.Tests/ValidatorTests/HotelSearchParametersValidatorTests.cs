using FluentValidation.TestHelper;
using HotelBookingSystem.Application.DTO.HotelDTO;
using HotelBookingSystem.Application.Validators;


namespace HotelBookingSystem.Tests.ValidatorTests
{
    public class HotelSearchParametersValidatorTests
    {
        private readonly HotelSearchParametersValidator _validator;

        public HotelSearchParametersValidatorTests()
        {
            _validator = new HotelSearchParametersValidator();
        }

        [Fact]
        public void ShouldHaveValidationErrorForQuery_WhenQueryContainsInvalidCharacters()
        {
            var model = new HotelSearchParameters
            {
                Query = "Invalid@Query"
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Query)
                .WithErrorMessage("Query contains invalid characters.");
        }


        [Fact]
        public void ShouldHaveValidationErrorForMinRating_WhenRatingIsOutOfRange()
        {
            var model = new HotelSearchParameters
            {
                MinRating = 0
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.MinRating)
                .WithErrorMessage("Minimum rating must be between 1 and 5.");
        }

        [Fact]
        public void ShouldHaveValidationErrorForMaxRating_WhenRatingIsOutOfRange()
        {
            var model = new HotelSearchParameters
            {
                MaxRating = 6
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.MaxRating)
                .WithErrorMessage("Maximum rating must be between 1 and 5.");
        }

        [Fact]
        public void ShouldHaveValidationErrorForRatingRange_WhenMinRatingIsGreaterThanMaxRating()
        {
            var model = new HotelSearchParameters
            {
                MinRating = 4,
                MaxRating = 3
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x)
                .WithErrorMessage("Minimum rating should not be greater than maximum rating.");
        }

        [Fact]
        public void ShouldHaveValidationErrorForAmenities_WhenContainsInvalidStrings()
        {
            var model = new HotelSearchParameters
            {
                Amenities = new List<string> { "SwimmingPool", "", "Gym" }
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Amenities)
                .WithErrorMessage("All amenities must be valid non-empty strings.");
        }


        [Fact]
        public void ShouldHaveValidationErrorForPage_WhenPageNumberIsNotPositive()
        {
            var model = new HotelSearchParameters
            {
                Page = 0
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Page)
                .WithErrorMessage("Page number must be a positive integer.");
        }

        [Fact]
        public void ShouldHaveValidationErrorForPageSize_WhenPageSizeIsOutOfRange()
        {
            var model = new HotelSearchParameters
            {
                PageSize = 51
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.PageSize)
                .WithErrorMessage("Page size must be between 1 and 50.");
        }

        [Fact]
        public void ShouldNotHaveValidationErrors_WhenAllPropertiesAreValid()
        {
            var model = new HotelSearchParameters
            {
                Query = "ValidQuery",
                MinRating = 1,
                MaxRating = 5,
                Amenities = new List<string> { "SwimmingPool", "Gym" },
                Page = 1,
                PageSize = 10
            };

            var result = _validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}

