using System.Data;
using Dapper;
using ServerApp.Models;
using WebAPI.Models;
using WebAPI.Services;

namespace ServerApp.Services
{
    public class InviteService
    {
        IConnectionService _connectionService;

        public InviteService(IConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        public async Task<IEnumerable<HouseInvites>?> GetInvitesForHouseAsync(int houseId)
        {
            using var connection = _connectionService.EstablishConnection();
            var parameters = new DynamicParameters();
            parameters.Add("house_id", houseId);

            IEnumerable<HouseInvites> invites = await connection.QueryAsync(
                map: (User user, InviteStatus status) =>
                {
                    return new HouseInvites { User = user, InviteStatus = status };
                },
                sql: "[GiftingApp].[dbo].[GetHouseInvites]",
                param: parameters,
                commandType: CommandType.StoredProcedure,
                splitOn: "Status");

            return invites;
        }

        public async Task<IEnumerable<UserInvites>?> GetInvitesForUserAsync(int userId)
        {
            using var connection = _connectionService.EstablishConnection();
            var parameters = new DynamicParameters();
            parameters.Add("user_id", userId);

            IEnumerable<UserInvites>? invites = await connection.QueryAsync(
                map: ( House house, InviteStatus status ) => 
                {
                    return new UserInvites { House = house, InviteStatus = status }; 
                },
                sql: "[GiftingApp].[dbo].[GetUserInvites]",
                param: parameters,
                commandType: CommandType.StoredProcedure,
                splitOn: "Status");

            return invites;
        }

        public async Task CreateInvite(int houseId, int userId)
        {
            using var connection = _connectionService.EstablishConnection();
            var parameters = new DynamicParameters();
            parameters.Add("house_id", houseId);
            parameters.Add("user_id", userId);

            await connection.ExecuteAsync(
                sql: "[GiftingApp].[dbo].[CreateInvite]",
                param: parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateInvite(int houseId, int userId, int status)
        {
            using var connection = _connectionService.EstablishConnection();
            var parameters = new DynamicParameters();
            parameters.Add("house_id", houseId);
            parameters.Add("user_id", userId);
            parameters.Add("status", status);

            await connection.ExecuteAsync(
                sql: "[GiftingApp].[dbo].[UpdateInvite]",
                param: parameters,
                commandType: CommandType.StoredProcedure);

        }
    }
}
