namespace BookCommand.Service.ViewModels
{
    public class MessageViewModel
    {
        public Command Command { get; set; }
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
    }
}
