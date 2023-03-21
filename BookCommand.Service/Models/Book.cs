namespace BookCommand.Service.Models
{
    /* The Book class has an Id, Title, Description, Author, and IsDeleted property */
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public bool IsDeleted { get; set; }
    }
}
