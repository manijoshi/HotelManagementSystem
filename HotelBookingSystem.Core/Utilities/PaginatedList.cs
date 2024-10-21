

using HotelBookingSystem.Domain.Interfaces;

namespace HotelBookingSystem.Domain.Utilities;

public class PaginatedList<T> : IPaginatedList<T>
{
    public int TotalResults { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public IList<T> Items { get; set; }
}
