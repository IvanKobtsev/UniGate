using UniGate.Common.DTOs;

namespace UniGate.DictionaryService.DTOs;

public class EducationProgramsPagedListDto
{
    public List<EducationProgramDto> Programs { get; set; }
    public PaginationDto Pagination { get; set; }
}