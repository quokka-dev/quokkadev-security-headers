using System.Security.Cryptography;

namespace QuokkaDev.SecurityHeaders.Csp
{
    internal class NonceService
    {

        public string RequestNonce { get; }

        public NonceService()
        {
            var ByteArray = new byte[20];
            using (var Rnd = RandomNumberGenerator.Create())
            {
                Rnd.GetBytes(ByteArray);
            }

            RequestNonce = Convert.ToBase64String(ByteArray);
        }
    }
}
