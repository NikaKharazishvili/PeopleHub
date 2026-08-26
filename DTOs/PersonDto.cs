namespace PeopleHub.DTOs;

public class PersonDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Age { get; set; }
    public string Profession { get; set; } = "";
    public string Country { get; set; } = "";
    
    public List<string> Quotes { get; set; } = new();
    public List<string> Interests { get; set; } = new();
}