using FluentValidation;
using HotelBookingSystem.Application.DTO.RoomDTO;
using HotelBookingSystem.Domain.Enums;

namespace HotelBookingSystem.Application.Validators
{
    public class RoomRequestValidator : AbstractValidator<RoomRequest>
    {
        public RoomRequestValidator()
        {
            RuleFor(x => x.RoomType).NotEmpty().WithMessage("RoomType is required")
                .Must(BeValidRoomType).WithMessage("Invalid RoomType");
            RuleFor(x => x.PricePerNight).GreaterThan(0).WithMessage("PricePerNight must be greater than 0");
            RuleFor(x => x.AdultCapacity).GreaterThan(0).WithMessage("AdultCapacity must be greater than 0");
            RuleFor(x => x.ChildCapacity).GreaterThanOrEqualTo(0).WithMessage("ChildCapacity must be greater than or equal to 0");
            RuleFor(x => x.ImagesUrl).NotNull().WithMessage("ImagesUrl cannot be null");
        }

        private bool BeValidRoomType(string roomType)
        {
            return Enum.TryParse(typeof(RoomType), roomType, true, out _);
        }
    }
}
