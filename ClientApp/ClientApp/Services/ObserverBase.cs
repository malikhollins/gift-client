using ClientApp.Models;

namespace ClientApp.Services
{
    public abstract class ObserverBase<T>
    {
        public abstract event EventHandler<T>? OnCreated;

        public abstract void NotifyCreated(T house);
    }
}