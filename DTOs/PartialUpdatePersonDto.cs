using System.ComponentModel.DataAnnotations;
using static PeopleHub.Common.Constants;

namespace PeopleHub.DTOs;

public class PartialUpdatePersonDto
{
    [MinLength(MinLength), MaxLength(MaxLength)] public string? Name { get; set; }
    [Range(MinRange, MaxRange)] public int? Age { get; set; }
    [MinLength(MinLength), MaxLength(MaxLength)] public string? Profession { get; set; }
    [MinLength(MinLength), MaxLength(MaxLength)] public string? Country { get; set; }

    public List<string>? Quotes { get; set; }
    public List<int>? InterestIds { get; set; }
}