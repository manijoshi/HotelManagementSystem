using AutoMapper;
using HotelBookingSystem.Application.DTO.PaymentDTO;
using HotelBookingSystem.Domain.Entities;
using HotelBookingSystem.Domain.Enums;
using HotelBookingSystem.Domain.Interfaces.Repository;

namespace HotelBookingSystem.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepository paymentRepository, IBookingRepository bookingRepository, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        public async Task<PaymentResponse> CreatePaymentAsync(PaymentRequest paymentRequest)
        {
            var booking = await _bookingRepository.GetByIdAsync(paymentRequest.BookingId);
            if (booking == null) throw new KeyNotFoundException("Booking not found");

            var payment = _mapper.Map<Payment>(paymentRequest);
            payment.Amount = booking.TotalPrice;
            
            await _paymentRepository.AddAsync(payment);

            return _mapper.Map<PaymentResponse>(payment);
        }

        public async Task<PaymentResponse> GetPaymentDetailsAsync(int paymentId)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment == null) throw new KeyNotFoundException("Payment not found");

            return _mapper.Map<PaymentResponse>(payment);
        }

        public async Task<PaymentResponse> UpdatePaymentStatusAsync(int paymentId, string status)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment == null) throw new KeyNotFoundException("Payment not found");

            if (!Enum.TryParse<PaymentStatus>(status, true, out var paymentStatus))
            {
                throw new ArgumentException("Invalid payment status.");
            }

            payment.Status = Enum.Parse<PaymentStatus>(status);
            await _paymentRepository.UpdateAsync(payment);

            return _mapper.Map<PaymentResponse>(payment);
        }
    }
}
