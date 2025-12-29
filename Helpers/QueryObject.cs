using System.ComponentModel.DataAnnotations;

namespace e_commerce_basic.Helpers
{
    public class QueryObject
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;
        [Required]
        [Range(1, 100)]
        public int PageSize { get; set; } = 10;
        public int SubCategoryId { get; set; } = 0;
        public string? Search { get; set; } = null;
        public string? SortBy { get; set; } = null;
        public bool IsDescending { get; set; } = false;

    }
}