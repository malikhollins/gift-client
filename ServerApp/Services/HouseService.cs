using Dapper;
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

        public async Task<IReadOnlyList<House>> GetHousesAsync( int userId )
        {
            using var connection = _connectionService.EstablishConnection();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("user_id", userId);

            var response = await connection.QueryAsync<House>(
                sql: "[dbo].[GetHouses]",
                param: parameters,
                commandType: CommandType.StoredProcedure);

            return response.ToList();
        }
    }
}
