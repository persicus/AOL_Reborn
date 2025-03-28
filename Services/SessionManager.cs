﻿using AOL_Reborn.Models;

public static class SessionManager
{
    static User? _currentUser;

    public static void SetCurrentUser(User user) => _currentUser = user;

    public static User GetCurrentUser()
    {
        return _currentUser ?? new User { Id = -1, Username = "Guest", DisplayName = "Guest" };
    }

    public static bool IsUserLoggedIn() => _currentUser != null;
}