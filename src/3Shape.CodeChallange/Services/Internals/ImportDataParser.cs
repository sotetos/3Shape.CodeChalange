using Models;
using Services.Internals.Models;
using Services.Ports;

namespace Services.Internals
{
    internal class ImportDataParser : IImportDataParser
    {
        public IEnumerable<ParsedInputData> ParseInput(string inputString)
        {
            using var dataStream = new MemoryStream();
            using var streamWriter = new StreamWriter(dataStream);
            streamWriter.Write(inputString);
            streamWriter.Flush();

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

                var lineData = line.Trim().Split(':');

                switch (lineData.Length)
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
                        currentResult.AddPropertyValueData(new KeyValuePair<string, string>(lineData[0], lineData[1]));
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
