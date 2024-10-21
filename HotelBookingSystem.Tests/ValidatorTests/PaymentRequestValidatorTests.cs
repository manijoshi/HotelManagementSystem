using FluentValidation.TestHelper;
using HotelBookingSystem.Application.DTO.PaymentDTO;
using HotelBookingSystem.Application.Validators;
using HotelBookingSystem.Domain.Enums;

namespace HotelBookingSystem.Tests.ValidatorTests
{
    public class PaymentRequestValidatorTests
    {
        private readonly PaymentRequestValidator _validator;

        public PaymentRequestValidatorTests()
        {
            _validator = new PaymentRequestValidator();
        }

        [Fact]
        public void ShouldHaveValidationErrorForBookingId_WhenBookingIdIsNotGreaterThanZero()
        {
            var model = new PaymentRequest
            {
                BookingId = 0
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.BookingId)
                .WithErrorMessage("BookingId must be greater than 0");
        }

        [Fact]
        public void ShouldHaveValidationErrorForPaymentMethod_WhenPaymentMethodIsEmpty()
        {
            var model = new PaymentRequest
            {
                BookingId = 1,
                PaymentMethod = string.Empty
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.PaymentMethod)
                .WithErrorMessage("PaymentMethod is required");
        }

        [Fact]
        public void ShouldHaveValidationErrorForPaymentMethod_WhenPaymentMethodIsInvalid()
        {
            var model = new PaymentRequest
            {
                BookingId = 1,
                PaymentMethod = "InvalidMethod"
            };

            var result = _validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(x => x.PaymentMethod)
                .WithErrorMessage("Invalid Payment Method");
        }

        [Fact]
        public void ShouldNotHaveValidationErrors_WhenAllPropertiesAreValid()
        {
            var model = new PaymentRequest
            {
                BookingId = 1,
                PaymentMethod = PaymentMethod.CreditCard.ToString()
            };

            var result = _validator.TestValidate(model);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
