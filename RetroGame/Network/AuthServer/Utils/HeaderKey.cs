namespace AuthServer.Utils
{
    internal static class HeaderKey
    {
        internal static string _headerKey;
        internal static string Get()
        {
            return _headerKey;
        }

        internal static void Set(string value)
        {
            _headerKey = value;
        }
    }
}
