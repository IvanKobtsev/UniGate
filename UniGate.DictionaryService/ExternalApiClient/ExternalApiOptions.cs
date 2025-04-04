namespace UniGate.DictionaryService.ExternalApiClient;

public class ExternalApiOptions
{
    public required string BaseUrl { get; set; }
    public required string Login { get; set; }
    public required string Password { get; set; }
    public int PaginationDivision { get; set; }
}