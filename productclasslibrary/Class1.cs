using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProductWebAPI.Controllers;
using ProductWebAPI.Data;
using ProductWebAPI.DTOs;
using ProductWebAPI.Model;
using ProductWebAPI.Services.ProductService;
using System;
using Xunit;

namespace productclasslibrary
{
   

    public class ProductControllerTests
    {
        private readonly Mock<Iproductservices> _mockService;
        // if not used, can be null

        public ProductControllerTests()
        {
            _mockService = new Mock<Iproductservices>();
            // or mock if needed
            
        }

        [Fact]
        public void CreateProduct_ReturnsBadRequest_WhenProductNameOrCreatedByIsInvalid()
        {
            // Arrange
            var controller = new ProductController(null, _mockService.Object);

            var dto = new Addproductdto { ProductName = "string", CreatedBy = "string" };

            // Act
            var result = controller.CreateProduct(dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateProduct_ReturnsBadRequest_WhenItemQuantityIsZero()
        {
            // Arrange
            var controller = new ProductController(null, _mockService.Object);
            var dto = new Addproductdto
            {
                ProductName = "Laptop",
                CreatedBy = "Dnyanesh",
                itemlist = new List<Item> { new Item { Quantity = 0 } }
            };

            // Act
            var result = controller.CreateProduct(dto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void CreateProduct_ReturnsOk_WhenValidProduct()
        {
            // Arrange
            var controller = new ProductController(null, _mockService.Object);
            var dto = new Addproductdto
            {
                ProductName = "Laptop",
                CreatedBy = "Dnyanesh",
                itemlist = new List<Item> { new Item { Quantity = 1 } }
            };

            // Act
            var result = controller.CreateProduct(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Product created successfully", okResult.Value);

            // Verify service call
            _mockService.Verify(s => s.AddProduct(dto), Times.Once);
        }

        [Fact]
        public void CreateProduct_ReturnsServerError_WhenExceptionThrown()
        {
            // Arrange
            var controller = new ProductController(null, _mockService.Object);
            var dto = new Addproductdto
            {
                ProductName = "Laptop",
                CreatedBy = "Dnyanesh",
                itemlist = new List<Item> { new Item { Quantity = 1 } }
            };

            _mockService.Setup(s => s.AddProduct(dto)).Throws(new Exception("DB error"));

            // Act
            var result = controller.CreateProduct(dto);

            // Assert
            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
        }


        [Fact]
        public void GetProductById_ProductExists_ReturnsOk()
        {
            // Arrange
          
            var options = new DbContextOptionsBuilder<Entityclass>()
                .UseInMemoryDatabase("TestDb")
                .Options;

            using var context = new Entityclass(options);

            context.products.Add(new Product { Id = 1, ProductName = "Laptop" });
            context.SaveChanges();

            var controller = new ProductController(context, _mockService.Object);

            // Act
            var result = controller.GetProductById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var product = Assert.IsType<Product>(okResult.Value);
            Assert.Equal("Laptop", product.ProductName);
        }

    }

}
