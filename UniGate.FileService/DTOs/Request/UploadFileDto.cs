namespace UniGateAPI.DTOs.Request;

public class UploadFileDto
{
    public required string FileName { get; set; }
    public required string ContentType { get; set; }
    public required byte[] Data { get; set; }
}