using System.Text;

namespace QuokkaDev.SecurityHeaders.Csp
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
            this.AllowedSources?.Add(source);
            return this;
        }

        public Directive All()
        {
            return AddSource("*");
        }

        public Directive None()
        {
            return AddSource("'none'");
        }

        public Directive Self()
        {
            return AddSource("'self'");
        }

        public Directive UnsafeInline()
        {
            return AddSource("'unsafe-inline'");
        }

        public Directive UnsafeEval()
        {
            return AddSource("'unsafe-eval'");
        }

        public override string ToString()
        {
            StringBuilder sb = new($"{Name} ");
            if (AllowedSources.Count > 0)
            {
                foreach (string source in AllowedSources.Where(s => !string.IsNullOrWhiteSpace(s)))
                {
                    sb.Append(source).Append(' ');
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("; ");
                return sb.ToString();
            }
            return sb.ToString().Trim();
        }
    }
}
