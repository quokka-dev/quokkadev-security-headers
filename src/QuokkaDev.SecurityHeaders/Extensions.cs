namespace QuokkaDev.SecurityHeaders
{
    internal static class Extensions
    {
        internal static string? DashReplace(this object o)
        {
            return o?.ToString()?.Replace('_', '-');
        }
    }
}
