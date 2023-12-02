using System.Text.RegularExpressions;
using Services.Internals.Models;
using Services.Ports;

namespace Services.Internals
{
    internal class SearchStringParser : ISearchStringParser
    {
        public IEnumerable<ParsedSearchCondition> ParseSearchString(string input)
        {
            ArgumentNullException.ThrowIfNull(input);
            if (string.IsNullOrWhiteSpace(input))
            {
                //I'm assuming that an empty search string indicates no condition, thus all results
                return Array.Empty<ParsedSearchCondition>();
            }

            //I am not good at regular expressions.  I got this one here: https://stackoverflow.com/questions/11819059/regex-match-character-which-is-not-escaped
            var conditions = Regex.Split(input, "(?<!\\\\)(?:\\\\\\\\)*&");

            return conditions
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c => new ParsedSearchCondition(c.Trim().Trim('*')));
        }
    }
}
