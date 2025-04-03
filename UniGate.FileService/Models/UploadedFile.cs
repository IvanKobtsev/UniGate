namespace UniGate.FileService.Models;

public class UploadedFile
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
}