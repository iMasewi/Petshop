using LoginUpLevel.Models;

namespace LoginUpLevel.Services.Interface
{
    public interface IJwtService
    {
        Task<string> GenerateTokenAsync(User user);
    }
}
