using System;
using FluentMigrator;
using Logic.Users.Options;
using static Logic.Users.DataAccess.Database.Configuration.DatabaseNames;
using static Logic.Users.DataAccess.Database.Configuration.DatabaseNames.
    UsersColumns;

namespace Logic.Users.DataAccess.Database.FluentMigrations
{
    [Migration(20210902065553)]
    public class InitialFluentMigration : Migration
    {
        public override void Up()
        {
            Create.Table(UsersTable)
                .WithColumn(Id)
                .AsInt32()
                .PrimaryKey()
                .Identity()
                .WithColumn(FIO)
                .AsString(UserConfigurationOptions.UserFIOMaxLength + 1)
                .NotNullable()
                .WithColumn(Phone)
                .AsFixedLengthAnsiString(UserConfigurationOptions
                    .UserPhoneMaxLength)
                .NotNullable()
                .Unique()
                .WithColumn(Email)
                .AsString(UserConfigurationOptions.UserEmailMaxLength + 1)
                .NotNullable()
                .Unique()
                .WithColumn(Password)
                .AsString(UserConfigurationOptions.UserPasswordMaxLength + 1)
                .NotNullable()
                .Indexed()
                .WithColumn(LastLogin)
                .AsDateTime2()
                .WithDefaultValue(DateTime.MinValue)
                .NotNullable();
        }

        public override void Down()
        {
            Delete.Table(UsersTable);
        }
    }
}