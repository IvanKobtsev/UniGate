using Quartz;
using UniGate.DictionaryService.Enums;
using UniGate.DictionaryService.Interfaces;

namespace UniGate.DictionaryService.Services;

public class JobService(ISchedulerFactory schedulerFactory) : IJobService
{
    public async Task StartImport(ImportType importType)
    {
        var scheduler = await schedulerFactory.GetScheduler();
        var jobKey = new JobKey("DictionaryImportJob");

        var jobDataMap = new JobDataMap
        {
            { "ImportType", importType }
        };

        await scheduler.TriggerJob(jobKey, jobDataMap);
    }
}