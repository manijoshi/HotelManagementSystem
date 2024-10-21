
using FluentValidation.TestHelper;
using HotelBookingSystem.Application.DTO.BookingDTO;
using HotelBookingSystem.Application.Validators;


namespace HotelBookingSystem.Tests.ValidatorTests
{
    public class BookingRequestValidatorTests
    {
        private readonly BookingRequestValidator _validator;

        public BookingRequestValidatorTests()
        {
            _validator = new BookingRequestValidator();
        }

        [Fact]
        public void ShouldHaveErrorWhenRoomIdIsLessThanOrEqualToZero()
        {
            var request = new BookingRequest { RoomId = 0 };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.RoomId).WithErrorMessage("RoomId must be greater than 0");

            request.RoomId = -1;
            result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.RoomId).WithErrorMessage("RoomId must be greater than 0");
        }


        [Fact]
        public void ShouldHaveErrorWhenCheckInDateIsInThePast()
        {
            var request = new BookingRequest
            {
                RoomId = 1,
                CheckInDate = DateTime.Now.AddDays(-1),
                CheckOutDate = DateTime.Now.AddDays(1)
            };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.CheckInDate).WithErrorMessage("CheckInDate must be in the future");
        }

        [Fact]
        public void ShouldHaveErrorWhenCheckOutDateIsBeforeCheckInDate()
        {
            var request = new BookingRequest
            {
                RoomId = 1,
                CheckInDate = DateTime.Now.AddDays(1),
                CheckOutDate = DateTime.Now
            };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.CheckOutDate).WithErrorMessage("CheckOutDate must be after CheckInDate");
        }

        [Fact]
        public void ShouldHaveErrorWhenSpecialRequestsExceedsMaxLength()
        {
            var request = new BookingRequest
            {
                RoomId = 1,
                CheckInDate = DateTime.Now.AddDays(1),
                CheckOutDate = DateTime.Now.AddDays(2),
                SpecialRequests = new string('a', 501) 
            };
            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.SpecialRequests).WithErrorMessage("Special requests must be no more than 500 characters.");
        }

        [Fact]
        public void ShouldPassWhenAllPropertiesAreValid()
        {
            var request = new BookingRequest
            {
                RoomId = 1,
                CheckInDate = DateTime.Now.AddDays(1),
                CheckOutDate = DateTime.Now.AddDays(2),
                SpecialRequests = "Request"
            };
            var result = _validator.TestValidate(request);
            result.ShouldNotHaveValidationErrorFor(x => x.RoomId);
            result.ShouldNotHaveValidationErrorFor(x => x.CheckInDate);
            result.ShouldNotHaveValidationErrorFor(x => x.CheckOutDate);
            result.ShouldNotHaveValidationErrorFor(x => x.SpecialRequests);
        }
    }
}
