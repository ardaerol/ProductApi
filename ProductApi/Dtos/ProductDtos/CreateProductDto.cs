namespace ProductApi.Dtos.ProductDtos
{
    public class CreateProductDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int StockQuantity { get; set; }
       


        public string CategoryId { get; set; }
    }
}
