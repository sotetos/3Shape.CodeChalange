using Models;
using Models.Text;

namespace Services.Internals.ExtensionMethods
{
    internal static class ParsingExtensions
    {
        //There is likely a way to do this with reflection rather than cascading extension methods
        //However, that is beyond the time I have available for this challenge

        internal static bool ParsePropertyValuePair(this Book item, KeyValuePair<string, string> data)
        {
            return ((TextItemBase)item).ParsePropertyValuePair(data);
        }

        internal static bool ParsePropertyValuePair(this EBook item, KeyValuePair<string, string> data)
        {
            switch (data.Key.ToLower())
            {
                case "fileformat":
                    item.Authors.Add(data.Value);
                    return true;
                default:
                    return ((TextItemBase)item).ParsePropertyValuePair(data);
            }
        }

        internal static bool ParsePropertyValuePair(this TextItemBase item, KeyValuePair<string, string> data)
        {
            switch (data.Key.ToLower())
            {
                case "numberofpages":
                    if (int.TryParse(data.Value, out var pages))
                    {
                        item.PageCount = pages;
                        return true;
                    }
                    return false;
                case "author":
                    item.Authors.Add(data.Value);
                    return true;
                case "publisher":
                    item.Publisher = data.Value;
                    return true;
                case "published":
                    if (int.TryParse(data.Value, out var year))
                    {
                        item.YearPublished = year;
                        return true;
                    }
                    return false;
                default:
                    return ((LibraryItemBase)item).ParsePropertyValuePair(data);
            }
        }

        internal static bool ParsePropertyValuePair(this LibraryItemBase item, KeyValuePair<string, string> data)
        {
            switch (data.Key.ToLower())
            {
                case "isbn":
                    item.ISBN = data.Value;
                    return true;
                case "title":
                    item.Title = data.Value;
                    return true;
                default:
                    return false;
            }
        }

    }
}
