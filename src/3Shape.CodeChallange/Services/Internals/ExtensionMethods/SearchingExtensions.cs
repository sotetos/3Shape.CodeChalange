using Models;
using Models.Text;
using Services.Internals.Models;

namespace Services.Internals.ExtensionMethods
{
    internal static class SearchingExtensions
    {
        internal static bool Search(this Book item, ParsedSearchCondition data)
        {
            if (data.IntegerValue.HasValue)
            {
                if (item.RoomId.ToString().Contains(data.StringValue))
                    return true;
                if (item.RowId.ToString().Contains(data.StringValue))
                    return true;
                if (item.ShelfId.ToString().Contains(data.StringValue))
                    return true;

            }

            return ((TextItemBase)item).Search(data);
        }

        internal static bool Search(this EBook item, ParsedSearchCondition data)
        {
            return item.FileFormat.Contains(data.StringValue) 
                   || ((TextItemBase)item).Search(data);
        }

        internal static bool Search(this TextItemBase item, ParsedSearchCondition data)
        {
            if (data.IntegerValue.HasValue && item.YearPublished.ToString().Contains(data.StringValue))
                return true;
            if (item.Authors.Any(a => a.Contains(data.StringValue)))
            {
                return true;
            }
            if (item.Publisher.Contains(data.StringValue))
            {
                return true;
            }
            return ((LibraryItemBase)item).Search(data);
        }

        internal static bool Search(this LibraryItemBase item, ParsedSearchCondition data)
        {
            if (data.IntegerValue.HasValue && item.ISBN.ToString().Contains(data.StringValue))
                return true;
            if (item.Title.Contains(data.StringValue))
                return true;
            return false;
        }
    }
}
