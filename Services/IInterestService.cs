using PeopleHub.DTOs;

namespace PeopleHub.Services;

public interface IInterestService
{
    Task<List<InterestDto>> GetAllAsync();
}