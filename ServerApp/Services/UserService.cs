using Dapper;
using ServerApp.Models;
using System.Data;
using WebAPI.Services;

namespace ServerApp.Services
{
    public class UserService
    {
        IConnectionService _connectionService;

        public UserService(IConnectionService connectionService) 
        {
            this._connectionService = connectionService;
        }

        public async Task<User?> CreateUserAsync( string authId, string email )
        {
            using var connection = _connectionService.EstablishConnection();

            DynamicParameters parameters = new();
            parameters.Add("user_id", authId);
            parameters.Add("email", email);

           var identity = await connection.QueryFirstAsync<int>(
                sql: "[dbo].[CreateUser]",
                param: parameters,
                commandType: CommandType.StoredProcedure);

            return new User
            {
                Id = identity,
                Email = email,
            };
        }

        public async Task<User?> GetUserAsync(string authId)
        {
            using var connection = _connectionService.EstablishConnection();

            DynamicParameters parameters = new();
            parameters.Add("user_id", authId);

            var user = await connection.QueryFirstOrDefaultAsync<User>(
                 sql: "[dbo].[GetUser]",
                 param: parameters,
                 commandType: CommandType.StoredProcedure);

            return user;
        }

        public async Task<IReadOnlyList<User?>> BulkGetUserAsync(string email, string name)
        {
            using var connection = _connectionService.EstablishConnection();

            DynamicParameters parameters = new();
            parameters.Add("email", email);
            parameters.Add("name", name);

            var users = await connection.QueryAsync<User>(
                 sql: "[dbo].[BulkGetUsers]",
                 param: parameters,
                 commandType: CommandType.StoredProcedure);

            return [.. users];
        }
    }
}
