using FluentValidation.TestHelper;
using HotelBookingSystem.Application.DTO.RoomDTO;
using HotelBookingSystem.Application.Validators;
using HotelBookingSystem.Domain.Enums;

namespace HotelBookingSystem.Tests.ValidatorTests
{
    public class RoomRequestValidatorTests
    {
        private readonly RoomRequestValidator _validator;

        public RoomRequestValidatorTests()
        {
            _validator = new RoomRequestValidator();
        }


        [Fact]
        public void ShouldHaveValidationErrorForRoomType_WhenRoomTypeIsInvalid()
        {
            var model = new RoomRequest
            {
                RoomType = "InvalidRoomType"
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.RoomType)
                .WithErrorMessage("Invalid RoomType");
        }

        [Fact]
        public void ShouldHaveValidationErrorForPricePerNight_WhenPricePerNightIsNotGreaterThanZero()
        {
            var model = new RoomRequest
            {
                RoomType = RoomType.Double.ToString(),
                PricePerNight = 0
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.PricePerNight)
                .WithErrorMessage("PricePerNight must be greater than 0");
        }

        [Fact]
        public void ShouldHaveValidationErrorForAdultCapacity_WhenAdultCapacityIsNotGreaterThanZero()
        {
            var model = new RoomRequest
            {
                RoomType = RoomType.Double.ToString(),
                PricePerNight = 100,
                AdultCapacity = 0
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.AdultCapacity)
                .WithErrorMessage("AdultCapacity must be greater than 0");
        }

        [Fact]
        public void ShouldHaveValidationErrorForChildCapacity_WhenChildCapacityIsNegative()
        {
            var model = new RoomRequest
            {
                RoomType = RoomType.Double.ToString(),
                PricePerNight = 100,
                AdultCapacity = 2,
                ChildCapacity = -1
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.ChildCapacity)
                .WithErrorMessage("ChildCapacity must be greater than or equal to 0");
        }

        [Fact]
        public void ShouldHaveValidationErrorForImagesUrl_WhenImagesUrlIsNull()
        {
            var model = new RoomRequest
            {
                RoomType = RoomType.Double.ToString(),
                PricePerNight = 100,
                AdultCapacity = 2,
                ChildCapacity = 1,
                ImagesUrl = null
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.ImagesUrl)
                .WithErrorMessage("ImagesUrl cannot be null");
        }

        [Fact]
        public void ShouldNotHaveValidationErrors_WhenAllPropertiesAreValid()
        {
            var model = new RoomRequest
            {
                RoomType = RoomType.Double.ToString(),
                PricePerNight = 100,
                AdultCapacity = 2,
                ChildCapacity = 1,
                ImagesUrl = new List<string> { "http://example.com/image1.jpg", "http://example.com/image2.jpg" }

            };

            var result = _validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
