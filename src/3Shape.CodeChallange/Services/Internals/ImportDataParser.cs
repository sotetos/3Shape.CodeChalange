using Models;
using Services.Internals.Models;
using Services.Ports;

namespace Services.Internals
{
    internal class ImportDataParser : IImportDataParser
    {
        public IEnumerable<ParsedInputData> ParseInput(string inputString)
        {
            ArgumentNullException.ThrowIfNull(inputString);
            if (string.IsNullOrWhiteSpace(inputString))
            {
                throw new ArgumentException(nameof(inputString));
            }

            using var dataStream = new MemoryStream();
            using var streamWriter = new StreamWriter(dataStream);
            streamWriter.Write(inputString);
            streamWriter.Flush();
            dataStream.Position = 0;

            using var streamReader = new StreamReader(dataStream);

            var resultData = new List<ParsedInputData>();
            var currentResult = new ParsedInputData();

            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    resultData.Add(currentResult);
                    currentResult = new ParsedInputData();
                    continue;
                }

                //Note: This approach would fail if a property value had a : in it.
                var lineData = line
                    .Split(':')
                    .Select(l => l.Trim())
                    .Where(l => !string.IsNullOrWhiteSpace(l));

                switch (lineData.Count())
                {
                    case 0:
                        resultData.Add(currentResult);
                        currentResult = new ParsedInputData();
                        continue;
                    //line indicates the type of item to import
                    case 1 when Enum.TryParse<LibraryItemType>(lineData.First(), true, out var itemType):
                        currentResult.InputType = itemType;
                        continue;
                    case 1:
                        currentResult.AddError(
                            $"Unable to parse item type from {lineData.First()} for item number {resultData.Count + 1}");
                        continue;
                    case 2:
                        currentResult.AddPropertyValueData(lineData.First(), lineData.Last());
                        continue;
                    default:
                        currentResult.AddError(
                            $"Unable to parse property value from {line}");
                        continue;

                }
            }

            if (!currentResult.Equals(resultData.LastOrDefault()))
            {
                resultData.Add(currentResult);
            }

            return resultData;
        }
    }
}
