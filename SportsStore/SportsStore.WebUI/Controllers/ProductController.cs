using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Models;
using SportsStore.Domain.Entities;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public IProductRepository repository;
        public int Pagesize = 4;
        public ProductController(IProductRepository repository)
        {
            this.repository = repository; 
        }
        public ViewResult List(string category,int page=1)
        {
            ProductsListViewModel result = new ProductsListViewModel{Products = repository.Products.Where(p=>category==null||p.Category==category).OrderBy(p => p.ProductID).Skip((page - 1) * Pagesize).Take(Pagesize),
                                                                     PagingInfo = new PagingInfo {CurrentPage=page,ItemsPerPage=Pagesize,
                                                                                                  TotalItems=(category==null?repository.Products.Count():repository.Products.Where(p=>p.Category==category).Count())},
                                                                     CurrentCategory=category };
               
            return View(result);
        }

        public FileContentResult GetImage(int productId)
        {
            Product product=repository.Products.FirstOrDefault(p => p.ProductID == productId);
            if(product!=null)
            {
                return File(product.ImageData, product.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
    }
}