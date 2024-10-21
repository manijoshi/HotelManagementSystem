using FluentValidation.TestHelper;
using HotelBookingSystem.Application.DTO.HotelDTO;
using HotelBookingSystem.Application.Validators;


namespace HotelBookingSystem.Tests.ValidatorTests
{
    public class HotelRequestValidatorTests
    {
        private readonly HotelRequestValidator _validator;

        public HotelRequestValidatorTests()
        {
            _validator = new HotelRequestValidator();
        }

        [Fact]
        public void ShouldHaveErrorWhenNameIsEmpty()
        {
            var request = new HotelRequest { Name = string.Empty };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Name).WithErrorMessage("Name is required");
        }

        [Fact]
        public void ShouldHaveErrorWhenOwnerIsEmpty()
        {
            var request = new HotelRequest { Owner = string.Empty };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Owner).WithErrorMessage("Owner is required");
        }

        [Fact]
        public void ShouldHaveErrorWhenHotelTypeIsEmpty()
        {
            var request = new HotelRequest { HotelType = string.Empty };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.HotelType).WithErrorMessage("HotelType is required");
        }

        [Fact]
        public void ShouldHaveErrorWhenCityIdIsZeroOrNegative()
        {
            var request = new HotelRequest { CityId = 0 };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.CityId).WithErrorMessage("CityId must be greater than 0");

            request.CityId = -1;
            result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.CityId).WithErrorMessage("CityId must be greater than 0");
        }

        [Fact]
        public void ShouldHaveErrorWhenDescriptionIsEmpty()
        {
            var request = new HotelRequest { Description = string.Empty };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Description).WithErrorMessage("Description is required");
        }

        [Fact]
        public void ShouldHaveErrorWhenThumbnailUrlIsEmpty()
        {
            var request = new HotelRequest { ThumbnailUrl = string.Empty };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.ThumbnailUrl).WithErrorMessage("ThumbnailUrl is required");
        }

        [Fact]
        public void ShouldHaveErrorWhenAmenitiesContainInvalidValues()
        {
            var request = new HotelRequest { Amenities = new[] { "InvalidAmenity" } };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Amenities).WithErrorMessage("One or more amenities are invalid");
        }

        [Fact]
        public void ShouldPassWhenAllPropertiesAreValid()
        {
            var request = new HotelRequest
            {
                Name = "Hotel Name",
                Owner = "Hotel Owner",
                HotelType = "Luxury",
                CityId = 1,
                Description = "Hotel Description",
                ThumbnailUrl = "http://example.com/thumbnail.jpg",
                Amenities = new[] { "SwimmingPool", "Gym" }
            };
            var result = _validator.TestValidate(request);
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
            result.ShouldNotHaveValidationErrorFor(x => x.Owner);
            result.ShouldNotHaveValidationErrorFor(x => x.HotelType);
            result.ShouldNotHaveValidationErrorFor(x => x.CityId);
            result.ShouldNotHaveValidationErrorFor(x => x.Description);
            result.ShouldNotHaveValidationErrorFor(x => x.ThumbnailUrl);
            result.ShouldNotHaveValidationErrorFor(x => x.Amenities);
        }
    }
}
