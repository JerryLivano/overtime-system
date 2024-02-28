namespace API.Utilities.ViewModels
{
    // Record -> immutable -> Setelah diinisiasi, data tidak bisa dirubah
    public record CustomErrorResponseVM(
        int Code,
        string Status,
        string Message,
        string ErrorDetails);
}