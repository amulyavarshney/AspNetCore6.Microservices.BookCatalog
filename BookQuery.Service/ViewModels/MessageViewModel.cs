namespace BookQuery.Service.ViewModels
{
    /* The MessageViewModel class is a class that contains a Command property, a BookId property, a
    Title property, a Description property, and an Author property */
    public class MessageViewModel
    {
        public Command Command { get; set; }
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
    }
}