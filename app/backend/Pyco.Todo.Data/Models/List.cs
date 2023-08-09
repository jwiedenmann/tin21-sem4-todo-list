namespace Pyco.Todo.Data.Models
{
    public class List
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool Archive { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
