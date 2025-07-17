// IOpeningService.cs
using OpeningExplorer.DTOs;
namespace OpeningExplorer.Services;
public interface IOpeningService {
    Task<OpeningDto[]> GetAll();
    Task<OpeningDto> Get(int id);
    Task<int> Create(CreateOpeningDto dto);
    Task Update(int id, CreateOpeningDto dto);
    Task Delete(int id);
}