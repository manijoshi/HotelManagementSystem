using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingSystem.Application.DTO.CityDTO
{
    public class CityResponseWithNumberOfHotels
    {
        public int CityId { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string PostOffice { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Visitors { get; set; }
        public int NumberOfHotels { get; set; }

    }
}
