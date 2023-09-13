namespace Pyco.Todo.Data.Models;

public class ListItem
{
    public int Id { get; set; }
    public int ListId { get; set; }
    public ListItemType TypeId { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool Archive { get; set; }
    public DateTime CreationDate { get; set; }

    public List<int> CheckedByUserIds { get; set; } = new List<int>();
}
