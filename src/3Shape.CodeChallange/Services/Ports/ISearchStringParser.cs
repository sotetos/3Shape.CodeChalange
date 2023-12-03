using Services.DTOs;

namespace Services.Ports;

public interface ISearchStringParser
{
    IEnumerable<ParsedSearchCondition> ParseSearchString(string input);
}