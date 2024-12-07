namespace ProductApi.Dtos.CategoryDtos
{
    public class UpdateCategoryDto
    {
        public string CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int MinimumStock { get; set; }
    }
}
