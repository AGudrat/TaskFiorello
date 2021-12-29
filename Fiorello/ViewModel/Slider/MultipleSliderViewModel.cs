using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fiorello.ViewModel.Slider
{
    public class MultipleSliderViewModel
    {
            public int Id { get; set; }

            [Required]
            public List<IFormFile> Photos { get; set; }
        
    }
}
