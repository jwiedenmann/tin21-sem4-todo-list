using Pyco.Todo.Data.Models;
using Pyco.Todo.DataAccess.Interfaces;

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
        IEnumerable<User> listUser = _userRepository.GetListUsers(listId);

        list.ListItems = listItems.ToList() ?? new List<ListItem>();
        list.ListUsers = listUser.ToList() ?? new List<User>();

        return list;
    }
}
