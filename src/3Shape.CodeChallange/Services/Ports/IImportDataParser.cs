using Services.Internals.Models;

namespace Services.Ports;

public interface IImportDataParser
{
    IEnumerable<ParsedInputData> ParseInput(string inputString);
}