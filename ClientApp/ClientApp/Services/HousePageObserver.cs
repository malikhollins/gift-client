using ClientApp.Models;

namespace ClientApp.Services
{
    public class HousePageObserver
    {
        public event EventHandler<House>? OnHouseUpdated;
        public void NotifyHouseCreated(House house) => OnHouseUpdated?.Invoke(this, house);
    }
}
