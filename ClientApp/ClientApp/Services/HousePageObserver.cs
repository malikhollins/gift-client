using ClientApp.Models;

namespace ClientApp.Services
{
    public class HousePageObserver : ObserverBase<UpdateEventHouseArgs>
    {
        public override event EventHandler<UpdateEventHouseArgs>? OnUpdated;

        public override void NotifyUpdated(UpdateEventHouseArgs eventArguments) =>
            OnUpdated?.Invoke(this, eventArguments);
    }
}