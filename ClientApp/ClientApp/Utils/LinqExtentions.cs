namespace ClientApp.Utils
{
    public static class LinqExtentions
    {
        public static bool IsNullOrEmpty<T>( this IEnumerable<T>? source )
        {
            return source == null || !source.Any();
        }
    }
}
