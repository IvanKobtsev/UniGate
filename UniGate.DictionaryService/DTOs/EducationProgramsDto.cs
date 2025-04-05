namespace UniGate.DictionaryService.DTOs;

public class EducationProgramsDto
{
    public List<EducationProgramDto> Programs { get; set; }
    public PaginationDto Pagination { get; set; }
}