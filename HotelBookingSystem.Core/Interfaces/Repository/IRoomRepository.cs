using HotelBookingSystem.Domain.Entities;

namespace HotelBookingSystem.Domain.Interfaces.Repository
{
    public interface IRoomRepository : IRepository<Room>
    {
        Task<(IList<Room> Rooms, int TotalResults)> SearchAsync(IRoomSearchParameters searchParameters);
    }
}
