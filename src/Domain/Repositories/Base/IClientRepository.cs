using Domain.Entities;

namespace Domain.Repositories.Base;

public interface IClientRepository : IRepository<Client>
{
    Task<Client> GetByCpf(string cpf);

}