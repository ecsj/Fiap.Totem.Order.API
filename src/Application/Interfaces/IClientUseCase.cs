using Domain.Entities;
using Domain.Request;

namespace Application.Interfaces;

public interface IClientUseCase : IBaseUseCase<Client, Client>
{
    Task<Client> GetByCpf(string cpf);
}