using Microsoft.EntityFrameworkCore;
using PeopleHub.Data;
using PeopleHub.DTOs;
using PeopleHub.Mappers;

namespace PeopleHub.Services;

public class InterestService : IInterestService
{
    readonly ApplicationDbContext context;
    readonly ILogger<InterestService> logger;

    public InterestService(ApplicationDbContext context, ILogger<InterestService> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task<List<InterestDto>> GetAllAsync()
    {
        logger.LogInformation("Fetching all interests");
        var interests = await context.Interests.AsNoTracking().OrderBy(i => i.Id).ToListAsync();
        return interests.Select(i => i.ToInterestDto()).ToList();
    }
}