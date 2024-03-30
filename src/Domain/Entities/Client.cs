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
    }

    public Client() { }
}