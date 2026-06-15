using Microsoft.EntityFrameworkCore;
using ProductWebAPI.Data;
using ProductWebAPI.DTOs;
using ProductWebAPI.Model;
using System;

namespace ProductWebAPI.Services.ProductService
{
    public class productservices:Iproductservices
    {
        
        private readonly Entityclass _context;

        public productservices(Entityclass context)
        {
            
            _context = context;
        }

        public Addproductdto AddProduct(Addproductdto product)
        {
            Product pd = new Product();

            pd.ProductName = product.ProductName;
            pd.CreatedOn = DateTime.Now;
            pd.CreatedBy = product.CreatedBy;
            pd.Items = product.itemlist;

            _context.products.Add(pd);
            _context.SaveChanges();
       
            return product;
        }

    }
}
