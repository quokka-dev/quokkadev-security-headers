using System.Text;

namespace QuokkaDev.SecurityHeaders.Csp
{
    public class ContentSecurityPolicy
    {
        private readonly Dictionary<string, Directive> directives;
        private string? policyString = null;

        internal ContentSecurityPolicy(string policyString) : this()
        {
            this.policyString = policyString;
        }

        public ContentSecurityPolicy()
        {
            this.directives = new Dictionary<string, Directive>();
        }

        internal Directive Add(string name, Directive directive)
        {
            if (!directives.ContainsKey(name))
            {
                directives.Add(name, directive);
            }
            return directives[name];
        }

        public string GetPolicyString()
        {
            if (policyString is null)
            {
                StringBuilder sb = new();
                foreach (var directive in directives)
                {
                    sb.Append(directive.Value.ToString());
                }
                policyString = sb.ToString().Trim();
            }
            return policyString;
        }
    }
}
