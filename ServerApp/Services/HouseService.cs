using Dapper;
using ServerApp.Models;
using System.Collections;
using System.Data;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class HouseService
    {
        IConnectionService _connectionService;

        public HouseService( IConnectionService connectionService ) 
        {
            _connectionService = connectionService;
        }

        public async Task<IEnumerable<House>> GetHousesAsync( int userId )
        {
            using var connection = _connectionService.EstablishConnection();

            DynamicParameters parameters = new();
            parameters.Add("user_id", userId);

            var response = await connection.QueryAsync<House>(
                sql: "[GiftingApp].[dbo].[GetHouses]",
                param: parameters,
                commandType: CommandType.StoredProcedure);

            return response;
        }

        public async Task<House> CreateHouseAsync(int userId, string name)
        {
            using var connection = _connectionService.EstablishConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("user_id", userId);
            parameters.Add("name", name);

            var house = await connection.QueryFirstAsync<House>(
                sql: "[GiftingApp].[dbo].[CreateHouse]",
                param: parameters,
                commandType: CommandType.StoredProcedure);

            return house;
        }
    }
}
