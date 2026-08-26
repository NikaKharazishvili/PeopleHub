namespace PeopleHub.Models;

public class Interest
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public List<Person> People { get; set; } = new();
}