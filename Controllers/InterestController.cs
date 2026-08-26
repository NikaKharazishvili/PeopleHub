using Microsoft.AspNetCore.Mvc;
using PeopleHub.Services;

namespace PeopleHub.Controllers;

[ApiController, Route("api/[controller]")]
public class InterestController : ControllerBase
{
    readonly IInterestService interestService;

    public InterestController(IInterestService interestService) => this.interestService = interestService;

    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await interestService.GetAllAsync());
}