namespace ProductApi.Dtos.CategoryDtos
{
    public class CreateCategoryDto
    {
        public string CategoryName { get; set; }

        public int MinimumStock { get; set; }
    }
}
