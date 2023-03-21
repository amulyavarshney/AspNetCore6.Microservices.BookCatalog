using System.ComponentModel.DataAnnotations;

namespace BookQuery.Service.Models
{
    /* The Book class has four properties: Id, Title, Description, and Author */
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
    }
}
