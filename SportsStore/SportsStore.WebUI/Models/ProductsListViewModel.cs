using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SportsStore.Domain.Entities;
namespace SportsStore.WebUI.Models
{
    public class ProductsListViewModel
    {
        public PagingInfo PagingInfo { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public string CurrentCategory { get; set; } 
    }
}