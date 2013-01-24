using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace GraphNet
{
    public abstract class DotElement
    {
        protected Dictionary<string, string> Styles { get; set; }

        protected DotElement()
        {
            Styles = new Dictionary<string, string>();
        }

        public void AddStyle(string property, string value)
        {
            Styles.Add(property, value);
        }

        protected string GetStyles()
        {
            if (Styles.Count == 0)
            {
                return "";
            }
            var output = new StringBuilder();
            output.Append(" [");
            var first = true;
            foreach (var style in Styles)
            {
                if (!first)
                {
                    output.Append(", ");
                }
                output.Append(style.Key);
                output.Append("=\"");
                output.Append(style.Value);
                output.Append("\"");
                first = false;
            }
            output.Append("]");
            return output.ToString();
        }

        protected string SanitizeName(string name)
        {
            var alphabetic = new Regex("[^a-zA-Z]");
            return alphabetic.Replace(name, "");
        }
    }
}