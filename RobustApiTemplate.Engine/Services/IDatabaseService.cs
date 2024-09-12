using RobustApiTemplate.Engine.Models;

namespace RobustApiTemplate.Engine.Services;

public interface IDatabaseService
{
    Task<Employee> GetEmployeeByIdAsync(int id);
}
