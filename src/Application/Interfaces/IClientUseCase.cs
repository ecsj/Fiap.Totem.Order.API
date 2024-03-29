using Domain.Entities;
using Domain.Request;

namespace Application.Interfaces;

public interface IClientUseCase : IBaseUseCase<ClientRequest, Client>
{
    Task<Client> GetByCpf(string cpf);
}