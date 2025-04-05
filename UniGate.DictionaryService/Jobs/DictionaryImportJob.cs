using Quartz;
using UniGate.DictionaryService.Enums;
using UniGate.DictionaryService.Interfaces;

namespace UniGate.DictionaryService.Jobs;

public class DictionaryImportJob(IImportService importService) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var importType = (ImportType)context.MergedJobDataMap.GetIntValue("ImportType");
        await importService.Import(importType);
    }
}