namespace Domain.Request;

public record struct ClientRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string CPF { get; set; }
}