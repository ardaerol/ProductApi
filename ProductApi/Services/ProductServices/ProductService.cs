using AutoMapper;
using MongoDB.Driver;
using ProductApi.DbSettings;
using ProductApi.Dtos.ProductDtos;
using ProductApi.Entities;
using ProductApi.Services.CategoryServices;

namespace ProductApi.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IMongoCollection<Product> _productCollection;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;

       

        public ProductService(IMapper mapper, IDatabaseSettings databaseSettings, ICategoryService categoryService)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _productCollection = database.GetCollection<Product>(databaseSettings.ProductCollectionName);
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public async Task CreateProductAsync(CreateProductDto createProductDto)
        {
            
            if (string.IsNullOrWhiteSpace(createProductDto.Title))
            {
                throw new ArgumentException("Product title cannot be null or empty.");
            }

            if(createProductDto.Title.Length > 200)
            {
                throw new ArgumentException("Title cannot exceed 200 characters.");
            }

            if (createProductDto.StockQuantity <= 0) {
                throw new ArgumentException("Stock quantity must be greater than zero.");
            }

            
            if (string.IsNullOrEmpty(createProductDto.CategoryId))
            {
                throw new ArgumentException("Product must have a valid category.");
            }

            
            var product = _mapper.Map<Product>(createProductDto);
            var category = await _categoryService.GetByIdCategoryAsync(product.CategoryId);
            if (product.StockQuantity > category.MinimumStock) {
                product.IsLive = true;
            }
            else
            {
                product.IsLive = false;
            }
            
            await _productCollection.InsertOneAsync(product);
        }

        public async Task DeleteProductAsync(string id)
        {
            var deleteResult = await _productCollection.Find<Product>(x => x.ProductId == id).FirstOrDefaultAsync();
            var category = await _categoryService.GetByIdCategoryAsync(deleteResult.CategoryId);
            if (deleteResult == null) {
                throw new ArgumentException("not found product");
            }
            deleteResult.StockQuantity = deleteResult.StockQuantity-1;
            if (deleteResult.StockQuantity > category.MinimumStock)
            {
                deleteResult.IsLive = true;
            }
            else
            {
                deleteResult.IsLive = false;
            }

           await _productCollection.ReplaceOneAsync(x => x.ProductId == deleteResult.ProductId, deleteResult);

        }

        public async Task<List<ResultProductDto>> GetAllProductsAsync()
        {
            var values = await _productCollection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultProductDto>>(values);
        }

        public async Task<GetByIdProductDto> GetByIdProductAsync(string id)
        {
            var product = await _productCollection.Find<Product>(x => x.ProductId == id).FirstOrDefaultAsync();

            if (product == null)
            {
                throw new KeyNotFoundException("Product not found for the given id.");
            }

            return _mapper.Map<GetByIdProductDto>(product);
        }

        public async Task UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            if (string.IsNullOrWhiteSpace(updateProductDto.Title))
            {
                throw new ArgumentException("Product title cannot be null or empty.");
            }

            if (updateProductDto.Title.Length > 200)
            {
                throw new ArgumentException("Title cannot exceed 200 characters.");
            }

            if (updateProductDto.StockQuantity <= 0)
            {
                throw new ArgumentException("Stock quantity must be greater than zero.");
            }
                

            var product = _mapper.Map<Product>(updateProductDto);
            var category = await _categoryService.GetByIdCategoryAsync(product.CategoryId);
            if (product.StockQuantity > category.MinimumStock)
            {
                product.IsLive = true;
            }
            else
            {
                product.IsLive = false;
            }
            await _productCollection.ReplaceOneAsync(x => x.ProductId == updateProductDto.ProductId, product);


        }

        public async Task<List<ResultProductDto>> FilterProductsAsync(string? keyword, int? minStock, int? maxStock)
        {
            var filterBuilder = Builders<Product>.Filter;
            var filter = filterBuilder.Empty;

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var keywordFilter = filterBuilder.Or(
                    filterBuilder.Regex("Title", new MongoDB.Bson.BsonRegularExpression(keyword, "i")),
                    filterBuilder.Regex("Description", new MongoDB.Bson.BsonRegularExpression(keyword, "i")),
                     filterBuilder.Regex("CategoryId", new MongoDB.Bson.BsonRegularExpression(keyword, "i"))
                );
                filter &= keywordFilter;
            }

            if (minStock.HasValue)
            {
                filter &= filterBuilder.Gte(x => x.StockQuantity, minStock.Value);
            }

            if (maxStock.HasValue)
            {
                filter &= filterBuilder.Lte(x => x.StockQuantity, maxStock.Value);
            }

            var filteredProducts = await _productCollection.Find(filter).ToListAsync();
            return _mapper.Map<List<ResultProductDto>>(filteredProducts);
        }
    }
}
