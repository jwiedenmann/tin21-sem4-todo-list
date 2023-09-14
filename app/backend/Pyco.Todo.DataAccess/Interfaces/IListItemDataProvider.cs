using Pyco.Todo.Data.Models;

namespace Pyco.Todo.DataAccess.Interfaces;

public interface IListItemDataProvider
{
    void Archive(int listId, int listItemId, int userId);
    void Check(int listId, int listItemId, int userId, bool isChecked);
    int Insert(ListItem listItem, int userId);
    void Update(ListItem listItem, int userId);
}
