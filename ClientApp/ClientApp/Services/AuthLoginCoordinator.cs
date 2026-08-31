using System.Threading;

namespace ClientApp.Services
{
    // Coordinates login attempts without using static flags.
    // Uses a SemaphoreSlim to prevent concurrent/overlapping login flows.
    public class AuthLoginCoordinator
    {
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        // Try to enter without waiting. Returns true if caller may proceed with login.
        public bool TryEnter()
        {
            try
            {
                return _semaphore.Wait(0);
            }
            catch
            {
                return false;
            }
        }

        // Release after a successful TryEnter.
        public void Exit()
        {
            try
            {
                _semaphore.Release();
            }
            catch
            {
                // ignore release errors
            }
        }
    }
}
