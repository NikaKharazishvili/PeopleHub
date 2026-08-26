using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PeopleHub.Models;

namespace PeopleHub.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Person> Persons { get; set; }
    public DbSet<Quote> Quotes { get; set; }
    public DbSet<Interest> Interests { get; set; }

    // Seeds default/sample data (Interests, Persons, Quotes, Person-Interest links)
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Person>().HasData(
            new Person { Id = 1, Name = "Nika", Age = 0, Profession = "Who Am I?", Country = "India", UserId = "11111111-1111-1111-1111-111111111111" },
            new Person { Id = 2, Name = "Misho", Age = 31, Profession = "Businessman", Country = "Georgia", UserId = "22222222-2222-2222-2222-222222222222" },
            new Person { Id = 3, Name = "Bako", Age = 25, Profession = "Traveller", Country = "Earth", UserId = "33333333-3333-3333-3333-333333333333" },
            new Person { Id = 4, Name = "Avala", Age = 25, Profession = "Plavala", Country = "Russia", UserId = "44444444-4444-4444-4444-444444444444" }
        );

        builder.Entity<Quote>().HasData(
            new Quote { Id = 1, Text = "I'm before everything", PersonId = 1 },
            new Quote { Id = 2, Text = "What's the point? What happens in the end?", PersonId = 1 },
            new Quote { Id = 3, Text = "How much money is enough money?", PersonId = 2 },
            new Quote { Id = 4, Text = "Yohturskaia", PersonId = 2 },
            new Quote { Id = 5, Text = "Do you have any visuals?", PersonId = 3 },
            new Quote { Id = 6, Text = "I'm collecting memories", PersonId = 3 },
            new Quote { Id = 7, Text = "Do you want some ganja?", PersonId = 4 }
        );

        builder.Entity<Interest>().HasData(
            new Interest { Id = 1, Name = "Arm Wrestling" },
            new Interest { Id = 2, Name = "Football" },
            new Interest { Id = 3, Name = "Crossfit" },
            new Interest { Id = 4, Name = "Basketball" },
            new Interest { Id = 5, Name = "Cars" },
            new Interest { Id = 6, Name = "Nature" },
            new Interest { Id = 7, Name = "Chilling At Home" },
            new Interest { Id = 8, Name = "Meditation" },
            new Interest { Id = 9, Name = "Mantras" },
            new Interest { Id = 10, Name = "Dance" },
            new Interest { Id = 11, Name = "Money" },
            new Interest { Id = 12, Name = "Reading Books" },
            new Interest { Id = 13, Name = "Indian Street Food" }
        );

        builder.Entity<Person>().HasMany(p => p.Interests).WithMany(i => i.People).UsingEntity(j => j.HasData(
            new { PeopleId = 1, InterestsId = 1 },  // Nika - Arm Wrestling
            new { PeopleId = 1, InterestsId = 7 },  // Nika - Chilling At Home
            new { PeopleId = 1, InterestsId = 8 },  // Nika - Meditation
            new { PeopleId = 1, InterestsId = 9 },  // Nika - Mantras
            new { PeopleId = 1, InterestsId = 13 }, // Nika - Indian Street Food
            new { PeopleId = 2, InterestsId = 2 },  // Misho - Football
            new { PeopleId = 2, InterestsId = 5 },  // Misho - Cars
            new { PeopleId = 2, InterestsId = 11 }, // Misho - Money
            new { PeopleId = 2, InterestsId = 12 }, // Misho - Reading Books
            new { PeopleId = 3, InterestsId = 3 },  // Bako - Crossfit
            new { PeopleId = 3, InterestsId = 5 },  // Bako - Cars
            new { PeopleId = 3, InterestsId = 6 },  // Bako - Nature
            new { PeopleId = 3, InterestsId = 10 }, // Bako - Dance
            new { PeopleId = 4, InterestsId = 4 },  // Avala - Basketball
            new { PeopleId = 4, InterestsId = 7 },  // Avala - Chilling At Home
            new { PeopleId = 4, InterestsId = 9 },  // Avala - Mantras
            new { PeopleId = 4, InterestsId = 10 }  // Avala - Dance
        ));
    }
}