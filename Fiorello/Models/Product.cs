using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Fiorello.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*Xahiş olunur adını daxil edəsiniz."), MaxLength(25, ErrorMessage = "*Ad 25 simvoldan çox ola bilməz.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "*Xahiş olunur qiyməti daxil edəsiniz."), DataType(DataType.Currency,ErrorMessage ="*Rəqəm daxil edin.")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "*Xahiş olunur sayı daxil edəsiniz.")]
        public int Count { get; set; }
        public bool IsDeleted { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<ProductImage> Images { get; set; }
    }
}
