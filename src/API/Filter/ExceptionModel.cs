namespace API.Filter;

public struct ExceptionModel
{
    public string message { get; set; }
    public string detail { get; set; }
    public string Request { get; set; }
    public string Response { get; set; }
}
