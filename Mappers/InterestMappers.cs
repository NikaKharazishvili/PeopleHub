using PeopleHub.DTOs;
using PeopleHub.Models;

namespace PeopleHub.Mappers;

public static class InterestMappers
{
    public static InterestDto ToInterestDto(this Interest interest) => new()
    {
        Id = interest.Id,
        Name = interest.Name
    };
}