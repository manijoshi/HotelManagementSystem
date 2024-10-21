using AutoMapper;
using FluentAssertions;
using HotelBookingSystem.Application.DTO.PaymentDTO;
using HotelBookingSystem.Application.MappingProfiles;
using HotelBookingSystem.Application.Services;
using HotelBookingSystem.Domain.Entities;
using HotelBookingSystem.Domain.Enums;
using HotelBookingSystem.Domain.Interfaces.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HotelBookingSystem.Tests.ServiceTests
{
    public class PaymentServiceTests
    {
        private readonly Mock<IPaymentRepository> _paymentRepositoryMock;
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly IMapper _mapper;
        private readonly PaymentService _paymentService;

        public PaymentServiceTests()
        {
            _paymentRepositoryMock = new Mock<IPaymentRepository>();
            _bookingRepositoryMock = new Mock<IBookingRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PaymentProfile>(); 
            });

            _mapper = config.CreateMapper();

            _paymentService = new PaymentService(
                _paymentRepositoryMock.Object,
                _bookingRepositoryMock.Object,
                _mapper
            );
        }

        [Fact]
        public async Task CreatePaymentAsync_ShouldReturnPaymentResponse_WhenBookingExists()
        {
            var booking = new Booking
            {
                BookingId = 1,
                TotalPrice = 100
            };

            var paymentRequest = new PaymentRequest
            {
                BookingId = 1,
                PaymentMethod = "CreditCard"
            };

            _bookingRepositoryMock.Setup(x => x.GetByIdAsync(paymentRequest.BookingId))
                .ReturnsAsync(booking);

            _paymentRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Payment>()))
                .ReturnsAsync((Payment payment) => payment);

            var result = await _paymentService.CreatePaymentAsync(paymentRequest);

            result.Should().NotBeNull();
            result.BookingId.Should().Be(paymentRequest.BookingId);
            result.Amount.Should().Be(booking.TotalPrice);
            result.Status.Should().Be(PaymentStatus.Pending.ToString());
        }

        [Fact]
        public async Task CreatePaymentAsync_ShouldThrowKeyNotFoundException_WhenBookingDoesNotExist()
        {
            var paymentRequest = new PaymentRequest
            {
                BookingId = 1,
                PaymentMethod = "CreditCard"
            };

            _bookingRepositoryMock.Setup(x => x.GetByIdAsync(paymentRequest.BookingId))
                .ReturnsAsync((Booking)null);

            Func<Task> act = async () => await _paymentService.CreatePaymentAsync(paymentRequest);

            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Booking not found");
        }

        [Fact]
        public async Task GetPaymentDetailsAsync_ShouldReturnPaymentResponse_WhenPaymentExists()
        {
            var payment = new Payment
            {
                PaymentId = 1,
                BookingId = 1,
                Amount = 100,
                Status = PaymentStatus.Completed,
                PaymentDate = DateTime.UtcNow
            };

            _paymentRepositoryMock.Setup(x => x.GetByIdAsync(payment.PaymentId))
                .ReturnsAsync(payment);

            var result = await _paymentService.GetPaymentDetailsAsync(payment.PaymentId);

            result.Should().NotBeNull();
            result.PaymentId.Should().Be(payment.PaymentId);
            result.Amount.Should().Be(payment.Amount);
            result.Status.Should().Be(payment.Status.ToString());
        }

        [Fact]
        public async Task GetPaymentDetailsAsync_ShouldThrowKeyNotFoundException_WhenPaymentDoesNotExist()
        {
            var paymentId = 1;

            _paymentRepositoryMock.Setup(x => x.GetByIdAsync(paymentId))
                .ReturnsAsync((Payment)null);

            Func<Task> act = async () => await _paymentService.GetPaymentDetailsAsync(paymentId);

            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Payment not found");
        }

        [Fact]
        public async Task UpdatePaymentStatusAsync_ShouldReturnPaymentResponse_WhenPaymentExistsAndStatusIsValid()
        {
            var payment = new Payment
            {
                PaymentId = 1,
                BookingId = 1,
                Amount = 100,
                Status = PaymentStatus.Pending,
                PaymentDate = DateTime.UtcNow
            };

            _paymentRepositoryMock.Setup(x => x.GetByIdAsync(payment.PaymentId))
                .ReturnsAsync(payment);

            _paymentRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Payment>()))
                .ReturnsAsync((Payment payment) => payment);

            var newStatus = "Completed";

            var result = await _paymentService.UpdatePaymentStatusAsync(payment.PaymentId, newStatus);

            result.Should().NotBeNull();
            result.Status.Should().Be(PaymentStatus.Completed.ToString());
        }

        [Fact]
        public async Task UpdatePaymentStatusAsync_ShouldThrowKeyNotFoundException_WhenPaymentDoesNotExist()
        {
            var paymentId = 1;
            var newStatus = "Completed";

            _paymentRepositoryMock.Setup(x => x.GetByIdAsync(paymentId))
                .ReturnsAsync((Payment)null);

            Func<Task> act = async () => await _paymentService.UpdatePaymentStatusAsync(paymentId, newStatus);

            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Payment not found");
        }

        [Fact]
        public async Task UpdatePaymentStatusAsync_ShouldThrowArgumentException_WhenStatusIsInvalid()
        {
            var payment = new Payment
            {
                PaymentId = 1,
                BookingId = 1,
                Amount = 100,
                Status = PaymentStatus.Pending,
                PaymentDate = DateTime.UtcNow
            };

            _paymentRepositoryMock.Setup(x => x.GetByIdAsync(payment.PaymentId))
                .ReturnsAsync(payment);

            var invalidStatus = "InvalidStatus";

            Func<Task> act = async () => await _paymentService.UpdatePaymentStatusAsync(payment.PaymentId, invalidStatus);

            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Invalid payment status.");
        }
    }
}
