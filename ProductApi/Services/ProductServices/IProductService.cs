using ProductApi.Dtos.ProductDtos;

namespace ProductApi.Services.ProductServices
{
    public interface IProductService
    {
        Task CreateProductAsync(CreateProductDto createProductDto);

        
        Task DeleteProductAsync(string id);

        
        Task<List<ResultProductDto>> GetAllProductsAsync();

        
        Task<GetByIdProductDto> GetByIdProductAsync(string id);

        
        Task UpdateProductAsync(UpdateProductDto updateProductDto);

        
        Task<List<ResultProductDto>> FilterProductsAsync(string? keyword, int? minStock, int? maxStock);

    }
}
