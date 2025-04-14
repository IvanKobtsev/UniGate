using UniGateAPI.DTOs.Common;
using UniGateAPI.Models;

namespace UniGateAPI.Mappers;

public static class ManagerReferenceMapper
{
    public static ManagerDto ToDto(this ManagerReference reference)
    {
        return new ManagerDto
        {
            UserId = reference.UserId,
            FullName = reference.FullName,
            AssignedFacultyId = reference.AssignedFacultyId,
            AssignedAdmissions = reference.AssignedAdmissions.ToLightDtos()
        };
    }
}