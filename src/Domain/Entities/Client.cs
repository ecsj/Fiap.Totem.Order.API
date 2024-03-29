using Domain.Base;
using Domain.Request;

namespace Domain.Entities;

public class Client : Entity, IAggregateRoot
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string CPF { get; set; }

    public Client(Guid id, string nome, string cpf, string email)
    {
        Id = id;
        Name = nome;
        CPF = cpf;
        Email = email;

        ValidateEntity();
    }

    public Client() { }

    public static Client FromRequest(ClientRequest clientRequest)
    {
        return new Client
        {
            Name = clientRequest.Name,
            Email = clientRequest.Email,
            CPF = clientRequest.CPF
        };
    }

    public void ValidateEntity()
    {
        AssertionConcern.AssertArgumentNotEmpty(Name, "O nome não pode ser vazio");
        AssertionConcern.AssertArgumentNotEmpty(Email, "O Email não pode ser vazio");
        AssertionConcern.AssertArgumentNotEmpty(CPF, "O CPF não pode ser vazio");
    }
}