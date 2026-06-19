using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductWebAPI;
using ProductWebAPI.Data;
using ProductWebAPI.DTOs;
using ProductWebAPI.Model;

using ProductWebAPI.Services.ProductService;
using System;

[ApiController]

[Route("api/v{version:apiVersion}/products")]
[ApiVersion("1.0")]
[ServiceFilter(typeof(LogActionFilter))]
public class ProductController : ControllerBase
{
    private readonly Entityclass _context;
    private readonly Iproductservices _iproduct;

    public ProductController(Entityclass context, Iproductservices _iproductservices)
    {
        _context = context;
        _iproduct = _iproductservices;
    }

    [Authorize]
    [HttpPost("CreateProduct")]
    public IActionResult CreateProduct(Addproductdto product)
    {
        try
        {
            if (product.ProductName == "string" || product.CreatedBy == "string")
            {
                return BadRequest("Please provide valid values instead of default placeholders.");
            }
            if (product.itemlist[0].Quantity==0)
            {
                return BadRequest("Item quantity cannot be 0. Please provide valid values.");
            }
            product.CreatedOn = DateTime.Now;
            _iproduct.AddProduct(product);
            return Ok("Product created successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error saving product: {ex.Message}");
        }

    }

    [Authorize]
    [HttpGet]
    public IActionResult GetAllProducts()
    {
        var products = _context.products.Include(p => p.Items).ToList();
        return Ok(products);
    }

    [Authorize]
    [HttpGet("{id}")]
    public IActionResult GetProductById(int id)
    {
       

        var product = _context.products.Include(p => p.Items)
                                       .FirstOrDefault(p => p.Id == id);
        if (product == null) return NotFound();
        return Ok(product);


    }

    [Authorize]
    [HttpPut("{id}")]
    public IActionResult UpdateProduct(int id, updateproductdto updatedProduct)
    {
        var product = _context.products.FirstOrDefault(p => p.Id == id);
        if (product == null) 
            return NotFound();

        product.ProductName = updatedProduct.ProductName;

        product.ModifiedBy = updatedProduct.ModifiedBy;
        product.ModifiedOn = DateTime.Now;

        _context.SaveChanges();
        return Ok(product);
    }

    //using storedProcedure
    [Authorize]
    [HttpPut("Update")]
    public IActionResult Update(int id1, updateproductdto up)
    {
        //var product = _context.products.Include(p => p.Items).FirstOrDefault(p => p.Id == id1);

        var item = up.itemlist.ToList();
       
        if (item == null)
            return BadRequest("No item provided for update");

        var rowsAffected = 0;
        foreach (var items in item)
        {
           rowsAffected = _context.Database.ExecuteSqlRaw(
         "EXEC UpdateProductAndItem @ProductId={0}, @NewProductName={1}, @ModifiedBy={2}, @ItemId={3}, @NewQuantity={4}",
         id1, up.ProductName, up.ModifiedBy, items.Id, items.Quantity);

        }

        if (rowsAffected == 0)
            return NotFound("No matching product/item found");

        return Ok("Product and item updated successfully");
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        var product = _context.products
                         .Include(p => p.Items)
                         .FirstOrDefault(p => p.Id == id);
        if (product == null) return NotFound();

        _context.products.Remove(product);
        _context.SaveChanges();
        return Ok("Product deleted successfully");
    }
}
