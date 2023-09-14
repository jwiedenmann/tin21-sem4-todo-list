using Pyco.Todo.Data.Models;
using Pyco.Todo.DataAccess.Interfaces;
using System.Transactions;

namespace Pyco.Todo.DataAccess.DataProvider;

public class ListItemDataProvider : IListItemDataProvider
{
    private readonly IListRepository _listRepository;
    private readonly IListItemRepository _listItemRepository;

    public ListItemDataProvider(
        IListRepository listRepository,
        IListItemRepository listItemRepository)
    {
        _listRepository = listRepository;
        _listItemRepository = listItemRepository;
    }

    public int Insert(ListItem listItem, int userId)
    {
        using TransactionScope transaction = new();

        if (!_listRepository.IsListAdmin(listItem.ListId, userId)) return -1;
        int listItemId = _listItemRepository.Insert(listItem);

        transaction.Complete();
        return listItemId;
    }

    public void Update(ListItem listItem, int userId)
    {
        using TransactionScope transaction = new();

        if (!_listRepository.IsListAdmin(listItem.ListId, userId)) return;
        _listItemRepository.Update(listItem);

        transaction.Complete();
    }

    public void Archive(int listId, int listItemId, int userId)
    {
        using TransactionScope transaction = new();

        if (!_listRepository.IsListAdmin(listId, userId)) return;
        _listItemRepository.Archive(listItemId);

        transaction.Complete();
    }

    public void Check(int listId, int listItemId, int userId, bool isChecked)
    {
        using TransactionScope transaction = new();

        if (!_listRepository.IsListUser(listId, userId)) return;

        if (isChecked)
        {
            _listItemRepository.Check(listItemId, userId);
        }
        else
        {
            _listItemRepository.Uncheck(listItemId, userId);
        }

        transaction.Complete();
    }
}
