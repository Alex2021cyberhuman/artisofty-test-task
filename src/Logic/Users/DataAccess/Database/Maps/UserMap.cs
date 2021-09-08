using Dapper.FluentMap.Mapping;
using Logic.Users.Models;
using static Logic.Users.DataAccess.Database.Configuration.DatabaseNames.
    UsersColumns;

namespace Logic.Users.DataAccess.Database.Maps
{
    public class UserMap : EntityMap<User>
    {
        public UserMap()
        {
            Map(u => u.Id)
                .ToColumn(Id);
            Map(u => u.FIO)
                .ToColumn(FIO);
            Map(u => u.Phone)
                .ToColumn(Phone);
            Map(u => u.Email)
                .ToColumn(Email);
            Map(u => u.Password)
                .ToColumn(Password);
            Map(u => u.LastLogin)
                .ToColumn(LastLogin);
        }
    }
}