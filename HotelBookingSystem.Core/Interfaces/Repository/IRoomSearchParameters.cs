using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingSystem.Domain.Interfaces.Repository
{
    public interface IRoomSearchParameters
    {
        public DateTime? CheckInDate { get; set; } 
        public DateTime? CheckOutDate { get; set; } 
        public int? Adults { get; set; }
        public int? Children { get; set; } 
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public IList<string>? RoomTypes { get; set; }
        public int Page { get; set; } 
        public int PageSize { get; set; } 
    }
}
