using System.Text.RegularExpressions;

namespace Text_processor
{
    public class SubPattern
    {
        public string Value { get; set; }
        public string RegexPattern { get; set; }
        public override string ToString()
        {
            if (RegexPattern == null || RegexPattern.Length == 0)
                return Value;
            else
                return $"{Value} R:({RegexPattern})";
        }
    }
}
