using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fiorello.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "*Xahiş olunur kateqoriyanı daxil edəsiniz."), MaxLength(25, ErrorMessage = "*Kateqoriya 25 simvoldan çox ola bilməz.")]
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
