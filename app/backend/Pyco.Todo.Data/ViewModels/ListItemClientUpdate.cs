namespace Pyco.Todo.Data.ViewModels;
/// <summary>
/// Information that a client sends to the server when updating list items
/// </summary>
public class ListItemClientUpdate
{
    /// <summary>
    /// The ID of the last synced revision
    /// </summary>
    public int LastSyncedRevision { get; set; }
    /// <summary>
    /// The Id of the updated listitem
    /// </summary>
    public int ListItemId { get; set; }
    /// <summary>
    /// The ID of the user that sent the update
    /// </summary>
    public int UserId { get; set; }
    /// <summary>
    /// Determines the type of update. True, if Insert; False, if delete operation.
    /// </summary>
    public bool IsInsert { get; set; }
    /// <summary>
    /// The position on which the update occured
    /// </summary>
    public int Position { get; set; }
    /// <summary>
    /// The length of the update (inerted/deleted characters)
    /// </summary>
    public int Length { get; set; }
    /// <summary>
    /// The inserted characters.
    /// </summary>
    public string Value { get; set; } = string.Empty;
}
