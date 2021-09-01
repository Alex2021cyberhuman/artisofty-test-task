using System;
using Logic.Users.Models;

namespace Logic.Users.Utilities
{
    public static class UserMutationHelper
    {
        public static User GetUserWithUpdatedLastLogin(DateTime login, User user) =>
            user with
            {
                LastLogin = login
            };
    }
}