using AOL_Reborn.Models;
using System.Collections.ObjectModel;

namespace AOL_Reborn.ViewModels
{
    internal class BuddyListViewModel
    {
        // A list of users that represent your friend list.
        public ObservableCollection<User> Friends { get; set; } = new ObservableCollection<User>();

        // Optionally, add methods to load, add, or remove friends.
    }
}
