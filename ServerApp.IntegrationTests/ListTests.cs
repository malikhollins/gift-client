using ServerApp.Models;
using ServerApp.Services;
using WebAPI.Models;

namespace ServerApp.IntegrationTests
{
    public class ListTests
    {
        private const string listName = "name";
        private const string itemName = "name";
        private const double itemPrice = 10.99;

        private static ListService CreateListService()
        {
            var connectionService = ConnectionTests.SetUpConnection();
            return new ListService(connectionService);
        }

        private static async Task<List> SetupBasicListAsync( ListService listService, House house, User user )
        {
            return await listService.CreateListAsync(house.Id, user.Id, listName);
        }

        [Fact]
        public async Task GetList_NoError()
        {
            var (house, user) = await TestUtils.CreateHouseAsync();
            var listService = CreateListService();
            var createdList = await SetupBasicListAsync( listService, house, user );
            var result = await listService.BulkGetListsAsync(house.Id);
            Assert.NotNull( result );
            var list = result.FirstOrDefault();
            Assert.NotNull( list );
            Assert.Equal(house.Id, list.House);
            Assert.Equal(user.Id, list.Owner);
            Assert.Equal(listName, list.Name);
        }

        [Fact]
        public async Task CreateList_NoError()
        {
            var (house, user) = await TestUtils.CreateHouseAsync();
            var listService = CreateListService();
            var result = await listService.CreateListAsync(house.Id, user.Id, listName);
            Assert.NotNull(result);
            Assert.Equal(house.Id, result.House);
            Assert.Equal(user.Id, result.Owner);
            Assert.Equal(listName, result.Name);
        }

        [Fact]
        public async Task CreateItemInList_NoError()
        {
            var (house, user) = await TestUtils.CreateHouseAsync();
            var listService = CreateListService();
            var list = await listService.CreateListAsync(house.Id, user.Id, listName);
            var item = await listService.CreateItemInListAsync(list.Id, itemName, null, itemPrice, null);
            Assert.NotNull(item);
            Assert.Equal(itemName, item.Name);
            Assert.Equal(itemPrice, item.Price);
            Assert.Null(item.Description);
            Assert.Null(item.Link);
        }

        [Fact]
        public async Task GetItemsInList_NoError()
        {
            var (house, user) = await TestUtils.CreateHouseAsync();
            var listService = CreateListService();
            var list = await listService.CreateListAsync(house.Id, user.Id, listName);
            var item = await listService.CreateItemInListAsync(list.Id, itemName, null, itemPrice, null);
            var items = await listService.BulkGetItemsFromListAsync(list.Id);
            Assert.NotNull(items);
            Assert.NotEmpty(items);
            var testItem = items.FirstOrDefault();
            Assert.NotNull(testItem);
            Assert.Equal(item.Id, testItem.Id);
            Assert.Equal(item.Name, testItem.Name);
            Assert.Equal(item.Price, testItem.Price);
        }


        [Fact]
        public async Task UpdateFavoriteInList_NoError()
        {
            var (house, user) = await TestUtils.CreateHouseAsync();
            var listService = CreateListService();
            var list = await listService.CreateListAsync(house.Id, user.Id, listName);
            var item = await listService.CreateItemInListAsync(list.Id, itemName, null, itemPrice, null);
            await listService.UpdateFavoriteItemAsync(item.Id, favorited: true );
            var items = await listService.BulkGetItemsFromListAsync(list.Id);
            Assert.NotNull(items);
            Assert.NotEmpty(items);
            var testItem = items.FirstOrDefault();
            Assert.NotNull(testItem);
            Assert.True(testItem.Favorite);
        }

        [Fact]
        public async Task UpdateBuyerInList_NoError()
        {
            var (house, user) = await TestUtils.CreateHouseAsync();
            var listService = CreateListService();
            var list = await listService.CreateListAsync(house.Id, user.Id, listName);
            var item = await listService.CreateItemInListAsync(list.Id, itemName, null, itemPrice, null);
            await listService.UpdateBuyerInItemAsync(item.Id, user.Id);
            var items = await listService.BulkGetItemsFromListAsync(list.Id);
            Assert.NotNull(items);
            Assert.NotEmpty(items);
            var testItem = items.FirstOrDefault();
            Assert.NotNull(testItem);
            Assert.Equal(testItem.Buyer, user.Id);
        }
    }
}
