using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlfavoxAPI;
using AlfavoxAPI.Controllers;
using System.Diagnostics;
using AlfavoxAPI.Models;
using Moq;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;

namespace AlfavoxAPI.Tests.Controllers
{
    public abstract class BaseProductControllerTests
    {
        protected readonly List<Product> products;
        protected readonly Mock<IProductService> mockService;
        protected readonly ProductController controllerUnderTest;

        protected BaseProductControllerTests(List<Product> items)
        { 
            products = items;
            mockService = new Mock<IProductService>();
            mockService.Setup(svc => svc.FindAsync(1)).ReturnsAsync(new Product() { Id=1, ProductName = "Pen" });
            controllerUnderTest = new ProductController(mockService.Object);
        }
    }

    [TestClass]
    public class ProductControllerTestAsync : BaseProductControllerTests
    { 

        private static readonly Product product_first = new Product() { Id = 1, ProductName = "Pen" };
        private static readonly Product product_second = new Product() { Id = 2, ProductName = "Pencil" };

        public ProductControllerTestAsync() : base(new List<Product>() { product_first, product_second })
        { 
        
        }

        [TestMethod]
        public async Task GetSingleProduct()
        {
            var result = await controllerUnderTest.GetProduct(1);
            Assert.IsNotNull(result);
        }


        /*
        [TestMethod]
        public async Task GetSomeProducts()
        {                        
            //Act
            var result = await controllerUnderTest.GetProducts("1-2-3");

            //Assert
            Assert.IsNotNull(result);
        }


        */
    }
}
