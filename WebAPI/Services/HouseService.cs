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

        public IReadOnlyList<House> GetUserHouses( int userId )
        {
            using var connection = _connectionService.EstablishConnection();

            var response = connection.QueryAsync<House>(
                sql: "GetHouses",
                param: userId,
                commandType: CommandType.StoredProcedure);

            return response.Result.ToList();
        }
    }
}
