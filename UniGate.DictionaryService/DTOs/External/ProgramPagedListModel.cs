using UniGate.Common.DTOs;

namespace UniGate.DictionaryService.DTOs.External;

public class ProgramPagedListModel
{
    public List<EducationProgramModel> Programs { get; set; }
    public PaginationDto Pagination { get; set; }
}