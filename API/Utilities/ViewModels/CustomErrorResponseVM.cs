namespace API.Utilities.ViewModels
{
    // Record -> immutable -> Setelah diinisiasi, data tidak bisa dirubah
    public record CustomErrorResponseVM(
        int Code, 
        string Status, 
        string Message, 
        string ErrorDetails);

    // Kalau class masih bisa diubah meskipun strukturnya = record
    //public class CustomClass
    //{
    //    public CustomClass(int Code, string Status, string Message, string ErrorDetails)
    //    {
            
    //    }

    //    public int Code { get; set; }
    //    public string? Status { get; set; }
    //    public string? Message { get; set; }
    //    public string? ErrorDetails { get; set; }
    //}
}
