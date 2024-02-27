namespace API.Utilities.ViewModels
{
    public record SingleResponseVM<T>(
        int Code,
        string Status,
        string Message,
        T Data);
}
