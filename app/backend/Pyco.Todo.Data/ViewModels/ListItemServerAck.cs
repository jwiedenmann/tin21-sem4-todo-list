namespace Pyco.Todo.Data.ViewModels;
/// <summary>
/// Information that a server sends to the clients 
/// </summary>
public class ListItemServerAck
{
    /// <summary>
    /// The ID of the new revision
    /// </summary>
    public int NewRevisionId { get; set; }
    /// <summary>
    /// Information for the acknowledged uodate
    /// </summary>
    public ListItemClientUpdate? ListItemClientUpdate { get; set; }
}
