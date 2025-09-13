namespace ClientApp.Utils
{
    public static class FireAndForget
    {
        public static void SafeFireAndForget(this Task task, Action<Exception>? onException = null)
        {
            task.ContinueWith(t =>
            {
                if (t.IsFaulted && t.Exception != null)
                {
                    onException?.Invoke(t.Exception);
                }
            }, TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}
