namespace ProductApi.Dtos.ProductDtos
{
    public class ResultProductDto
    {
        public string ProductId { get; set; }

        public string Title { get; set; }
        public string? Description { get; set; }
        public int StockQuantity { get; set; }
        public bool IsLive { get; set; }


        public string CategoryId { get; set; }
    }
}
