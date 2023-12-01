using Services.Internals.Models;

namespace Services.Ports;

public interface ISearchStringParser
{
    IEnumerable<ParsedSearchCondition> ParseSearchString(string input);
}