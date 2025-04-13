using UniGate.UserService.DTOs.Common;
using UniGate.UserService.DTOs.Requests;
using UniGate.UserService.Models;

namespace UniGate.UserService.Mappers;

public static class UserMapper
{
    public static ProfileDto ToDto(this User user, List<string> roles)
    {
        return new ProfileDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Patronymic = user.Patronymic,
            Email = user.Email!,
            PhoneNumber = user.PhoneNumber,
            Roles = roles
        };
    }

    public static User ToUser(this RegisterUserDto userDto)
    {
        return new User
        {
            UserName = userDto.Email,
            Email = userDto.Email,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Patronymic = userDto.Patronymic ?? string.Empty,
            PhoneNumber = userDto.PhoneNumber ?? string.Empty
        };
    }
}