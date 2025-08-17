using Dapper;
using ServerApp.Models;
using WebAPI.Services;

namespace ServerApp.Services
{
    public class ListService
    {
        private readonly IConnectionService _connectionService;

        public ListService(IConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        public async Task<IEnumerable<List>> BulkGetListsAsync(int houseId)
        { 
            using var connection = _connectionService.EstablishConnection();

            var paremeters = new DynamicParameters();
            paremeters.Add("house_id", houseId);

            var result = await connection.QueryAsync<List>( 
                sql: "[dbo].[BulkGetLists]",
                param: paremeters,
                commandType: System.Data.CommandType.StoredProcedure );

            return result;
        }

        public async Task<List> CreateListAsync(int houseId, int userId, string? name)
        {
            using var connection = _connectionService.EstablishConnection();

            var paremeters = new DynamicParameters();
            paremeters.Add("house_id", houseId);
            paremeters.Add("user_id", userId);
            paremeters.Add("name", name);

            var result = await connection.QueryFirstAsync<List>(
                sql: "[dbo].[CreateList]",
                param: paremeters,
                commandType: System.Data.CommandType.StoredProcedure);

            return result;

        }

        public async Task<Item> CreateItemInListAsync(
            int listId,  
            string name, 
            string? description, 
            double price, 
            string? link)
        {
            using var connection = _connectionService.EstablishConnection();

            var paremeters = new DynamicParameters();
            paremeters.Add("list_id", listId);
            paremeters.Add("name", name);
            paremeters.Add("description", description);
            paremeters.Add("price", price);
            paremeters.Add("link", link);

            var result = await connection.QueryFirstAsync<Item>(
                sql: "[dbo].[CreateItemInList]",
                param: paremeters,
                commandType: System.Data.CommandType.StoredProcedure
                );

            return result;
        }

        public async Task<IEnumerable<Item>> BulkGetItemsFromListAsync(int listId)
        {
            using var connection = _connectionService.EstablishConnection();

            var paremeters = new DynamicParameters();
            paremeters.Add("list_id", listId);

            var result = await connection.QueryAsync<Item>(
                sql: "[dbo].[BulkGetItemsFromList]",
                param: paremeters,
                commandType: System.Data.CommandType.StoredProcedure
                );

            return result;
        }
        public async Task UpdateBuyerInItemAsync(int itemId, int buyerId)
        {
            using var connection = _connectionService.EstablishConnection();

            var paremeters = new DynamicParameters();
            paremeters.Add("item_id", itemId);
            paremeters.Add("buyer_id", buyerId);

            await connection.ExecuteAsync(
                sql: "[dbo].[UpdateBuyerInItem]",
                param: paremeters,
                commandType: System.Data.CommandType.StoredProcedure
                );
        }

        public async Task UpdateFavoriteItemAsync(int itemId, bool favorited)
        {
            using var connection = _connectionService.EstablishConnection();

            var paremeters = new DynamicParameters();
            paremeters.Add("item_id", itemId);
            paremeters.Add("favorited", favorited);

            await connection.ExecuteAsync(
                sql: "[dbo].[UpdateFavoriteItem]",
                param: paremeters,
                commandType: System.Data.CommandType.StoredProcedure
                );
        }
    }
}
