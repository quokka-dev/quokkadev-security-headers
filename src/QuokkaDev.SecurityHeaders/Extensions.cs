using Microsoft.AspNetCore.Http;
using QuokkaDev.SecurityHeaders.Csp;

namespace QuokkaDev.SecurityHeaders
{
    public static class Extensions
    {
        internal static string? DashReplace(this object o)
        {
            return o?.ToString()?.Replace('_', '-');
        }

        public static string GetNonceAttribute(this IHttpContextAccessor accessor)
        {
            var service = accessor.HttpContext.RequestServices.GetService(typeof(NonceService));
            if (service is NonceService nonceService)
            {
                return $"nonce=\"{nonceService.RequestNonce}\"";
            }
            else
            {
                throw new InvalidOperationException("NonceService is not configured");
            }
        }
    }
}
