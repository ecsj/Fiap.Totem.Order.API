using Application.Interfaces;
using Domain.Base;
using Domain.Entities;
using Domain.Repositories.Base;
using Domain.Request;

namespace Application.UseCases;

public class ClientUseCase : IClientUseCase
{
    private readonly IClientRepository _clienteRepository;

    public ClientUseCase(IClientRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public IQueryable<Client> Get()
    {
        return _clienteRepository.Get();
    }

    public async Task<Client> GetById(Guid id)
    {
        return await _clienteRepository.GetByIdAsync(id);
    }

    public async Task<Client> GetByCpf(string cpf)
    {
        return await _clienteRepository.GetByCpf(cpf);
    }
    public async Task<Client> Add(ClientRequest request)
    {
        var client = Client.FromRequest(request);

        await _clienteRepository.AddAsync(client);

        await _clienteRepository.SaveChangesAsync();

        return client;
    }

    public async Task Update(Guid id, ClientRequest clientRequest)
    {
        var client = await GetById(id);

        if (client is null) throw new DomainException("Client não encontrado");

        await _clienteRepository.UpdateAsync(client);

        await _clienteRepository.SaveChangesAsync();

    }

    public async Task Delete(Guid id)
    {
        var client = await GetById(id);

        if (client is null) throw new DomainException("Client não encontrado");

        await _clienteRepository.DeleteAsync(client);

        await _clienteRepository.SaveChangesAsync();

    }
}