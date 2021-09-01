using Logic.Users.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Logic.Users.Options.UserConfigurationOptions;

namespace Logic.Users.DataAccess.Database
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(user => user.Id);
            
            builder.Property(user => user.FIO).IsRequired().HasMaxLength(UserFIOMaxLength);
            
            builder.Property(user => user.Phone).IsRequired().HasMaxLength(UserPhoneMaxLength);
            builder.HasIndex(user => user.Phone).IsUnique();

            builder.Property(user => user.Email).IsRequired().HasMaxLength(UserEmailMaxLength);
            builder.HasIndex(user => user.Email).IsUnique();

            builder.Property(user => user.Password).IsRequired().HasMaxLength(UserPasswordMaxLength);
        }
    }
}