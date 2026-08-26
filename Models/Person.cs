namespace PeopleHub.Models;

public class Person
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int Age { get; set; }
    public required string Profession { get; set; }
    public required string Country { get; set; }

    public List<Interest> Interests { get; set; } = new();
    public List<Quote> Quotes { get; set; } = new();
    
    public string UserId { get; set; } = string.Empty;
}