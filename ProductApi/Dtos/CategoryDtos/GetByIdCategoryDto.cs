namespace ProductApi.Dtos.CategoryDtos
{
    public class GetByIdCategoryDto
    {
        public string CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int MinimumStock { get; set; }
    }
}
