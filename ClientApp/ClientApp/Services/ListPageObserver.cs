using ClientApp.Models;

namespace ClientApp.Services
{
    public class ListPageObserver : ObserverBase<UserList>
    {
        public override event EventHandler<UserList>? OnCreated;

        public override void NotifyCreated(UserList house) => OnCreated?.Invoke(this, house);
    }
}
