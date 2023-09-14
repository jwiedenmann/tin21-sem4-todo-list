using Pyco.Todo.Data.Models;
using Pyco.Todo.DataAccess.Interfaces;
using System.Transactions;

namespace Pyco.Todo.DataAccess.DataProvider;

public class ListDataProvider : IListDataProvider
{
    private readonly IListRepository _listRepository;
    private readonly IListItemRepository _listItemRepository;
    private readonly IUserRepository _userRepository;

    public ListDataProvider(
        IListRepository listRepository,
        IListItemRepository listItemRepository,
        IUserRepository userRepository)
    {
        _listRepository = listRepository;
        _listItemRepository = listItemRepository;
        _userRepository = userRepository;
    }

    public List? Get(int listId, int userId)
    {
        if (!_listRepository.IsListUser(listId, userId)) return null;

        List? list = _listRepository.Get(listId);

        if (list is null) return null;

        IEnumerable<ListItem> listItems = _listItemRepository.Get(listId);
        Dictionary<int, List<int>> listItemChecks = _listItemRepository.GetListItemChecks(listId);
        IEnumerable<User> listUser = _userRepository.GetListUsers(listId);

        foreach (var item in listItems)
        {
            if(listItemChecks.TryGetValue(item.Id, out List<int>? listChecks) &&
                listChecks is not null)
            {
                item.CheckedByUserIds = listChecks;
            }
        }

        list.ListItems = listItems.ToList() ?? new List<ListItem>();
        list.ListUsers = listUser.ToList() ?? new List<User>();

        return list;
    }

    public int Insert(List list)
    {
        using TransactionScope transaction = new();

        int listId = _listRepository.Insert(list);

        foreach (User listUser in list.ListUsers ?? new())
        {
            _listRepository.InsertListUser(listId, listUser.Id, listUser.ListUserRole);
        }

        transaction.Complete();

        return listId;
    }

    public void Update(List list, int userId)
    {
        using TransactionScope transaction = new();

        if (!_listRepository.IsListAdmin(list.Id, userId)) return;

        _listRepository.Update(list);
        _listRepository.DeleteListUsers(list.Id);

        foreach (User listUser in list.ListUsers ?? new())
        {
            _listRepository.InsertListUser(list.Id, listUser.Id, listUser.ListUserRole);
        }

        transaction.Complete();
    }
}
