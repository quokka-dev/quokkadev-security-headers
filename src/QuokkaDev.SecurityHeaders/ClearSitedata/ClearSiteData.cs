using Microsoft.Extensions.Configuration;
using System.Text;

namespace QuokkaDev.SecurityHeaders.ClearSitedata
{
    public class ClearSiteData
    {
        private static readonly string[] allowedValues = new string[] { "cache", "cookies", "storage", "executionContexts", "*" };
        private readonly List<string> values = new();

        public ClearSiteData()
        {
        }

        public ClearSiteData(IEnumerable<string> values)
        {
            AddValues(values);
        }

        public ClearSiteData(params string[] values)
        {
            AddValues(values);
        }
        public static ClearSiteData? ReadFromConfig(IConfiguration config, string sectionName)
        {
            ClearSiteData? result = null;
            var configValues = config?.GetSection(sectionName).Get<string[]>();
            if (configValues?.Length > 0)
            {
                result = new ClearSiteData(configValues);
            }
            return result;
        }

        public ClearSiteData ClearCache()
        {
            AddValues(new string[] { "cache" });
            return this;
        }

        public ClearSiteData ClearCookies()
        {
            AddValues(new string[] { "cookies" });
            return this;
        }

        public ClearSiteData ClearStorage()
        {
            AddValues(new string[] { "storage" });
            return this;
        }

        public ClearSiteData ClearExecutionContexts()
        {
            AddValues(new string[] { "executionContexts" });
            return this;
        }

        public ClearSiteData ClearAll()
        {
            AddValues(new string[] { "*" });
            return this;
        }

        public override string ToString()
        {
            StringBuilder sb = new("");
            foreach (string value in values)
            {
                sb.AppendFormat("\"{0}\",", value);
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        private void AddValues(IEnumerable<string> values)
        {
            this.values.AddRange(values?.Intersect(allowedValues).Except(this.values) ?? Array.Empty<string>());
        }
    }
}
