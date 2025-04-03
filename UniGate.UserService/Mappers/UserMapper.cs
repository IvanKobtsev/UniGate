using UniGate.UserService.DTOs.Common;
using UniGate.UserService.Models;

namespace UniGate.UserService.Mappers;

public static class UserMapper
{
    public static ProfileDto ToDto(this User user)
    {
        return new ProfileDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Patronymic = user.Patronymic,
            Email = user.Email!,
            PhoneNumber = user.PhoneNumber
        };
    }
}