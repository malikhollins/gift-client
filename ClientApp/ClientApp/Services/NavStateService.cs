using ClientApp.Models;

namespace ClientApp.Services
{
    public class NavStateService
    {
        public event EventHandler<NavState>? OnNavStateChanged;

        public NavState? CurrentState { get; private set; }

        public void UpdateNavState(NavState navState)
        {
            CurrentState = navState;
            OnNavStateChanged?.Invoke( this, navState );
        }
    }
}
