using FluentValidation.TestHelper;
using HotelBookingSystem.Application.DTO.UserDTO;
using HotelBookingSystem.Application.Validators;


namespace HotelBookingSystem.Tests.ValidatorTests
{
    public class UserRequestValidatorTests
    {
        private readonly UserRequestValidator _validator;

        public UserRequestValidatorTests()
        {
            _validator = new UserRequestValidator();
        }

        [Fact]
        public void ShouldHaveValidationErrorForFirstName_WhenFirstNameIsEmpty()
        {
            var model = new UserRequest
            {
                FirstName = string.Empty
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.FirstName)
                .WithErrorMessage("FirstName is required");
        }

        [Fact]
        public void ShouldHaveValidationErrorForLastName_WhenLastNameIsEmpty()
        {
            var model = new UserRequest
            {
                LastName = string.Empty
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.LastName)
                .WithErrorMessage("LastName is required");
        }

        [Fact]
        public void ShouldHaveValidationErrorForEmail_WhenEmailIsEmpty()
        {
            var model = new UserRequest
            {
                Email = string.Empty
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Valid Email is required");
        }

        [Fact]
        public void ShouldHaveValidationErrorForEmail_WhenEmailIsInvalid()
        {
            var model = new UserRequest
            {
                Email = "invalid-email"
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorMessage("Valid Email is required");
        }

        [Fact]
        public void ShouldHaveValidationErrorForPassword_WhenPasswordIsEmpty()
        {
            var model = new UserRequest
            {
                Password = string.Empty
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Password)
                .WithErrorMessage("Password is required");
        }

        [Fact]
        public void ShouldHaveValidationErrorForPhoneNumber_WhenPhoneNumberIsEmpty()
        {
            var model = new UserRequest
            {
                PhoneNumber = string.Empty
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
                .WithErrorMessage("PhoneNumber is required");
        }

        [Fact]
        public void ShouldHaveValidationErrorForDateOfBirth_WhenDateOfBirthIsInTheFuture()
        {
            var model = new UserRequest
            {
                DateOfBirth = DateTime.Now.AddDays(1)
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.DateOfBirth)
                .WithErrorMessage("DateOfBirth must be in the past");
        }

        [Fact]
        public void ShouldHaveValidationErrorForAddress_WhenAddressIsEmpty()
        {
            var model = new UserRequest
            {
                Address = string.Empty
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Address)
                .WithErrorMessage("Address is required");
        }

        [Fact]
        public void ShouldHaveValidationErrorForCity_WhenCityIsEmpty()
        {
            var model = new UserRequest
            {
                City = string.Empty
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.City)
                .WithErrorMessage("City is required");
        }

        [Fact]
        public void ShouldHaveValidationErrorForCountry_WhenCountryIsEmpty()
        {
            var model = new UserRequest
            {
                Country = string.Empty
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Country)
                .WithErrorMessage("Country is required");
        }

        [Fact]
        public void ShouldHaveValidationErrorForRole_WhenRoleIsEmpty()
        {
            var model = new UserRequest
            {
                Role = string.Empty
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.Role)
                .WithErrorMessage("Role is required");
        }

        [Fact]
        public void ShouldNotHaveValidationErrors_WhenAllPropertiesAreValid()
        {
            var model = new UserRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "securepassword",
                PhoneNumber = "1234567890",
                DateOfBirth = DateTime.Now.AddYears(-25),
                Address = "123 Main St",
                City = "Anytown",
                Country = "Country",
                Role = "User"
            };

            var result = _validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
