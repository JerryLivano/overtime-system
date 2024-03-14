namespace API.DTOs.Overtimes
{
    public record OvertimeDownloadResponseDto(
        byte[] Document,
        string ContentType,
        string FileName);
}
