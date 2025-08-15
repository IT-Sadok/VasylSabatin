using MyWebApp.DTO;
using MyWebApp.Models;

namespace MyWebApp.Mapper;

public static class UserMapper
{
    public static UserProfileModel ToModel(this User entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        return new UserProfileModel()
        {
            FullName = entity.FullName,
            Age = entity.Age,
            Weight = entity.Weight,
            Email = entity.Email,
            AccountDescription = entity.AccountDescription
        };
    }

    public static void ApplyTo(this UserUpdateModel model, User entity)
    {
        entity.FullName = model.FullName;
        entity.Age = model.Age;
        entity.Weight = model.Weight;
        entity.AccountDescription = model.AccountDescription;
    }
}