using System.Text;

namespace QuokkaDev.SecurityHeaders.PermissionPolicy
{
    public class Directive
    {
        public HashSet<string> AllowedSources { get; set; }
        public string? Name { get; set; }

        public Directive()
        {
            this.AllowedSources = new HashSet<string>();
        }

        public Directive AddSource(string source)
        {
            if (source == "self" || source == "")
            {
                this.AllowedSources?.Add(source);
            }
            else
            {
                this.AllowedSources?.Add($"\"{source}\"");
            }
            return this;
        }

        public Directive Self()
        {
            return AddSource("self");
        }

        public override string ToString()
        {
            StringBuilder sb = new($"{Name}=(");
            if (AllowedSources.Count > 0)
            {
                foreach (string source in AllowedSources)
                {
                    sb.Append(source).Append(' ');
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(')');
                return sb.ToString();
            }
            return sb.ToString().Trim();
        }
    }
}
