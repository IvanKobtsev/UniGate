using UniGate.Common.Utilities;

namespace UniGate.DictionaryService.Interfaces;

public interface IValidationService
{
    public Task<Result<Guid>> ValidateProgramApplicationAndGetFacultyId(Guid programToChoose,
        Guid? alreadyChosenProgram,
        Guid? educationDocumentTypeId);
}