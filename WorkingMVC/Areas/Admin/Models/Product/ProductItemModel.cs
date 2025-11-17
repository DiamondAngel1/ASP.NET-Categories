namespace WorkingMVC.Areas.Admin.Models.Product
{
    public class ProductItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<string> Images { get; set; } = new();
        public string CategoryName { get; set; } = string.Empty;
    }
}
