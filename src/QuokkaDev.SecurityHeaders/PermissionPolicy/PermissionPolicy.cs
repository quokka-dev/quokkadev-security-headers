using System.Text;

namespace QuokkaDev.SecurityHeaders.PermissionPolicy
{
    public class PermissionPolicy
    {
        private readonly Dictionary<string, Directive> directives;
        private string? policyString = null;

        internal PermissionPolicy(string policyString) : this()
        {
            this.policyString = policyString;
        }

        public PermissionPolicy()
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
                    sb.Append(", ");
                }
                if (sb.Length >= 2)
                {
                    sb.Remove(sb.Length - 2, 2);
                }
                policyString = sb.ToString().Trim();
            }
            return policyString;
        }
    }
}
