using Microsoft.EntityFrameworkCore;
using PeopleHub.Common;
using PeopleHub.Data;
using PeopleHub.DTOs;
using PeopleHub.Mappers;
using PeopleHub.Models;

namespace PeopleHub.Services;

public class PersonService : IPersonService
{
    readonly ApplicationDbContext context;
    readonly ILogger<PersonService> logger;

    public PersonService(ApplicationDbContext context, ILogger<PersonService> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task<PagedResponse<PersonDto>> GetAllAsync(int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 5;

        logger.LogInformation($"Fetching persons - page: {page}, page size: {pageSize}");
        var query = context.Persons.Include(p => p.Interests).Include(p => p.Quotes).OrderBy(p => p.Id).AsNoTracking();
        var totalCount = await query.CountAsync();
        var persons = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedResponse<PersonDto>
        {
            Items = persons.Select(p => p.ToPersonDto()).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }

    public async Task<PersonDto?> GetByIdAsync(int id)
    {
        logger.LogInformation($"Fetching person - id: {id}");
        var existing = await context.Persons.Include(p => p.Quotes).Include(p => p.Interests).AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        if (existing == null) logger.LogWarning($"Person with id {id} not found");
        return existing?.ToPersonDto();
    }

    public async Task<PersonDto> CreateAsync(string userId, CreatePersonDto dto)
    {
        logger.LogInformation($"Creating new person - name: {dto.Name}, user: {userId}");
        var person = dto.ToPerson();
        person.UserId = userId;

        if (dto.InterestIds.Any())
        {
            var interests = await context.Interests.Where(i => dto.InterestIds.Contains(i.Id)).ToListAsync();
            person.Interests = interests;
        }

        await context.Persons.AddAsync(person);
        await context.SaveChangesAsync();
        logger.LogInformation($"Person created successfully. Id: {person.Id}");
        return person.ToPersonDto();
    }

    public async Task<(bool, bool, PersonDto?)> UpdateAsync(string userId, int id, UpdatePersonDto dto)
    {
        logger.LogInformation($"Updating person - id: {id}, user: {userId}");
        var existing = await context.Persons.Include(p => p.Quotes).Include(p => p.Interests).FirstOrDefaultAsync(p => p.Id == id);
        if (existing == null)
        {
            logger.LogWarning($"Person with id {id} not found");
            return (false, false, null);
        }
        if (existing.UserId != userId)
        {
            logger.LogWarning($"User {userId} attempted to update person {id} they don't own");
            return (true, false, null);
        }

        existing.Name = dto.Name;
        existing.Age = dto.Age;
        existing.Profession = dto.Profession;
        existing.Country = dto.Country;
        existing.Quotes = dto.Quotes.Select(q => new Quote { Text = q }).ToList();
        existing.Interests = await context.Interests.Where(i => dto.InterestIds.Contains(i.Id)).ToListAsync();

        await context.SaveChangesAsync();
        logger.LogInformation($"Person {id} updated successfully");
        return (true, true, existing.ToPersonDto());
    }

    public async Task<(bool, bool, PersonDto?)> PartialUpdateAsync(string userId, int id, PartialUpdatePersonDto dto)
    {
        logger.LogInformation($"Partially updating person - id: {id}, user: {userId}");
        var existing = await context.Persons.Include(p => p.Quotes).Include(p => p.Interests).FirstOrDefaultAsync(p => p.Id == id);
        if (existing == null)
        {
            logger.LogWarning($"Person with id {id} not found");
            return (false, false, null);
        }
        if (existing.UserId != userId)
        {
            logger.LogWarning($"User {userId} attempted to partially update person {id} they don't own");
            return (true, false, null);
        }

        if (!string.IsNullOrWhiteSpace(dto.Name)) existing.Name = dto.Name;
        if (dto.Age.HasValue) existing.Age = dto.Age.Value;
        if (!string.IsNullOrWhiteSpace(dto.Profession)) existing.Profession = dto.Profession;
        if (!string.IsNullOrWhiteSpace(dto.Country)) existing.Country = dto.Country;
        if (dto.Quotes != null) existing.Quotes = dto.Quotes.Select(q => new Quote { Text = q }).ToList();
        if (dto.InterestIds != null) existing.Interests = await context.Interests.Where(i => dto.InterestIds.Contains(i.Id)).ToListAsync();

        await context.SaveChangesAsync();
        logger.LogInformation($"Person {id} partially updated successfully");
        return (true, true, existing.ToPersonDto());
    }

    public async Task<(bool, bool, PersonDto?)> DeleteAsync(string userId, int id)
    {
        logger.LogInformation($"Deleting person - id: {id}, user: {userId}");
        var existing = await context.Persons.FirstOrDefaultAsync(p => p.Id == id);
        if (existing == null)
        {
            logger.LogWarning($"Person with id {id} not found");
            return (false, false, null);
        }
        if (existing.UserId != userId)
        {
            logger.LogWarning($"User {userId} attempted to delete person {id} they don't own");
            return (true, false, null);
        }

        context.Persons.Remove(existing);
        await context.SaveChangesAsync();
        logger.LogInformation($"Person {id} deleted successfully");
        return (true, true, existing.ToPersonDto());
    }
}