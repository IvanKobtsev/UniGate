using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Options;
using UniGate.Common.Exceptions;
using UniGate.DictionaryService.DTOs;
using UniGate.DictionaryService.DTOs.External;
using UniGate.DictionaryService.Interfaces;

namespace UniGate.DictionaryService.ExternalApiClient;

public class ExternalApiClient : IExternalApiClient
{
    private readonly HttpClient _http;
    private readonly ExternalApiOptions _options;

    public ExternalApiClient(HttpClient http, IOptions<ExternalApiOptions> options)
    {
        _http = http;
        _options = options.Value;
        _http.BaseAddress = new Uri(_options.BaseUrl);
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
            Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_options.Login}:{_options.Password}")));
    }

    public async Task<List<EducationLevelModel>> ImportEducationLevelsAsync()
    {
        var response =
            await _http.GetFromJsonAsync<List<EducationLevelModel>>(
                "/api/dictionary/education_levels");

        if (response == null) throw new NotFoundException("Import failed: service responded with null");

        return response;
    }

    public async Task<List<FacultyModel>> ImportFacultiesAsync()
    {
        var response =
            await _http.GetFromJsonAsync<List<FacultyModel>>(
                "/api/dictionary/faculties");

        if (response == null) throw new NotFoundException("Import failed: service responded with null");

        return response;
    }

    public async Task<List<EducationDocumentTypeModel>> ImportEducationDocumentTypesAsync()
    {
        var response =
            await _http.GetFromJsonAsync<List<EducationDocumentTypeModel>>(
                "/api/dictionary/document_types");

        if (response == null) throw new NotFoundException("Import failed: service responded with null");

        return response;
    }

    public async Task<ProgramPagedListModel> ImportEducationProgramsAsync(int page = 1)
    {
        var response =
            await _http.GetFromJsonAsync<ProgramPagedListModel>(
                "/api/dictionary/programs?page=" + page + "&size=" + _options.PaginationDivision);

        if (response == null) throw new NotFoundException("Import failed: service responded with null");

        return response;
    }
}