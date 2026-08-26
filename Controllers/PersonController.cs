using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeopleHub.DTOs;
using PeopleHub.Services;

namespace PeopleHub.Controllers;

[ApiController, Route("api/[controller]"), Authorize]
public class PersonController : ControllerBase
{
    readonly IPersonService personService;
    string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    public PersonController(IPersonService personService) => this.personService = personService;

    [HttpGet, AllowAnonymous] public async Task<IActionResult> GetAll(int page = 1, int pageSize = 5) => Ok(await personService.GetAllAsync(page, pageSize));

    [HttpGet("{id:int}"), AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var person = await personService.GetByIdAsync(id);
        return person != null ? Ok(person) : NotFound(new ProblemDetails { Title = "Person not found", Detail = $"No person exists with Id {id}", Status = 404 });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePersonDto dto)
    {
        var newPerson = await personService.CreateAsync(CurrentUserId, dto);
        return CreatedAtAction(nameof(GetById), new { id = newPerson.Id }, newPerson);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdatePersonDto dto)
    {
        var (found, authorized, person) = await personService.UpdateAsync(CurrentUserId, id, dto);
        if (!found) return NotFound(new ProblemDetails { Title = "Person not found", Detail = $"No person exists with Id {id}", Status = 404 });
        if (!authorized) return Forbid();
        return Ok(person);
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> PartialUpdate(int id, PartialUpdatePersonDto dto)
    {
        var (found, authorized, person) = await personService.PartialUpdateAsync(CurrentUserId, id, dto);
        if (!found) return NotFound(new ProblemDetails { Title = "Person not found", Detail = $"No person exists with Id {id}", Status = 404 });
        if (!authorized) return Forbid();
        return Ok(person);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var (found, authorized, _) = await personService.DeleteAsync(CurrentUserId, id);
        if (!found) return NotFound(new ProblemDetails { Title = "Person not found", Detail = $"No person exists with Id {id}", Status = 404 });
        if (!authorized) return Forbid();
        return NoContent();
    }
}