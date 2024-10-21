namespace HotelBookingSystem.Application.PasswordHasher
{
    public interface IPasswordHasher
    {
        string HashPassword(string password, byte[] salt);
        bool VerifyPassword(string password, string hashedPassword);

        byte[] GenerateSalt(int length = 32);
    }
}
