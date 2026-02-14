using ClientApp.Models;

namespace ClientApp.Services
{
    public abstract class ObserverBase<T>
    {
        public abstract event EventHandler<T>? OnUpdated;
        public abstract void NotifyUpdated(T eventArguments);
    }
}