using UniGate.Common.Enums;
using UniGate.Common.Utilities;
using UniGateAPI.DTOs.Common;
using UniGateAPI.Enums;
using UniGateAPI.Models;

namespace UniGateAPI.Interfaces;

public interface IAdmissionRepository
{
    public Task<Admission?> GetAdmissionById(Guid admissionId, bool includeChosenPrograms = false);
    public Task<Admission?> RetrieveAdmissionById(Guid admissionId, bool includeChosenProgramsAndApplicantRef = false);
    public Task<Guid?> AddAdmission(Admission admission);
    public Task<bool> AddProgramPreference(ProgramPreference programPreference);

    public Task<ProgramPreference?> RetrieveProgramPreferenceById(Guid programPreferenceId,
        bool includeAdmissionAndApplicant = false);

    public Task<bool> RemoveProgramPreference(ProgramPreference programPreference);

    public Task<PaginatedList<AdmissionFullDto>> GetPaginatedAdmissions(string? name, Guid? programId,
        List<Guid> faculties, AdmissionStatus? admissionStatus,
        bool onlyNotTaken = false,
        bool onlyMine = false, Sorting sorting = Sorting.DateDesc,
        int page = 1, int pageSize = 10);
}