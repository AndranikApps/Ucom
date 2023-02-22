using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UcomGridView.Data.Entities;
using UcomGridView.Data.Interfaces;
using UcomGridView.Data.Options;

namespace UcomGridView.Data.Classes
{
    public class UserRepository : IUserRepository
    {
        private SqlConnection _connection;
        private SqlCommand _command;
        private readonly IOptions<ConnectionStringOption> _connectionStringOptions;

        public UserRepository(IOptions<ConnectionStringOption> connectionStringOptions)
        {
            _connectionStringOptions = connectionStringOptions;
        }

        public async Task<User?> CreateAsync(User user)
        {
            using (_connection = new SqlConnection(_connectionStringOptions.Value.ConnString))
            {
                await _connection.OpenAsync();
                _command = _connection.CreateCommand();
                _command.CommandType = System.Data.CommandType.StoredProcedure;
                _command.CommandText = "[dbo].[CreateUser]";
                _command.Parameters.AddWithValue("@Firstname", user.Firstname);
                _command.Parameters.AddWithValue("@Lastname", user.Lastname);
                _command.Parameters.AddWithValue("@Age", user.Age);
                _command.Parameters.AddWithValue("@Email", user.Email);
                _command.Parameters.AddWithValue("@AvatarPath", user.AvatarPath != null ? user.AvatarPath : DBNull.Value);
                _command.Parameters.AddWithValue("@StatusId", user.StatusId);

                using var reader = await _command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    user.Id = Convert.ToInt32(reader["Id"]);
                    user.Firstname = reader["Firstname"].ToString();
                    user.Lastname = reader["Lastname"].ToString();
                    user.Age = Convert.ToInt32(reader["Age"]);
                    user.Email = reader["Email"].ToString();
                    user.CreatedAt = DateTime.Parse(reader["CreatedAt"].ToString());
                    user.UpdatedAt = DateTime.Parse(reader["UpdatedAt"].ToString());
                    user.IsDeleted = Convert.ToBoolean(reader["IsDeleted"]);
                    user.AvatarPath = reader["AvatarPath"]?.ToString();
                    user.StatusId = Convert.ToInt32(reader["StatusId"]);
                }
                else
                {
                    user = null;
                }
            }

            return user;
        }

        public async Task<int> DeleteAsync(int id)
        { 
            int result;

            using (_connection = new SqlConnection(_connectionStringOptions.Value.ConnString))
            {
                await _connection.OpenAsync();
                _command = _connection.CreateCommand();
                _command.CommandType = System.Data.CommandType.StoredProcedure;
                _command.CommandText = "[dbo].[DeleteUser]";
                _command.Parameters.AddWithValue("@Id", id);

                result = await _command.ExecuteNonQueryAsync();
            }

            return result;
        }

        public async Task<IEnumerable<User>> GetAsync(int skip, int take, string columnName, string order)
        {
            List<User> users;

            using (_connection = new SqlConnection(_connectionStringOptions.Value.ConnString))
            {
                await _connection.OpenAsync();
                _command = _connection.CreateCommand();
                _command.CommandType = System.Data.CommandType.StoredProcedure;
                _command.CommandText = "[dbo].[GetUsers]";
                _command.Parameters.AddWithValue("@skip", skip);
                _command.Parameters.AddWithValue("@take", take);
                if (!columnName.IsNullOrEmpty()) _command.Parameters.AddWithValue("@columnName", columnName);
                if (!order.IsNullOrEmpty()) _command.Parameters.AddWithValue("@order", order);

                users = new List<User>();
                using var reader = await _command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    users.Add(new User()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Firstname = reader["Firstname"].ToString(),
                        Lastname = reader["Lastname"].ToString(),
                        Age = Convert.ToInt32(reader["Age"]),
                        Email = reader["Email"].ToString(),
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                        UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"]),
                        IsDeleted = Convert.ToBoolean(reader["IsDeleted"]),
                        AvatarPath = reader["AvatarPath"]?.ToString(),
                        StatusId = Convert.ToInt32(reader["StatusId"])
                    });
                }
            }

            return users;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            User user = null;

            using (_connection = new SqlConnection(_connectionStringOptions.Value.ConnString))
            {
                await _connection.OpenAsync();
                _command = _connection.CreateCommand();
                _command.CommandType = System.Data.CommandType.StoredProcedure;
                _command.CommandText = "[dbo].[GetUserById]";
                _command.Parameters.AddWithValue("@id", id);

                using var reader = await _command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    user = new User();
                    user.Id = Convert.ToInt32(reader["Id"]);
                    user.Firstname = reader["Firstname"].ToString();
                    user.Lastname = reader["Lastname"].ToString();
                    user.Age = Convert.ToInt32(reader["Age"]);
                    user.Email = reader["Email"].ToString();
                    user.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);
                    user.UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"]);
                    user.IsDeleted = Convert.ToBoolean(reader["IsDeleted"]);
                    user.AvatarPath = reader["AvatarPath"]?.ToString();
                    user.StatusId = Convert.ToInt32(reader["StatusId"]);
                }
            }

            return user;
        }

        public async Task<string> UpdateAsync(User user)
        {
            string result = null;

            using (_connection = new SqlConnection(_connectionStringOptions.Value.ConnString))
            {
                await _connection.OpenAsync();
                _command = _connection.CreateCommand();
                _command.CommandType = System.Data.CommandType.StoredProcedure;
                _command.CommandText = "[dbo].[UpdateUser]";
                _command.Parameters.AddWithValue("@id", user.Id);
                _command.Parameters.AddWithValue("@Firstname", user.Firstname);
                _command.Parameters.AddWithValue("@Lastname", user.Lastname);
                _command.Parameters.AddWithValue("@Age", user.Age);
                _command.Parameters.AddWithValue("@Email", user.Email);
                _command.Parameters.AddWithValue("@IsDeleted", user.IsDeleted);
                _command.Parameters.AddWithValue("@AvatarPath", user.AvatarPath != null ? user.AvatarPath : DBNull.Value);
                _command.Parameters.AddWithValue("@StatusId", user.StatusId);

                using var reader = await _command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    result = reader["AvatarPath"]?.ToString();
                }
            }

            return result;
        }
    }
}
