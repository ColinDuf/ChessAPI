// IAuthService.cs
using OpeningExplorer.DTOs;
namespace OpeningExplorer.Services;
public interface IAuthService {
    Task<int> Register(RegisterDto dto);
    Task<string> Login(LoginDto dto);
}