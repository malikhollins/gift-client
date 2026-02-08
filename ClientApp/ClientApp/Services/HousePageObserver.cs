using ClientApp.Models;

namespace ClientApp.Services
{
    public class HousePageObserver : ObserverBase<House>
    {
        public override event EventHandler<House>? OnCreated;

        public override void NotifyCreated(House house) => OnCreated?.Invoke(this, house);
    }
}
