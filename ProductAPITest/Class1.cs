using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductWebAPI.Data;
using ProductWebAPI.DTOs;
using ProductWebAPI.Model;
using ProductWebAPI.Services.ProductService;
using Xunit;

namespace ProductAPITest
{
    public class Class1
    {
        private readonly Mock<Iproductservices> _mockService;
       // if not used, can be null

        public Class1()
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

    }
}
