

namespace HotelBookingSystem.Domain.Interfaces
{
   public interface IPaginatedList<T>
    {
        public int TotalResults { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public IList<T> Items { get; set; }
    }
}
