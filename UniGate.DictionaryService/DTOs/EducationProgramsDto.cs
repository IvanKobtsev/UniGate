using UniGate.DictionaryService.DTOs.Common;

namespace UniGate.DictionaryService.DTOs.Response;

public class EducationProgramsDto
{
    public List<EducationProgramDto> EducationPrograms { get; set; }
    public PaginationDto Pagination { get; set; }
}