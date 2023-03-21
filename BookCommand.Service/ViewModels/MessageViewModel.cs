namespace BookCommand.Service.ViewModels
{
    /* This is a class that is used to pass data between the service and the client. */
    public class MessageViewModel
    {
        public Command Command { get; set; }
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
    }
}
