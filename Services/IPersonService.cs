using PeopleHub.Common;
using PeopleHub.DTOs;

namespace PeopleHub.Services;

public interface IPersonService
{
    Task<PagedResponse<PersonDto>> GetAllAsync(int page, int pageSize);
    Task<PersonDto?> GetByIdAsync(int id);
    Task<PersonDto> CreateAsync(string userId, CreatePersonDto dto);
    Task<(bool found, bool authorized, PersonDto? person)> UpdateAsync(string userId, int id, UpdatePersonDto dto);
    Task<(bool found, bool authorized, PersonDto? person)> PartialUpdateAsync(string userId, int id, PartialUpdatePersonDto dto);
    Task<(bool found, bool authorized, PersonDto? person)> DeleteAsync(string userId, int id);
}