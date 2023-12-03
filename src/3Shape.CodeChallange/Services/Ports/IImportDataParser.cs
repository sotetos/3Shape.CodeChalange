using Services.DTOs;

namespace Services.Ports;

public interface IImportDataParser
{
    IEnumerable<ParsedInputData> ParseInput(string input);
}