using FluentValidation.TestHelper;
using HotelBookingSystem.Application.DTO.CityDTO;
using HotelBookingSystem.Application.Validators;

namespace HotelBookingSystem.Tests.ValidatorTests
{
    public class CityRequestValidatorTests
    {
        private readonly CityRequestValidator _validator;

        public CityRequestValidatorTests()
        {
            _validator = new CityRequestValidator();
        }

        [Fact]
        public void ShouldHaveErrorWhenNameIsEmpty()
        {
            var request = new CityRequest { Name = string.Empty };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Name).WithErrorMessage("Name is required");
        }

        [Fact]
        public void ShouldHaveErrorWhenCountryIsEmpty()
        {
            var request = new CityRequest { Country = string.Empty };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Country).WithErrorMessage("Country is required");
        }

        [Fact]
        public void ShouldHaveErrorWhenPostOfficeIsEmpty()
        {
            var request = new CityRequest { PostOffice = string.Empty };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.PostOffice).WithErrorMessage("PostOffice is required");
        }

        [Fact]
        public void ShouldPassWhenAllPropertiesAreValid()
        {
            var request = new CityRequest
            {
                Name = "New York",
                Country = "USA",
                PostOffice = "10001"
            };
            var result = _validator.TestValidate(request);
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
            result.ShouldNotHaveValidationErrorFor(x => x.Country);
            result.ShouldNotHaveValidationErrorFor(x => x.PostOffice);
        }
    }
}
