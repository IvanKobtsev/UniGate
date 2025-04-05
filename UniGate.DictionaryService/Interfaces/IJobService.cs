using UniGate.DictionaryService.Enums;

namespace UniGate.DictionaryService.Interfaces;

public interface IJobService
{
    public Task StartImport(ImportType importType);
}