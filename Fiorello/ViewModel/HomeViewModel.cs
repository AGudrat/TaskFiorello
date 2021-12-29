using Fiorello.Models;
using System.Collections.Generic;

namespace Fiorello.ViewModel
{
    public class HomeViewModel
    {
        public List<Models.Slider> Slider { get; set; }
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
        public About About { get; set; }
        public List<Features> Features { get; set; }
        public AboutVideo AboutVideo { get; set; }
        public List<Experts> Experts { get; set; }
        public List<Blog> Blogs { get; set; }
    }
}
