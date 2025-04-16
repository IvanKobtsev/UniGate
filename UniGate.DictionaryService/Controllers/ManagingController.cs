using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UniGate.Common.Enums;
using UniGate.DictionaryService.Enums;
using UniGate.DictionaryService.Interfaces;

namespace UniGate.DictionaryService.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/v1/dictionary")]
public class ManagingController(
    IJobService jobService,
    IImportService importService,
    IDictionaryService dictionaryService) : ControllerBase
{
    [HttpGet("status")]
    [SwaggerOperation(Summary = "Get dictionary import's status")]
    public async Task<IActionResult> GetImportStatus()
    {
        var importStatus = await importService.GetImportStatus();

        if (importStatus == null) return NoContent();

        return Ok(importStatus);
    }

    [HttpPost("import")]
    [SwaggerOperation(Summary = "Send request for importing dictionary using UPSERT")]
    public async Task<IActionResult> UpsertDictionary()
    {
        if (await importService.IsImporting()) return BadRequest(new { message = "Import is already in progress" });

        await jobService.StartImport(ImportType.Upsert);

        return Accepted(new { message = "Import started successfully" });
    }

    [HttpPost("soft_reset")]
    [SwaggerOperation(Summary = "Send request for importing dictionary with soft reset")]
    public async Task<IActionResult> ImportDictionary()
    {
        if (await importService.IsImporting()) return BadRequest(new { message = "Import is already in progress" });

        await jobService.StartImport(ImportType.SoftReset);

        return Accepted(new { message = "Import started successfully" });
    }

    [HttpGet("import_history")]
    [SwaggerOperation(Summary = "Get a list of all dictionary imports ever made")]
    public async Task<IActionResult> GetImports([Range(1, int.MaxValue)] [FromQuery] int currentPage = 1,
        [Range(1, int.MaxValue)] [FromQuery] int pageSize = 10, [FromQuery] Sorting sorting = Sorting.DateDesc)
    {
        return Ok(await importService.GetImportHistory(currentPage, pageSize, sorting));
    }

    // [HttpDelete("import_history/clear")]
    // [SwaggerOperation(Summary = "Clear dictionary imports history")]
    // public IActionResult ClearImportHistory()
    // {
    //     importService.ClearHistory();
    // }

    // [HttpDelete("hard_reset")]
    // [SwaggerOperation(Summary = "Clear dictionary database, including import history")]
    // public IActionResult HardReset()
    // {
    //     importService.HardReset();
    // }
}