namespace QuokkaDev.SecurityHeaders.Csp
{
    public interface INonceService
    {
        string RequestNonce { get; }
    }
}