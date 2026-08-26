using PeopleHub.DTOs;
using PeopleHub.Models;

namespace PeopleHub.Mappers;

public static class PersonMappers
{
    public static PersonDto ToPersonDto(this Person person) => new()
    {
        Id = person.Id,
        Name = person.Name,
        Age = person.Age,
        Profession = person.Profession,
        Country = person.Country,
        Quotes = person.Quotes.Select(q => q.Text).ToList(),
        Interests = person.Interests.Select(i => i.Name).ToList()
    };

    public static Person ToPerson(this CreatePersonDto dto) => new()
    {
        Name = dto.Name,
        Age = dto.Age,
        Profession = dto.Profession,
        Country = dto.Country,
        Quotes = dto.Quotes.Select(q => new Quote { Text = q }).ToList()
    };
}