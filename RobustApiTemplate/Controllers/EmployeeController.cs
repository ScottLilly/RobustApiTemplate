using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using RobustApiTemplate.Engine.Models;
using RobustApiTemplate.Engine.Services;

namespace RobustApiTemplate.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v1/employee")]
public class EmployeeController(IDatabaseService databaseService) : Controller
{
    private readonly IDatabaseService _databaseService = databaseService;

    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetEmployeeAsync(int id)
    {
        var employee = await _databaseService.GetEmployeeByIdAsync(id);

        if (employee == null)
        {
            return NotFound();
        }

        return Ok(employee);
    }
}
