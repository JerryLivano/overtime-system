namespace API.Utilities.ViewModels
{
    public record ListResponseVM<T>(
        int Code,
        string Status,
        string Message,
        IEnumerable<T> Data);
}
