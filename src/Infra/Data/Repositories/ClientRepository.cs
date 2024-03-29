using Domain.Entities;
using Domain.Repositories.Base;
using Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class ClientRepository : Repository<Client>, IClientRepository
{
    public ClientRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        
    }
    public async Task<Client> GetByCpf(string cpf)
    {
        return await _dbContext.Set<Client>().FirstOrDefaultAsync(c => c.CPF == cpf);
    }
}