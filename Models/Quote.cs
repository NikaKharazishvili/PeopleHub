namespace PeopleHub.Models;

public class Quote
{
    public int Id { get; set; }
    public required string Text { get; set; }

    public int PersonId { get; set; }
    public Person Person { get; set; } = null!;
}