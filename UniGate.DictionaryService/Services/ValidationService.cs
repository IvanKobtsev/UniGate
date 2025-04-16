using UniGate.Common.Enums;
using UniGate.Common.Utilities;
using UniGate.DictionaryService.Interfaces;

namespace UniGate.DictionaryService.Services;

public class ValidationService(IDictionaryRepository dictionaryRepository) : IValidationService
{
    public async Task<Result<Guid>> ValidateProgramApplicationAndGetFacultyId(Guid programToChooseId,
        Guid? alreadyChosenProgramId,
        Guid? educationDocumentTypeId)
    {
        var programToChoose = await dictionaryRepository.GetEducationProgram(programToChooseId);

        if (programToChoose == null)
            return new Result<Guid>
            {
                Code = HttpCode.NotFound,
                Message = "Program with specified ID not found"
            };

        if (alreadyChosenProgramId != null)
        {
            var alreadyChosenProgram = await dictionaryRepository.GetEducationProgram(alreadyChosenProgramId.Value);

            if (alreadyChosenProgram == null)
                throw new InvalidOperationException(
                    "Already chosen program not found: AdmissionService database contains invalid data");

            var bothProgramsAreOnTheSameLevel =
                (await dictionaryRepository.GetAllEducationDocumentTypes()).Select(dt =>
                    dt.NextEducationLevels.Select(ela => ela.EducationLevel.IntegerId).ToList()).Any(list =>
                    list.Contains(programToChoose.EducationLevel.IntegerId) &&
                    list.Contains(alreadyChosenProgram.EducationLevel.IntegerId));

            if (programToChoose.EducationLevelId != alreadyChosenProgram.EducationLevelId &&
                !bothProgramsAreOnTheSameLevel)
                return new Result<Guid>
                {
                    Code = HttpCode.BadRequest,
                    Message =
                        "Cannot apply because chosen program is of different education level than already chosen program(s)"
                };
        }

        if (educationDocumentTypeId != null)
        {
            var educationDocumentType =
                await dictionaryRepository.GetEducationDocumentType(educationDocumentTypeId.Value);

            if (educationDocumentType == null)
                throw new InvalidOperationException(
                    "Education document type not found: AdmissionService database contains invalid data");

            var neededEducationLevel = programToChoose.EducationLevel.IntegerId;
            var nextEducationLevels = educationDocumentType.NextEducationLevels
                .Select(ela => ela.EducationLevel.IntegerId)
                .ToList();

            if (neededEducationLevel != educationDocumentType.EducationLevel.IntegerId &&
                !nextEducationLevels.Contains(neededEducationLevel))
                return new Result<Guid>
                {
                    Code = HttpCode.BadRequest,
                    Message =
                        "Cannot apply because chosen program is of different education level than uploaded education document type"
                };
        }

        return new Result<Guid>
        {
            Data = programToChoose.FacultyId
        };
        ;
    }
}