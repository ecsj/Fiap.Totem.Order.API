using Application.Interfaces;
using Domain.Base;
using Domain.Entities;
using Domain.Repositories.Base;
using Domain.Request;

namespace Application.UseCases;

public class ProductUseCase : IProductUseCase
{
    private readonly IRepository<Product> _productRepository;

    public ProductUseCase(IRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public IQueryable<Product> Get()
    {
        return _productRepository.Get();
    }

    public async Task<Product> GetById(Guid productId)
    {
        return await _productRepository.GetByIdAsync(productId);
    }

    public List<Product> GetByCategory(Category category)
    {
        return _productRepository.Get().Where(p => p.Category == category).ToList();
    }
    public async Task<Product> Add(ProductRequest request)
    {
        var product = Product.FromProductRequest(request);

        await _productRepository.AddAsync(product);

        await _productRepository.SaveChangesAsync();

        return product;
    }

    public async Task Update(Guid id, ProductRequest productRequest)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product != null)
        {
            await _productRepository.UpdateAsync(product);
        }

        _productRepository.UpdateAsync(product);
    }

    public async Task Delete(Guid productId)
    {
        var product = await GetById(productId);

        if (product is null) throw new DomainException("Produto não encontrado");

        await _productRepository.DeleteAsync(product);
    }
}