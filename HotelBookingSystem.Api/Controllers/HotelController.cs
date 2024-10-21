
using HotelBookingSystem.Application.DTO.GuestReviewDTO;
using HotelBookingSystem.Application.DTO.HotelDTO;
using HotelBookingSystem.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace HotelBookingSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        private readonly IGuestReviewService _guestReviewService;

        public HotelController(IHotelService hotelService, IGuestReviewService guestReviewService)
        {
            _hotelService = hotelService;
            _guestReviewService = guestReviewService;
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost("create-hotel")]
        public async Task<IActionResult> CreateHotel([FromBody] HotelRequest request)
        {
            var hotel = await _hotelService.CreateHotelAsync(request);
            return CreatedAtAction(nameof(GetHotelById), new { hotelId = hotel.HotelId }, hotel);
        }

        [HttpGet("{hotelId}")]
        public async Task<IActionResult> GetHotelById(int hotelId)
        {
            var hotel = await _hotelService.GetHotelByIdAsync(hotelId);
            return Ok(hotel);
        }

        [HttpGet("{hotelId}/with-reviews")]
        public async Task<IActionResult> GetHotelByIdWithReviews(int hotelId)
        {
            var hotel = await _hotelService.GetHotelByIdWithReviewsAsync(hotelId);
            return Ok(hotel);
        }



        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("{hotelId}")]
        public async Task<IActionResult> UpdateHotel(int hotelId, [FromBody] HotelRequest request)
        {

            var hotel = await _hotelService.UpdateHotelAsync(hotelId, request);
            return Ok(hotel);

        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{hotelId}")]
        public async Task<IActionResult> DeleteHotel(int hotelId)
        {

            await _hotelService.DeleteHotelAsync(hotelId);
            return NoContent();

        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] HotelSearchParameters searchParameters)
        {

            var result = await _hotelService.SearchHotelsAsync(searchParameters);
            return Ok(result);

        }

        [HttpPost("{hotelId}/reviews")]
        [Authorize(Policy = "CustomerPolicy")]
        public async Task<IActionResult> AddGuestReview(int hotelId, [FromBody] GuestReviewRequest reviewRequest)
        {

            var reviewResponse = await _guestReviewService.AddReviewAsync(hotelId, reviewRequest);
            return Ok(reviewResponse);

        }

        [Authorize(Policy = "AdminOrCustomer")]
        [HttpDelete("{hotelId}/reviews/{reviewId}")]
        public async Task<IActionResult> DeleteGuestReview(int hotelId, int reviewId)
        {

            await _guestReviewService.DeleteReviewAsync(hotelId, reviewId);
            return NoContent();

        }

        [HttpGet("featured-deals")]
        public async Task<IActionResult> GetFeaturedDeals([FromQuery] int? limit)
        {

            var effectiveLimit = limit.HasValue && limit.Value > 0 ? limit.Value : 5;
            var deals = await _hotelService.GetFeaturedDealsAsync(effectiveLimit);
            return Ok(deals);
        }

        [Authorize(Policy = "CustomerPolicy")]
        [HttpGet("recent-hotels")]
        public async Task<IActionResult> GetRecentHotels([FromQuery] int limit = 5)
        {

            var recentHotels = await _hotelService.GetRecentHotelsAsync(limit);
            return Ok(recentHotels);

        }
    }

}
