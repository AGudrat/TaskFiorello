using Fiorello.Models;


namespace Fiorello.ViewModel.Product
{
    public class ProductListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public string Image { get; set; }
        public string CategoryName { get; set; }

    }
}
