using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SportsStore.Domain.Entities
{
    public class Product
    {   [HiddenInput(DisplayValue=false)]
        public int ProductID { get; set; }
        [Required(ErrorMessage ="Vendosni emrin")]
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage ="Vendosni pershkrimin")]
        public string Description { get; set; }

        [Required]
        [Range(0.01,Double.MaxValue,ErrorMessage ="Vendosni nje vlere pozitive")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Vendosni kategorine")]
        public string Category { get; set; }
        public string ImageMimeType { get; set; }
        public byte[] ImageData { get; set; }
    }
}
