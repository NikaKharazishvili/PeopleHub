using System.ComponentModel.DataAnnotations;
using static PeopleHub.Common.Constants;

namespace PeopleHub.DTOs;

public class UpdatePersonDto
{
    [Required(), MinLength(MinLength), MaxLength(MaxLength)] public string Name { get; set; } = "";
    [Required(), Range(MinRange, MaxRange)] public int Age { get; set; }
    [Required(), MinLength(MinLength), MaxLength(MaxLength)] public string Profession { get; set; } = "";
    [Required(), MinLength(MinLength), MaxLength(MaxLength)] public string Country { get; set; } = "";

    public List<string> Quotes { get; set; } = new();
    public List<int> InterestIds { get; set; } = new();
}