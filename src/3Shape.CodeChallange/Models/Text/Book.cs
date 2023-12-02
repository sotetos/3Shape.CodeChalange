namespace Models.Text
{
    public class Book: TextItemBase
    {
        public int RoomId { get; set; }
        public int RowId { get; set; }
        public int ShelfId { get; set; }
        public Book() 
        {
            LibraryItemType = LibraryItemType.Book;
        }
    }
}
