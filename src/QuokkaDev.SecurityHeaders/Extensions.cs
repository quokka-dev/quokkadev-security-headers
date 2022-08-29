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

        public static string GetNonce(this IHttpContextAccessor accessor)
        {
            var service = accessor.HttpContext.RequestServices.GetService(typeof(INonceService));
            if (service is INonceService nonceService)
            {
                return nonceService.RequestNonce;
            }
            else
            {
                throw new InvalidOperationException("NonceService is not configured");
            }
        }

        public static string GetNonceAttribute(this IHttpContextAccessor accessor)
        {
            return $"nonce=\"{GetNonce(accessor)}\"";
        }
    }
}
