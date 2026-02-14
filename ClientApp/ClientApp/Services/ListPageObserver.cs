using ClientApp.Models;

namespace ClientApp.Services
{
    public class ListPageObserver : ObserverBase<UpdateEventListArgs>
    {
        public override event EventHandler<UpdateEventListArgs>? OnUpdated;

        public override void NotifyUpdated(UpdateEventListArgs eventArguments) => OnUpdated?.Invoke(this, eventArguments);
    }
}
