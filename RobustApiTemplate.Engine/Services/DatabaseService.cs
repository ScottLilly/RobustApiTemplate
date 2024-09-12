using RobustApiTemplate.Engine.Common;
using RobustApiTemplate.Engine.Models;
using System.Data.SqlClient;

namespace RobustApiTemplate.Engine.Services;

public class DatabaseService(string connectionString) : IDatabaseService
{
    #region Fields

    private readonly string _connectionString = connectionString;

    #endregion

    #region Public Methods

    public async Task<Employee> GetEmployeeByIdAsync(int id)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            using SqlCommand command = connection.CreateCommand();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = "SELECT * FROM [Employee] WHERE [Id] = @EmployeeId";
            command.Parameters.AddWithValue("@EmployeeId", id);

            SqlDataReader reader = await command.ExecuteReaderAsync();

            if (reader.HasRows && reader.Read())
            {
                return new Employee()
                {
                    Id = reader.GetValue<int>("Id"),
                    FirstName = reader.GetValue<string>("FirstName"),
                    LastName = reader.GetValue<string>("LastName")
                };
            }

            return null;
        }
    }

    #endregion
}
