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

        public async Task<IReadOnlyList<House>> GetUserHousesAsync( int userId )
        {
            using var connection = _connectionService.EstablishConnection();

            var response = await connection.QueryAsync<House>(
                sql: "GetHouses",
                param: userId,
                commandType: CommandType.StoredProcedure);

            return response.ToList();
        }
    }
}
