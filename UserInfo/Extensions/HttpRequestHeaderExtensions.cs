namespace UserInfo.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    public static class HttpRequestHeaderExtensions
    {
        public static string GetHeaderValue(this HttpRequestMessage request, string name)
        {
            var found = request.Headers.TryGetValues(name, out IEnumerable<string> values);
            if (found)
            {
                return values.FirstOrDefault();
            }

            return null;
        }
    }
}